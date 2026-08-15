namespace Shared.Dtos;

/// <summary>
/// One teacher's contribution, for the credits page and the printed acknowledgements.
///
/// There is no rank, no score and no rate here on purpose (پڕۆمپت ٧). These are counts of work
/// done, broken down the way the printed book breaks it down — by بواری زانستی.
/// </summary>
public class ContributorCreditDto
{
    public Guid UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string? FullName { get; set; }

    public int WordsCreated { get; set; }
    public int SensesClassified { get; set; }
    public int RelationsAdded { get; set; }
    public int FormsAdded { get; set; }

    public DateTime? FirstContributionAt { get; set; }
    public DateTime? LastContributionAt { get; set; }

    public List<ContributorDomainCreditDto> Domains { get; set; } = new();
}

public class ContributorDomainCreditDto
{
    public int? DomainId { get; set; }
    public string Domain { get; set; } = string.Empty;

    public int WordsCreated { get; set; }
    public int SensesClassified { get; set; }
    public int FeaturesSet { get; set; }
    public int RelationsAdded { get; set; }
    public int FormsAdded { get; set; }
}

/// <summary>One line of a word's history. The proof that a teacher's work still exists.</summary>
public class WordHistoryEntryDto
{
    public long Id { get; set; }
    public DateTime OccurredAt { get; set; }

    public string UserName { get; set; } = string.Empty;
    public string? FullName { get; set; }

    public string EventType { get; set; } = string.Empty;
    public string EntityType { get; set; } = string.Empty;

    public string? FieldName { get; set; }
    public string? OldValue { get; set; }
    public string? NewValue { get; set; }
    public string? Note { get; set; }

    /// <summary>Human · Migration · Import — so a migrated row never looks like somebody typed it.</summary>
    public string SourceKind { get; set; } = string.Empty;
}
