namespace Shared.Dtos;

public class CreateWordDto
{
    public string Kurdish { get; set; } = string.Empty;
    public string? Meaning { get; set; }
    public string? Category { get; set; }
    public string? Description { get; set; }
    public List<CreateRelatedWordDto> RelatedWords { get; set; } = new();
}

public class CreateRelatedWordDto
{
    public int TargetWordId { get; set; }
    public string RelationType { get; set; } = string.Empty;
}
