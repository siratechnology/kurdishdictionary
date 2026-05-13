namespace Shared.Dtos;

public class UpdateWordDto
{
    public string Kurdish { get; set; } = string.Empty;
    public List<int> SpeechPanes { get; set; } = new();
    public List<int> CategoryIds { get; set; } = new();
    public int Gender { get; set; }
    public string? Description { get; set; }
    public List<WordMeansDto> Meanings { get; set; } = new();
    public List<CreateRelatedWordDto> RelatedWords { get; set; } = new();
}
