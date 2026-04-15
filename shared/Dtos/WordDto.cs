namespace Shared.Dtos;

public class WordDto
{
    public int Id { get; set; }
    public string Kurdish { get; set; } = string.Empty;
    public string? Meaning { get; set; }
    public string? Category { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }

    // لێرەدا دەتوانیت هەردووکیان جیابکەیتەوە بۆ ئەوەی لە گرافەکەدا سوودی لێ ببینی
    public List<RelatedWordDto> OutgoingRelations { get; set; } = new();
    public List<RelatedWordDto> IncomingRelations { get; set; } = new();
}

public class RelatedWordDto
{
    public int Id { get; set; }
    public int RelatedWordId { get; set; } // ئایدی وشە پەیوەندیدارەکە
    public string? RelatedKurdish { get; set; }
    public string RelationType { get; set; } = string.Empty;
    public bool IsIncoming { get; set; } // بۆ ئەوەی بزانیت ئەم وشەیە ئاماژەی بۆ کراوە یان خۆی ئاماژەی کردووە
    public int Weight { get; set; } = 1; // نزیکایەتی وشەکە لە وشەی سەرەکییەوە
}

public class CreateWordDto
{
    public string Kurdish { get; set; } = string.Empty;
    public string? Meaning { get; set; }
    public string? Category { get; set; }
    public string? Description { get; set; }
    // کاتی دروستکردن تەنها پەیوەندییە دەرچووەکان دادەنێین
    public List<CreateRelatedWordDto> RelatedWords { get; set; } = new();
}

public class CreateRelatedWordDto
{
    public int TargetWordId { get; set; }
    public string RelationType { get; set; } = string.Empty;
    public int Weight { get; set; } = 1;
}