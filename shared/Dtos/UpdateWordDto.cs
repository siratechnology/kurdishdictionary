namespace Shared.Dtos;

public class UpdateWordDto
{
    public string Kurdish { get; set; } = string.Empty;
    public string? Meaning { get; set; }
    public string? Category { get; set; }
    public string? Description { get; set; }
    public List<CreateRelatedWordDto> RelatedWords { get; set; } = new();
}
