namespace Shared.Dtos;

public class WordDto
{
    public int Id { get; set; }
    public string Kurdish { get; set; } = string.Empty;
    public string? Meaning { get; set; }
    public string? Category { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<RelatedWordDto> RelatedWords { get; set; } = new();
}

public class RelatedWordDto
{
    public int Id { get; set; }
    public int TargetWordId { get; set; }
    public string? TargetKurdish { get; set; }
    public string RelationType { get; set; } = string.Empty;
}
