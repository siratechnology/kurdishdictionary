namespace backend.Data.Models;

public class Word
{
    public int Id { get; set; }
    public string Kurdish { get; set; } = string.Empty;
    public string? Meaning { get; set; }
    public string? Category { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<RelatedWord> RelatedWords { get; set; } = new List<RelatedWord>();
}
