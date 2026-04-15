using backend.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace backend.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Word> Words => Set<Word>();
    public DbSet<RelatedWord> RelatedWords => Set<RelatedWord>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Word>(entity =>
        {
            entity.HasKey(w => w.Id);
            entity.Property(w => w.Kurdish).IsRequired().HasMaxLength(200);
            entity.Property(w => w.Meaning).HasMaxLength(500);
            entity.Property(w => w.Category).HasMaxLength(100);
            entity.Property(w => w.Description).HasMaxLength(1000);
        });

        modelBuilder.Entity<RelatedWord>()
    .HasOne(r => r.Word)
    .WithMany(w => w.OutgoingRelations)
    .HasForeignKey(r => r.WordId)
    .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<RelatedWord>()
            .HasOne(r => r.TargetWord)
            .WithMany(w => w.IncomingRelations)
            .HasForeignKey(r => r.TargetWordId)
            .OnDelete(DeleteBehavior.Restrict);


        //    var words = new List<Word>();
        //    var relations = new List<RelatedWord>();

        //    string[] rawWords = {
        //    "برنج", "شۆربا", "کباب", "دۆڵمە", "نیسک", "ماست", "شێلم", "پەنیر", "هێلکە", "مریشک", // 1-10
        //    "گۆشت", "سەوزە", "تەماتە", "خەیار", "پیاز", "سیر", "بیبەر", "باینجان", "کولەکە", "پەتاتە", // 11-20
        //    "سێو", "مۆز", "پرتەقاڵ", "هەنار", "ترێ", "هەنجیر", "قەیسی", "شووتی", "کاڵەک", "گێلاس", // 21-30
        //    "نانی تیرێ", "سەموون", "گەنم", "جۆ", "نۆک", "نیسکێنە", "ترخێنە", "دۆینە", "شەلەم", "قەرەخەرمان", // 31-40
        //    "کەوچک", "چەتاڵ", "چەقۆ", "قاپ", "مەنجەڵ", "قابڵەمە", "پەرداغ", "دۆلکە", "سفرە", "تەباغ", // 41-50
        //    "فڕن", "سەلاجە", "خەڵاتە", "هاوەن", "سەبەتە", "سینای", "فنجان", "قۆری", "سەماوەر", "چای", // 51-60
        //    "شەکر", "خوێ", "بیبەری ڕەش", "زەردەچەوە", "دارچین", "مێخەک", "زەیت", "کەرە", "دۆشاوی تەماتە", "سماق", // 61-70
        //    "نانی هەورامی", "کولێرە", "ناوکەوڕەقە", "کەلانە", "شلکێنە", "یاپراخ", "کووپە", "جەوژەن", "گۆشتەبڕاو", "قوراو", // 71-80
        //    "لیمۆ", "لالەنگی", "هەرمێ", "خۆخ", "قەیسی وشک", "مێوژ", "گوێز", "بایەم", "فستق", "بندق", // 81-90
        //    "سیرکە", "ڕووبەهەنار", "دۆ", "شەربەت", "ئاوی تەماتە", "قاوە", "شیر", "هەنگوین", "مورەبا", "دۆشاو" // 91-100
        //};

        //    for (int i = 0; i < rawWords.Length; i++)
        //    {
        //        words.Add(new Word
        //        {
        //            Id = i + 1,
        //            Kurdish = rawWords[i],
        //            Meaning = $"زانیاری دەربارەی {rawWords[i]}",
        //            Category = i < 40 ? "خواردن و دانەوێڵە" : (i < 70 ? "ئامراز و بەهارات" : "خواردنی کلتووری و میوە"),
        //            Description = "ئەم داتایە بە شێوەی ئۆتۆماتیکی دروستکراوە بۆ تاقیکردنەوەی مایند ماپەکە.",
        //            CreatedAt = DateTime.UtcNow
        //        });
        //    }

        //    // دروستکردنی پەیوەندییەکان (١٠٠ پەیوەندی بۆ ئەوەی گرافەکە زۆر تێکەڵ بێت)
        //    int relId = 1;
        //    // بەستنەوەی خواردنەکان بە ئامرازەکانەوە
        //    relations.Add(new RelatedWord { Id = relId++, WordId = 45, TargetWordId = 1, RelationType = "بەکاردێت بۆ" }); // مەنجەڵ -> برنج
        //    relations.Add(new RelatedWord { Id = relId++, WordId = 45, TargetWordId = 4, RelationType = "بەکاردێت بۆ" }); // مەنجەڵ -> دۆڵمە
        //    relations.Add(new RelatedWord { Id = relId++, WordId = 60, TargetWordId = 58, RelationType = "پەیوەندیدار" }); // چای -> قۆری
        //    relations.Add(new RelatedWord { Id = relId++, WordId = 60, TargetWordId = 59, RelationType = "پەیوەندیدار" }); // چای -> سەماوەر
        //    relations.Add(new RelatedWord { Id = relId++, WordId = 1, TargetWordId = 35, RelationType = "پێکەوە دەخورێن" }); // برنج -> نۆک
        //    relations.Add(new RelatedWord { Id = relId++, WordId = 6, TargetWordId = 93, RelationType = "لێ دروست دەکرێت" }); // ماست -> دۆ
        //    relations.Add(new RelatedWord { Id = relId++, WordId = 11, TargetWordId = 3, RelationType = "پێکهاتەیە لە" }); // گۆشت -> کباب
        //    relations.Add(new RelatedWord { Id = relId++, WordId = 15, TargetWordId = 16, RelationType = "هاوپۆل" }); // پیاز -> سیر
        //    relations.Add(new RelatedWord { Id = relId++, WordId = 21, TargetWordId = 22, RelationType = "میوە" }); // سێو -> مۆز

        //    // زیادکردنی پەیوەندی بازنەیی (بۆ ئەوەی گرافەکە پچڕانی تێنەکەوێت)
        //    for (int i = 1; i < 90; i++)
        //    {
        //        relations.Add(new RelatedWord
        //        {
        //            Id = relId++,
        //            WordId = i,
        //            TargetWordId = i + 1,
        //            RelationType = "پەیوەندیدار"
        //        });
        //    }


        var baseKurdishWords = new List<string> {
        "برنج", "کباب", "دۆڵمە", "نیسکێنە", "ترخێنە", "شۆربا", "مریشک", "گۆشت", "ماست", "پەنیر",
        "کەرە", "قەیماغ", "هەنگوین", "مورەبا", "هێلکە", "نانی تیرێ", "سەموون", "کولێرە", "کەلانە", "شلکێنە",
        "سێو", "مۆز", "پرتەقاڵ", "هەنار", "ترێ", "هەنجیر", "قەیسی", "شووتی", "کاڵەک", "گێلاس",
        "تەماتە", "خەیار", "پیاز", "سیر", "بیبەر", "باینجان", "کولەکە", "پەتاتە", "سەوزە", "کەرەوز",
        "مەنجەڵ", "قاپ", "کەوچک", "چەتاڵ", "چەقۆ", "پەرداغ", "دۆلکە", "سفرە", "تەباغ", "فڕن",
        "سەلاجە", "خەڵاتە", "هاوەن", "سەبەتە", "سینای", "فنجان", "قۆری", "سەماوەر", "چای", "قاوە",
        "شەکر", "خوێ", "سماق", "زەردەچەوە", "دارچین", "مێخەک", "زەیت", "دۆشاوی تەماتە", "ئاوی لیمۆ", "سیرکە",
        "گوێز", "بایەم", "فستق", "بندق", "مێوژ", "ناووکەوڕەقە", "خورما", "گەنمی کوتراو", "نۆک", "نیسک",
        "یاپراخ", "کووپە", "جەوژەن", "گۆشتەبڕاو", "قوراو", "سیرەمۆز", "دۆ", "شەربەت", "شیر", "دۆشاو",
        "تەحین", "پاقڵاوە", "لوقمەقازی", "بامیە", "کفتە", "شفتە", "شیشکەباب", "پڵاو", "ساوەر", "شەلەم",
        "پیزا", "بەرگر", "فینگەر", "سایندویچ", "فەلافل", "گەس", "مایۆنێز", "کەتچەپ", "خەردەل", "زەڵاتە",
        "سپێناخ", "مەعدەنوس", "کەوەر", "تەرۆزی", "سڵق", "ڕێواس", "قارچک", "کوندەر", "لازانیا", "ماکەرۆنی",
        "نۆدڵز", "ئاش", "شێلم", "ماش", "لۆبیا", "نۆکی ڕەش", "بڕوێش", "گوێزاو", "ئاوی میوە", "دۆشاوی هەنار",
        "کفتەی برنج", "کفتەی ساوەر", "شفتەی مریشک", "قەیسی تەڕ", "هەنجیری تەڕ", "زالۆکە", "بابا غەنوج", "حومس",
        "موتەبەل", "فتووش", "تەبولە", "زەڵاتەی وەرزی", "زەڵاتەی پەتاتە", "ماست و خەیار", "ماست و موسیر",
        "دۆخیوا", "گیپە", "سەروپێ", "قەل", "مریشکی برژاو", "ماسی", "ماسی مەسگوف", "گۆشتی بەرخ", "گۆشتی مانگا",
        "قەلیە", "شۆربای نیسک", "شۆربای جۆ", "شۆربای سەوزە", "برنجی کوردی", "برنجی بسمەتی", "برنجی ڕەش",
        "کێکی سادە", "کێکی شوکولاتە", "پسکیتی چای", "قەیسی وشککراو", "تەمر هیندی", "قەمەرەدین", "جەلی", "مەهەلەبی"
        // دەتوانی لێرەدا بەردەوام بیت تا ١٠٠٠ وشە...
    };

        var words = new List<Word>();
        var relations = new List<RelatedWord>();
        int relId = 1; // simple counter for relation IDs

        for (int i = 10000; i <= 11000; i++)
        {
            int baseIndex = (i - 10000) % baseKurdishWords.Count;
            string prefix = i > 10500 ? "جۆری نایابی " : "";
            string suffix = i > 10500 ? $" ({i})" : "";

            words.Add(new Word
            {
                Id = i,
                Kurdish = $"{prefix}{baseKurdishWords[baseIndex]}{suffix}",
                Meaning = $"زانیاری ورد دەربارەی {baseKurdishWords[baseIndex]}",
                Category = baseIndex < 20 ? "خواردنی سەرەکی" : (baseIndex < 40 ? "میوە و سەوزە" : "کەلوپەل و بەهارات"),
                Description = $"ئەم وشەیە پەیوەستە بە لقی {baseKurdishWords[baseIndex]}",
                CreatedAt = DateTime.UtcNow
            });
        }

        // relations in a separate loop AFTER all words are created
        for (int i = 10000; i <= 11000; i++)
        {
            string[] relationTypes = { "synonym", "antonym", "related", "contextual", "example", "usage" };

            for (int j = 0; j < 6; j++)
            {
                // target must be a valid word ID in range 10000-11000, and not same as source
                int targetId = 10000 + ((i - 10000 + (j + 1) * 17) % 1001);
                if (targetId == i) targetId = 10000 + ((targetId - 10000 + 1) % 1001);

                relations.Add(new RelatedWord
                {
                    Id = relId++,   // simple auto-increment, no duplicates
                    WordId = i,
                    TargetWordId = targetId,
                    RelationType = relationTypes[j]
                });
            }
        }

        // HasData called ONCE only
        modelBuilder.Entity<Word>().HasData(words);
        modelBuilder.Entity<RelatedWord>().HasData(relations);


    }
}
