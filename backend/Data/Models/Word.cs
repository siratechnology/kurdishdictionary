namespace backend.Data.Models;

public class Word
{
    public int Id { get; set; }
    public string Kurdish { get; set; } = string.Empty;
    public string? Meaning { get; set; }
    public string? Category { get; set; }
    public string? ColorCode { get; set; } // بۆ مایند ماپەکە: "blue", "#ff0000"
    public int Importance { get; set; } = 1; // قەبارەی بازنەکە لە ناو گرافەکە
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<RelatedWord> OutgoingRelations { get; set; } = new List<RelatedWord>();
    public ICollection<RelatedWord> IncomingRelations { get; set; } = new List<RelatedWord>();
}

