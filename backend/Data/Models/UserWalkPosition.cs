namespace backend.Data.Models;

/// <summary>
/// Where a person had got to in a walk, so closing the app and coming back does not send them
/// to the first word of the dictionary.
///
/// This is the difference between a tool somebody uses for an hour a day and one they use once.
/// The station walks ~4,900 senses; a teacher who worked to position 812 on Tuesday and is put
/// back at 1 on Wednesday has to either remember the number or scroll for it, and in practice
/// they re-do work they have already done.
///
/// Keyed by (UserId, Walk) rather than a column on AppUser, because there is more than one walk —
/// the station and the relation workspace both step through the dictionary — and they must not
/// share a position. A new walk needs a new string here and nothing else.
///
/// Stored as an ANCHOR, not just a number. The position is an index into an ordering that shifts
/// whenever senses are added or removed, so a bare "812" silently drifts onto a different word.
/// The word and sense ids are what actually identify the place; the position is a hint used to
/// restore the counter without re-deriving it, and it is corrected on the way back in.
/// </summary>
public class UserWalkPosition
{
    public long Id { get; set; }

    public Guid UserId { get; set; }
    public AppUser? User { get; set; }

    /// <summary>Which walk — see <see cref="Walks"/>. Unique together with the user.</summary>
    public string Walk { get; set; } = string.Empty;

    /// <summary>The place itself. Survives reordering; the position does not.</summary>
    public int? WordId { get; set; }
    public int? SenseId { get; set; }

    /// <summary>Last known index, for restoring the counter without a re-derivation.</summary>
    public int Position { get; set; }

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

/// <summary>The walk names. Constants rather than an enum: they are a persisted key, and an enum
/// renumbering itself would silently repoint everyone's saved place.</summary>
public static class Walks
{
    public const string Station = "station";
    public const string Relations = "relations";
}
