using backend.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Data;

/// <summary>
/// Schema v3 — the taxonomy engine (پڕۆمپت ٣). Kept out of <see cref="AppDbContext"/> so the new
/// model can be read as one piece rather than interleaved with the tables it will eventually
/// replace.
///
/// Two things are seeded here and nothing else:
///   · the seven parts of speech — empty named records the team can rename
///   · the relation types — because Scope and Inverse are structural, not editorial
///
/// Everything else ships EMPTY. The app must work correctly against a completely empty taxonomy:
/// a part of speech with zero configured axes shows headword, definition and example, and saves.
/// That state is day one, not an edge case.
/// </summary>
public static class TaxonomyModelConfiguration
{
    // Stable ids so the seed is idempotent and history can point at them forever.
    private const int PosNoun = 1, PosVerb = 2, PosAdjective = 3, PosAdverb = 4,
                      PosPronoun = 5, PosParticle = 6, PosInfinitive = 7;

    public static void Configure(ModelBuilder b)
    {
        ConfigurePartsOfSpeech(b);
        ConfigureFeatures(b);
        ConfigureDomains(b);
        ConfigureSenses(b);
        ConfigureFormsAndRelations(b);
        ConfigureOperations(b);
        ConfigureSoftDeleteFilters(b);
    }

    /// <summary>The operations room (پڕۆمپت ٧): claims, disagreements, consistency samples.</summary>
    private static void ConfigureOperations(ModelBuilder b)
    {
        b.Entity<SenseClaim>(e =>
        {
            e.HasOne(x => x.Sense).WithMany().HasForeignKey(x => x.SenseId).OnDelete(DeleteBehavior.Cascade);
            e.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Restrict);

            // One live claim per sense. Filtered on ReleasedAt so the history of past claims can
            // stay in the table — an expired claim is still a fact about who was working where.
            e.HasIndex(x => x.SenseId).IsUnique().HasFilter("[ReleasedAt] IS NULL");
            e.HasIndex(x => new { x.UserId, x.ExpiresAt });
        });

        b.Entity<SenseDisagreement>(e =>
        {
            e.HasOne(x => x.Sense).WithMany().HasForeignKey(x => x.SenseId).OnDelete(DeleteBehavior.Cascade);
            e.HasOne(x => x.Axis).WithMany().HasForeignKey(x => x.AxisId).OnDelete(DeleteBehavior.NoAction);

            // Restrict on both: a disagreement names two people, and it stops being evidence of
            // anything the moment one of those names can be deleted out of it.
            e.HasOne(x => x.FirstUser).WithMany().HasForeignKey(x => x.FirstUserId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.SecondUser).WithMany().HasForeignKey(x => x.SecondUserId).OnDelete(DeleteBehavior.NoAction);

            e.HasIndex(x => new { x.SenseId, x.ResolvedAt });
        });

