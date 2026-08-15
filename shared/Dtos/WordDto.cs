namespace Shared.Dtos;

public class WordDto
{
    public int Id { get; set; }
    public string Kurdish { get; set; } = string.Empty;
    public List<SpeechPaneDto> SpeechPanes { get; set; } = new();
    public List<CategoryDto> Categories { get; set; } = new();
    public int Gender { get; set; }
    public string? GenderKurdish { get; set; }
    public string? Description { get; set; }

    /// <summary>بەشی فەرهەنگ. Null on the three thousand words imported before it existed.</summary>
    public int? DictionarySectionId { get; set; }
    public string? DictionarySectionName { get; set; }

    public DateTime CreatedAt { get; set; }
    public int TotalRelations { get; set; }
    public List<WordMeansDto> Meanings { get; set; } = new();
    public List<RelatedWordDto> OutgoingRelations { get; set; } = new();
    public List<RelatedWordDto> IncomingRelations { get; set; } = new();

    /// <summary>
    /// Set only when this word was found through one of its inflected forms rather than its
    /// headword — e.g. «جوانترین — پلەی باڵای جوان». The result is the headword's entry; this
    /// says why it came back, so a search for جوانترین does not look like a mistake.
    /// Null on every other result.
    /// </summary>
    public string? MatchedForm { get; set; }
}

public class SpeechPaneDto
{
    public int Id { get; set; }
    public string Kurdish { get; set; } = string.Empty;
}

/// <summary>بەشی فەرهەنگ — one per word.</summary>
public class DictionarySectionDto
{
    public int Id { get; set; }
    public string NameKu { get; set; } = string.Empty;
    public int WordCount { get; set; }
}

public class CategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int WordCount { get; set; }
}

public class SpeechPaneStatDto
{
    public int Id { get; set; }
    public string Kurdish { get; set; } = string.Empty;
    public int WordCount { get; set; }
}

public class WordMeansDto
{
    public int Id { get; set; }
    public string Meaning { get; set; } = string.Empty;
    public string? Locate { get; set; }
}

public class RelatedWordDto
{
    public int Id { get; set; }
    public int RelatedWordId { get; set; }
    public string? RelatedKurdish { get; set; }
    public string RelationType { get; set; } = string.Empty;
    public bool IsIncoming { get; set; }
    public int Weight { get; set; } = 1;
}

public class CreateWordDto
{
    public string Kurdish { get; set; } = string.Empty;

    /// <summary>بەشی فەرهەنگ — required by the editor, nullable here for the legacy rows.</summary>
    public int? DictionarySectionId { get; set; }
    public List<int> SpeechPanes { get; set; } = new();
    public List<int> CategoryIds { get; set; } = new();
    public int Gender { get; set; }
    public string? Description { get; set; }
    public List<WordMeansDto> Meanings { get; set; } = new();
    public List<CreateRelatedWordDto> RelatedWords { get; set; } = new();
}

public class CreateRelatedWordDto
{
    public int TargetWordId { get; set; }
    public string RelationType { get; set; } = string.Empty;
    public int Weight { get; set; } = 1;
}

public class WordMetaDto
{
    public int Id { get; set; }
    public string Kurdish { get; set; } = string.Empty;
    public List<SpeechPaneDto> SpeechPanes { get; set; } = new();
    public List<CategoryDto> Categories { get; set; } = new();
    public string? GenderKurdish { get; set; }
    public string? FirstMeaning { get; set; }
    public string? Description { get; set; }
}
