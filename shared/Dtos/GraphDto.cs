namespace Shared.Dtos;

/// <summary>
/// گرافی D3.js بۆ نیشاندانی پەیوەندییەکانی وشەکان
/// </summary>
public class GraphDto
{
    public List<GraphNodeDto> Nodes { get; set; } = new();
    public List<GraphLinkDto> Links { get; set; } = new();
}

public class GraphNodeDto
{
    public string Id { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public string? Meaning { get; set; }
    public string? Category { get; set; }
    public bool IsCenter { get; set; }
    /// <summary>ژمارەی پەیوەندییەکان — بۆ قەبارەی نۆدەکە</summary>
    public int Weight { get; set; } = 1;
    public string? Color { get; set; }
    /// <summary>جۆری پەیوەندی بە نۆدی ناوەندەوە</summary>
    public string? RelationType { get; set; }
}

public class GraphLinkDto
{
    public string Source { get; set; } = string.Empty;
    public string Target { get; set; } = string.Empty;
    public string RelationType { get; set; } = string.Empty;
    public int Weight { get; set; } = 1;
    public bool IsIncoming { get; set; }
}
