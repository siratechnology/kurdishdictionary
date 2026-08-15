using System.ComponentModel.DataAnnotations;

namespace backend.Data.Models;

// ═══════════════════════════════════════════════════════════════════════════
// The operations room (پڕۆمپت ٧).
//
// Read this before changing anything here. The people using it are EXPERT LANGUAGE TEACHERS, not
// crowd workers. Their names go in the published dictionary's contributor credits, attached to the
// sections they built — that is the motivation, and the ledger is its evidence.
//
// Nothing in this file may become a score, a ranking, or a productivity metric. There is no XP, no
// leaderboard, and no accuracy rate. A claim lock is concurrency control, a disagreement is
// linguistic data, and a consistency sample is about terminology across the book — none of them are
// judgements of a professor.
// ═══════════════════════════════════════════════════════════════════════════

/// <summary>
/// What a user may do without waiting for anyone.
///
/// This is a WORKFLOW level, not a rank. A Senior is not better at Kurdish than a Contributor; they
/// have agreed to take responsibility for publishing. Never render it as a badge or a tier.
/// </summary>
public enum TrustLevel
{
    /// <summary>Work goes to a Senior's queue for publishing.</summary>
    Contributor = 0,

    /// <summary>Publishes directly — پۆلێنکراو and بڵاوکراو in one action, no waiting.</summary>
    Senior = 1,
}

/// <summary>
/// A 30-minute hold on a sense while somebody works on it.
///
/// This is CONCURRENCY CONTROL. Two experts opening the same word at once loses one of their
/// contributions, and that is the only problem it solves. It is not a check-out log, nobody is
/// measured by how long they hold a claim, and an expired claim is not a failure.
/// </summary>
public class SenseClaim
{
    public int Id { get; set; }

    public int SenseId { get; set; }
    public Sense Sense { get; set; } = null!;

    public Guid UserId { get; set; }
    public AppUser User { get; set; } = null!;

    public DateTime ClaimedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// ClaimedAt + 30 minutes. An expired claim is reclaimable by anyone — a teacher who closed
    /// their laptop must never be able to lock a word out of the dictionary indefinitely.
    /// </summary>
    public DateTime ExpiresAt { get; set; }

    /// <summary>Set when the holder releases it deliberately (Esc on the station screen).</summary>
    public DateTime? ReleasedAt { get; set; }

    public bool IsActive(DateTime now) => ReleasedAt is null && ExpiresAt > now;
}

/// <summary>
/// Two teachers classified the same sense differently.
///
/// This is NOT an error report and neither judgement is marked wrong. زۆر is هاوەڵناو AND
/// هاوەڵکار in the source deck itself — legitimate disagreement is linguistic data, and a system
/// that silently overwrites the first answer loses information the dictionary needs.
///
/// Both judgements, both names and both notes are kept and shown together.
/// </summary>
public class SenseDisagreement
{
    public int Id { get; set; }

    public int SenseId { get; set; }
    public Sense Sense { get; set; } = null!;

    /// <summary>Null when the disagreement is about the part of speech rather than an axis.</summary>
    public int? AxisId { get; set; }
    public FeatureAxis? Axis { get; set; }

    /// <summary>What the first teacher recorded, as text — the value may later be renamed or retired.</summary>
    [MaxLength(200)]
    public string FirstJudgement { get; set; } = string.Empty;

    public Guid FirstUserId { get; set; }
    public AppUser FirstUser { get; set; } = null!;

    [MaxLength(1000)]
    public string? FirstNote { get; set; }

    [MaxLength(200)]
    public string SecondJudgement { get; set; } = string.Empty;

    public Guid SecondUserId { get; set; }
    public AppUser SecondUser { get; set; } = null!;

    [MaxLength(1000)]
    public string? SecondNote { get; set; }

    public DateTime OccurredAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Set when a linguist decides the sense genuinely holds one reading. Unresolved is the normal
    /// state, not a backlog — some words legitimately carry both.
    /// </summary>
    public DateTime? ResolvedAt { get; set; }
    public Guid? ResolvedByUserId { get; set; }

    [MaxLength(1000)]
    public string? Resolution { get; set; }
}

/// <summary>
/// A sense drawn by the background sampler for a terminology read.
///
/// Surfaced to people as «یەکڕێزی زاراوە» — is the same concept called the same thing across
/// contributors? — and never as a review of the person who wrote it. Roughly 5% of published
/// senses, sampled at random, so being drawn carries no implication at all.
/// </summary>
public class ConsistencySample
{
    public int Id { get; set; }

    public int SenseId { get; set; }
    public Sense Sense { get; set; } = null!;

    public DateTime SampledAt { get; set; } = DateTime.UtcNow;

    public DateTime? ReadAt { get; set; }
    public Guid? ReadByUserId { get; set; }

    /// <summary>Free text about the TERMINOLOGY, e.g. "this uses سیفەت where the rest of the book uses هاوەڵناو".</summary>
    [MaxLength(1000)]
    public string? Note { get; set; }
}
