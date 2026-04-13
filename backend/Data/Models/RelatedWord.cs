namespace backend.Data.Models;

public class RelatedWord
{
    public int Id { get; set; }

    public int WordId { get; set; }
    public Word Word { get; set; } = null!;

    public int TargetWordId { get; set; }
    public Word TargetWord { get; set; } = null!;

    /// <summary>synonym | antonym | related | example</summary>
    public string RelationType { get; set; } = string.Empty;
}