        b.Entity<ConsistencySample>(e =>
        {
            e.HasOne(x => x.Sense).WithMany().HasForeignKey(x => x.SenseId).OnDelete(DeleteBehavior.Cascade);
            e.HasIndex(x => new { x.ReadAt, x.SampledAt });
        });
    }

    private static void ConfigurePartsOfSpeech(ModelBuilder b)
    {
        b.Entity<PartOfSpeech>(e =>
        {
            e.HasIndex(x => x.Code).IsUnique();
            e.HasIndex(x => x.SortOrder);

            // The seven. Names are the team's to correct — the deck itself has typos (هەوەڵناو,
            // هەوەڵکار) and the live database uses the wrong terms (سیفەت, ئاوەڵناو), so these are
            // seeded with the CORRECT forms from the source and are renameable from settings.
            e.HasData(
                new PartOfSpeech { Id = PosNoun,       Code = "noun",       NameKu = "ناو",      SortOrder = 1 },
                new PartOfSpeech { Id = PosVerb,       Code = "verb",       NameKu = "کار",      SortOrder = 2 },
                new PartOfSpeech { Id = PosAdjective,  Code = "adjective",  NameKu = "هاوەڵناو", SortOrder = 3 },
                new PartOfSpeech { Id = PosAdverb,     Code = "adverb",     NameKu = "هاوەڵکار", SortOrder = 4 },
                new PartOfSpeech { Id = PosPronoun,    Code = "pronoun",    NameKu = "جێناو",    SortOrder = 5 },
                new PartOfSpeech { Id = PosParticle,   Code = "particle",   NameKu = "ئامڕاز",   SortOrder = 6 },
                new PartOfSpeech { Id = PosInfinitive, Code = "infinitive", NameKu = "چاوگ",     SortOrder = 7 });
        });
    }

    private static void ConfigureFeatures(ModelBuilder b)
    {
        b.Entity<FeatureAxis>(e =>
        {
            e.HasIndex(x => x.Code).IsUnique();

            // Single choice is the default because it is what every axis that already exists does.
            // A new axis starts life behaving like the noun's, and widening it is a deliberate act
            // in the settings tree rather than something that happens by omission.
            e.Property(x => x.MinSelections).HasDefaultValue(0);
            e.Property(x => x.MaxSelections).HasDefaultValue(1);
        });

        b.Entity<FeatureValue>(e =>
        {
            e.HasOne(x => x.Axis)
             .WithMany(a => a.Values)
             .HasForeignKey(x => x.AxisId)
             .OnDelete(DeleteBehavior.Restrict);

            // Two values on one axis may not share a name — the settings validator says so, and the
            // database agrees, so a race between two admins cannot slip a duplicate through.
            e.HasIndex(x => new { x.AxisId, x.NameKu }).IsUnique();

            e.HasOne<FeatureValue>()
             .WithMany()
             .HasForeignKey(x => x.MergedIntoValueId)
             .OnDelete(DeleteBehavior.NoAction);
        });

        b.Entity<PartOfSpeechAxis>(e =>
        {
            // An axis is assigned to a part of speech at most once.
            e.HasIndex(x => new { x.PartOfSpeechId, x.AxisId }).IsUnique();

            e.HasOne(x => x.PartOfSpeech)
             .WithMany(p => p.Axes)
             .HasForeignKey(x => x.PartOfSpeechId)
             .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(x => x.Axis)
             .WithMany()
             .HasForeignKey(x => x.AxisId)
             .OnDelete(DeleteBehavior.Restrict);

            // NoAction: the condition points at a FeatureValue that belongs to another axis on the
            // same part of speech, so a cascade here would delete assignments as a side effect of
            // tidying a value. The service layer refuses to delete a value that is in use as a
            // condition — it may only be deactivated, and the UI names what depends on it.
            e.HasOne(x => x.RequiresValue)
             .WithMany()
             .HasForeignKey(x => x.RequiresValueId)
             .OnDelete(DeleteBehavior.NoAction);
        });

        b.Entity<SenseFeature>(e =>
        {
            // UNIQUE(SenseId, AxisId, ValueId) — several values per axis where the axis allows it,
            // but never the same value twice on one sense. How many is MaxSelections, which is data.
            //
            // ValueId is nullable and SQL Server treats NULLs as equal in a unique index, which is
            // exactly what is wanted here: the not-applicable row (ValueId NULL) can exist at most
            // once per axis, so the escape hatch cannot be recorded twice.
            //
            // Filtered so soft-deleted answers do not block a re-answer.
            e.HasIndex(x => new { x.SenseId, x.AxisId, x.ValueId })
             .IsUnique()
             .HasFilter("[IsDeleted] = 0");

            e.HasOne(x => x.Sense)
             .WithMany(s => s.Features)
             .HasForeignKey(x => x.SenseId)
             .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(x => x.Axis)
             .WithMany()
             .HasForeignKey(x => x.AxisId)
             .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(x => x.Value)
             .WithMany()
             .HasForeignKey(x => x.ValueId)
             .OnDelete(DeleteBehavior.Restrict);

            // Either a value or an explicit not-applicable, never both and never neither. The
            // distinction is the whole reason the work queue can ever be emptied.
            e.ToTable(t => t.HasCheckConstraint(
                "CK_SenseFeature_ValueOrNotApplicable",
                "([ValueId] IS NOT NULL AND [IsNotApplicable] = 0) OR ([ValueId] IS NULL AND [IsNotApplicable] = 1)"));
        });
    }

    private static void ConfigureDomains(ModelBuilder b)
    {
        b.Entity<Domain>(e =>
        {
            e.HasIndex(x => new { x.ParentId, x.NameKu }).IsUnique();

            // NoAction rather than Cascade: deleting a parent must not silently take its children —
            // the settings area moves them first and says how many are affected.
            e.HasOne(x => x.Parent)
             .WithMany(x => x.Children)
             .HasForeignKey(x => x.ParentId)
             .OnDelete(DeleteBehavior.NoAction);
        });
    }

    private static void ConfigureSenses(ModelBuilder b)
    {
        b.Entity<Sense>(e =>
        {
            e.HasOne(x => x.Word)
             .WithMany(w => w.Senses)
             .HasForeignKey(x => x.WordId)
             .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(x => x.PartOfSpeech)
             .WithMany()
             .HasForeignKey(x => x.PartOfSpeechId)
             .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(x => x.Domain)
             .WithMany()
             .HasForeignKey(x => x.DomainId)
             .OnDelete(DeleteBehavior.Restrict);

            e.HasIndex(x => x.WordId);
            e.HasIndex(x => x.PartOfSpeechId);
            e.HasIndex(x => x.DomainId);
            e.HasIndex(x => x.WorkflowState);

            // Persisted so it can be filtered and sorted without recomputation. Only sense-local
            // facts: required-axis coverage needs a join and belongs to the work queue.
            e.Property(x => x.Completeness)
             .HasComputedColumnSql(
                 "(CASE WHEN LEN(LTRIM(RTRIM([Definition]))) > 0 THEN 34 ELSE 0 END + " +
                 " CASE WHEN LEN(LTRIM(RTRIM([ExampleUsage]))) > 0 THEN 33 ELSE 0 END + " +
                 " CASE WHEN [DomainId] IS NOT NULL THEN 33 ELSE 0 END)",
                 stored: true);
        });
    }

    private static void ConfigureFormsAndRelations(ModelBuilder b)
    {
        b.Entity<SenseTranslation>(e =>
        {
            e.HasOne(x => x.Sense)
             .WithMany()
             .HasForeignKey(x => x.SenseId)
             .OnDelete(DeleteBehavior.Cascade);

            e.HasIndex(x => new { x.SenseId, x.LanguageCode });
        });

        b.Entity<WordForm>(e =>
        {
            e.HasOne(x => x.Word)
             .WithMany(w => w.Forms)
             .HasForeignKey(x => x.WordId)
             .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(x => x.FormType)
             .WithMany()
             .HasForeignKey(x => x.FormTypeId)
             .OnDelete(DeleteBehavior.Restrict);

            // A query for جوانترین has to hit this index and come back with the parent headword.
            e.HasIndex(x => x.Normalized);
        });

        b.Entity<RelationTypeDef>(e =>
        {
            e.HasIndex(x => x.Code).IsUnique();

            e.HasOne(x => x.Inverse)
             .WithMany()
             .HasForeignKey(x => x.InverseId)
             .OnDelete(DeleteBehavior.NoAction);

            SeedRelationTypes(e);
        });

        b.Entity<WordRelation>(e =>
        {
            e.HasOne(x => x.FromWord)
             .WithMany(w => w.OutgoingWordRelations)
             .HasForeignKey(x => x.FromWordId)
             .OnDelete(DeleteBehavior.Cascade);

            // Restrict on the far end: SQL Server refuses two cascade paths into the same table,
            // and a word must not disappear because something pointed at it.
            e.HasOne(x => x.ToWord)
             .WithMany(w => w.IncomingWordRelations)
             .HasForeignKey(x => x.ToWordId)
             .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(x => x.Type)
             .WithMany()
             .HasForeignKey(x => x.TypeId)
             .OnDelete(DeleteBehavior.Restrict);

            e.HasIndex(x => new { x.FromWordId, x.ToWordId, x.TypeId })
             .IsUnique()
             .HasFilter("[IsDeleted] = 0");
        });

        b.Entity<SenseRelation>(e =>
        {
            e.HasOne(x => x.FromSense)
             .WithMany()
             .HasForeignKey(x => x.FromSenseId)
             .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(x => x.ToSense)
             .WithMany()
             .HasForeignKey(x => x.ToSenseId)
             .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(x => x.Type)
             .WithMany()
             .HasForeignKey(x => x.TypeId)
             .OnDelete(DeleteBehavior.Restrict);

            e.HasIndex(x => new { x.FromSenseId, x.ToSenseId, x.TypeId })
             .IsUnique()
             .HasFilter("[IsDeleted] = 0");
        });
    }

    /// <summary>
    /// The relation types. Directional types come in pairs that point at each other, so saving one
    /// edge can create its inverse automatically and no teacher ever enters an edge twice.
    /// </summary>
    /// <remarks>
    /// InverseId is deliberately left NULL here and wired up by an UPDATE in the migration. The
    /// pairs are mutually referencing (ڕەگ ↔ داڕێژراو لێی), and HasData topologically sorts its
    /// inserts by foreign key — a two-row cycle has no valid order, so EF fails to scaffold the
    /// migration at all. Setting the column after both rows exist is the standard way out.
    /// The wiring is asserted by <c>TaxonomySchemaTests</c> so a silent NULL cannot survive.
    /// </remarks>
    private static void SeedRelationTypes(
        Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<RelationTypeDef> e)
    {
        e.HasData(
            // ── Word scope: morphology ──────────────────────────────────────
            new RelationTypeDef { Id = 1, Code = "root",          NameKu = "ڕەگ",            Scope = RelationScope.Word, IsSymmetric = false, SortOrder = 1 },
            new RelationTypeDef { Id = 2, Code = "derived-from",  NameKu = "داڕێژراو لێی",   Scope = RelationScope.Word, IsSymmetric = false, SortOrder = 2 },

            new RelationTypeDef { Id = 3, Code = "component",     NameKu = "پێکهاتە",        Scope = RelationScope.Word, IsSymmetric = false, SortOrder = 3 },
            new RelationTypeDef { Id = 4, Code = "part-of",       NameKu = "بەشێکە لە",      Scope = RelationScope.Word, IsSymmetric = false, SortOrder = 4 },

            new RelationTypeDef { Id = 5, Code = "infinitive-of", NameKu = "چاوگی کارەکە",   Scope = RelationScope.Word, IsSymmetric = false, SortOrder = 5 },
            new RelationTypeDef { Id = 6, Code = "verb-of",       NameKu = "کاری چاوگەکە",   Scope = RelationScope.Word, IsSymmetric = false, SortOrder = 6 },

            new RelationTypeDef { Id = 7, Code = "regional",      NameKu = "زاراوەی هەرێمی", Scope = RelationScope.Word, IsSymmetric = true,  SortOrder = 7 },

            // ── Sense scope: semantics ──────────────────────────────────────
            new RelationTypeDef { Id = 8,  Code = "synonym",  NameKu = "هاومانا",      Scope = RelationScope.Sense, IsSymmetric = true,  SortOrder = 8 },
            new RelationTypeDef { Id = 9,  Code = "antonym",  NameKu = "پێچەوانە",     Scope = RelationScope.Sense, IsSymmetric = true,  SortOrder = 9 },

            new RelationTypeDef { Id = 10, Code = "broader",  NameKu = "مانای گشتیتر", Scope = RelationScope.Sense, IsSymmetric = false, SortOrder = 10 },
            new RelationTypeDef { Id = 11, Code = "narrower", NameKu = "مانای وردتر",  Scope = RelationScope.Sense, IsSymmetric = false, SortOrder = 11 });
    }

    /// <summary>
    /// The inverse wiring, applied by the migration once all eleven rows exist.
    /// Symmetric types (زاراوەی هەرێمی، هاومانا، پێچەوانە) stay NULL — they are their own inverse.
    /// </summary>
    public const string InverseWiringSql = @"
        UPDATE [RelationTypes] SET [InverseId] = 2  WHERE [Id] = 1;   -- ڕەگ            ↔ داڕێژراو لێی
        UPDATE [RelationTypes] SET [InverseId] = 1  WHERE [Id] = 2;
        UPDATE [RelationTypes] SET [InverseId] = 4  WHERE [Id] = 3;   -- پێکهاتە        ↔ بەشێکە لە
        UPDATE [RelationTypes] SET [InverseId] = 3  WHERE [Id] = 4;
        UPDATE [RelationTypes] SET [InverseId] = 6  WHERE [Id] = 5;   -- چاوگی کارەکە   ↔ کاری چاوگەکە
        UPDATE [RelationTypes] SET [InverseId] = 5  WHERE [Id] = 6;
        UPDATE [RelationTypes] SET [InverseId] = 11 WHERE [Id] = 10;  -- مانای گشتیتر   ↔ مانای وردتر
        UPDATE [RelationTypes] SET [InverseId] = 10 WHERE [Id] = 11;
    ";

    /// <summary>
    /// Every taxonomy entity is soft-deletable, so every one needs its filter. A dependent must
    /// repeat its principal's filter or EF warns and the join returns rows whose parent is hidden.
    /// </summary>
    private static void ConfigureSoftDeleteFilters(ModelBuilder b)
    {
        b.Entity<PartOfSpeech>().HasQueryFilter(x => !x.IsDeleted);
        b.Entity<FeatureAxis>().HasQueryFilter(x => !x.IsDeleted);
        b.Entity<FeatureValue>().HasQueryFilter(x => !x.IsDeleted && !x.Axis.IsDeleted);
        b.Entity<Domain>().HasQueryFilter(x => !x.IsDeleted);
        b.Entity<WordFormType>().HasQueryFilter(x => !x.IsDeleted);

        // PartOfSpeechAxis is not itself soft-deletable, but it is required-end to two entities that
        // are. Without a matching filter EF warns, and an assignment would keep resolving after its
        // axis was hidden — the entry form would render an axis the settings screen had retired.
        b.Entity<PartOfSpeechAxis>().HasQueryFilter(x => !x.PartOfSpeech.IsDeleted && !x.Axis.IsDeleted);

        b.Entity<Sense>().HasQueryFilter(x => !x.IsDeleted && !x.Word.IsDeleted);

        // The operations tables are not soft-deletable themselves, but they all hang off a Sense
        // that is. Without matching filters EF warns, and a claim or a disagreement would keep
        // resolving against a sense that has been hidden.
        b.Entity<SenseClaim>().HasQueryFilter(x => !x.Sense.IsDeleted && !x.Sense.Word.IsDeleted);
        b.Entity<SenseDisagreement>().HasQueryFilter(x => !x.Sense.IsDeleted && !x.Sense.Word.IsDeleted);
        b.Entity<ConsistencySample>().HasQueryFilter(x => !x.Sense.IsDeleted && !x.Sense.Word.IsDeleted);
        b.Entity<SenseFeature>().HasQueryFilter(x => !x.IsDeleted && !x.Sense.IsDeleted);
        b.Entity<SenseTranslation>().HasQueryFilter(x => !x.IsDeleted && !x.Sense.IsDeleted);
        b.Entity<WordForm>().HasQueryFilter(x => !x.IsDeleted && !x.Word.IsDeleted);
        b.Entity<WordRelation>().HasQueryFilter(x => !x.IsDeleted && !x.FromWord.IsDeleted);
        b.Entity<SenseRelation>().HasQueryFilter(x => !x.IsDeleted && !x.FromSense.IsDeleted);
    }
}
