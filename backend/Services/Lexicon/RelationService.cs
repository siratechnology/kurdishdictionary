using backend.Data;
using backend.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Services.Lexicon;

/// <summary>
/// Creates relations, and their inverses.
///
/// Saving a ڕەگ writes the داڕێژراو لێی edge back automatically; saving a symmetric type writes the
/// mirror. Nobody enters an edge twice, and the graph cannot end up half-directed because someone
/// forgot the other side.
///
/// The inverse is attributed to the SAME user and produces its own RelationAdded event, so the
/// ledger shows exactly what was created and by whom — an auto-created edge is still that person's
/// contribution, not an anonymous system write.
/// </summary>
public class RelationService
{
    private readonly AppDbContext _db;

    public RelationService(AppDbContext db) => _db = db;

    public async Task<WordRelation> AddWordRelationAsync(
        int fromWordId, int toWordId, int typeId, CancellationToken ct = default)
    {
        if (fromWordId == toWordId)
            throw new InvalidOperationException("وشەیەک ناتوانێت پەیوەندی بە خۆیەوە هەبێت.");

        var type = await _db.RelationTypes.FirstOrDefaultAsync(t => t.Id == typeId, ct)
                   ?? throw new InvalidOperationException("جۆری پەیوەندی نەدۆزرایەوە.");

        if (type.Scope != RelationScope.Word)
            throw new InvalidOperationException($"«{type.NameKu}» پەیوەندییەکی مانایە، نەک وشەیی.");

        var edge = await Upsert(fromWordId, toWordId, type.Id, isAutoInverse: false, ct);

        // Symmetric types are their own inverse; directional ones point at their opposite. Either
        // way the far edge is created here, never left to the caller.
        var inverseTypeId = type.IsSymmetric ? type.Id : type.InverseId;
        if (inverseTypeId is { } inv)
            await Upsert(toWordId, fromWordId, inv, isAutoInverse: true, ct);

        await _db.SaveChangesAsync(ct);
        return edge;
    }

    public async Task RemoveWordRelationAsync(int relationId, CancellationToken ct = default)
    {
        var edge = await _db.WordRelations
            .Include(r => r.Type)
            .FirstOrDefaultAsync(r => r.Id == relationId, ct);

        if (edge is null) return;

        // Remove the pair. Leaving the inverse behind would make the graph assert something the
        // teacher just retracted.
        var inverseTypeId = edge.Type.IsSymmetric ? edge.TypeId : edge.Type.InverseId;
        if (inverseTypeId is { } inv)
        {
            var mirror = await _db.WordRelations.FirstOrDefaultAsync(
                r => r.FromWordId == edge.ToWordId && r.ToWordId == edge.FromWordId && r.TypeId == inv, ct);

            if (mirror is not null) _db.WordRelations.Remove(mirror);
        }

        _db.WordRelations.Remove(edge);
        await _db.SaveChangesAsync(ct);
    }

    // ═══════════════════════════════════════════════════════════════════════
    // Sense scope — هاومانا, پێچەوانە, مانای گشتیتر, مانای وردتر
    //
    // The same two operations, against SenseRelation. Kept as a parallel pair rather than folded
    // into the word ones behind a flag: the two tables point at different things on purpose (see
    // WordRelation's remarks), and a shared method would need the caller to pass "which id kind"
    // on every call, which is the distinction leaking out anyway with none of the type safety.
    // ═══════════════════════════════════════════════════════════════════════

    public async Task<SenseRelation> AddSenseRelationAsync(
        int fromSenseId, int toSenseId, int typeId, CancellationToken ct = default)
    {
        if (fromSenseId == toSenseId)
            throw new InvalidOperationException("مانایەک ناتوانێت پەیوەندی بە خۆیەوە هەبێت.");

        var type = await _db.RelationTypes.FirstOrDefaultAsync(t => t.Id == typeId, ct)
                   ?? throw new InvalidOperationException("جۆری پەیوەندی نەدۆزرایەوە.");

        if (type.Scope != RelationScope.Sense)
            throw new InvalidOperationException($"«{type.NameKu}» پەیوەندییەکی وشەییە، نەک مانایی.");

        // Both senses must exist. Without this the FK failure surfaces as a 500 on SaveChanges
        // instead of the sentence that says which side was wrong.
        var found = await _db.Senses.CountAsync(s => s.Id == fromSenseId || s.Id == toSenseId, ct);
        if (found < 2) throw new InvalidOperationException("مانایەک لە مانەکان نەدۆزرایەوە.");

        var edge = await UpsertSense(fromSenseId, toSenseId, type.Id, isAutoInverse: false, ct);

        var inverseTypeId = type.IsSymmetric ? type.Id : type.InverseId;
        if (inverseTypeId is { } inv)
            await UpsertSense(toSenseId, fromSenseId, inv, isAutoInverse: true, ct);

        await _db.SaveChangesAsync(ct);
        return edge;
    }

    public async Task RemoveSenseRelationAsync(int relationId, CancellationToken ct = default)
    {
        var edge = await _db.SenseRelations
            .Include(r => r.Type)
            .FirstOrDefaultAsync(r => r.Id == relationId, ct);

        if (edge is null) return;

        var inverseTypeId = edge.Type.IsSymmetric ? edge.TypeId : edge.Type.InverseId;
        if (inverseTypeId is { } inv)
        {
            var mirror = await _db.SenseRelations.FirstOrDefaultAsync(
                r => r.FromSenseId == edge.ToSenseId && r.ToSenseId == edge.FromSenseId && r.TypeId == inv, ct);

            if (mirror is not null) _db.SenseRelations.Remove(mirror);
        }

        _db.SenseRelations.Remove(edge);
        await _db.SaveChangesAsync(ct);
    }

    private async Task<SenseRelation> UpsertSense(int fromId, int toId, int typeId, bool isAutoInverse, CancellationToken ct)
    {
        var existing = await _db.SenseRelations
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(r => r.FromSenseId == fromId && r.ToSenseId == toId && r.TypeId == typeId, ct);

        if (existing is not null)
        {
            if (existing.IsDeleted)
            {
                existing.IsDeleted = false;
                existing.DeletedAt = null;
                existing.DeletedByUserId = null;
            }
            return existing;
        }

        var edge = new SenseRelation
        {
            FromSenseId = fromId,
            ToSenseId = toId,
            TypeId = typeId,
            IsAutoInverse = isAutoInverse,
        };

        _db.SenseRelations.Add(edge);
        return edge;
    }

    /// <summary>
    /// Adds the edge, or resurrects it if a previous removal soft-deleted it. Inserting a second
    /// row would violate the unique index on (from, to, type).
    /// </summary>
    private async Task<WordRelation> Upsert(int fromId, int toId, int typeId, bool isAutoInverse, CancellationToken ct)
    {
        var existing = await _db.WordRelations
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(r => r.FromWordId == fromId && r.ToWordId == toId && r.TypeId == typeId, ct);

        if (existing is not null)
        {
            if (existing.IsDeleted)
            {
                existing.IsDeleted = false;
                existing.DeletedAt = null;
                existing.DeletedByUserId = null;
            }
            return existing;
        }

        var edge = new WordRelation
        {
            FromWordId = fromId,
            ToWordId = toId,
            TypeId = typeId,
            IsAutoInverse = isAutoInverse,
        };

        _db.WordRelations.Add(edge);
        return edge;
    }
}
