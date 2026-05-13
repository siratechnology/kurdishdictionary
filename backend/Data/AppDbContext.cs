using backend.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Word> Words => Set<Word>();
    public DbSet<RelatedWord> RelatedWords => Set<RelatedWord>();
    public DbSet<WordMeans> WordMeans => Set<WordMeans>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<WordSpeechPane> WordSpeechPanes => Set<WordSpeechPane>();
    public DbSet<WordCategory> WordCategories => Set<WordCategory>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Word>(entity =>
        {
            entity.HasKey(w => w.Id);
            entity.Property(w => w.Kurdish).IsRequired().HasMaxLength(200);
            entity.Property(w => w.Description).HasMaxLength(1000);
            entity.Property(w => w.Gender).HasDefaultValue(GrammaticalGender.None);
        });

        // Many-to-many: Word <-> SpeechPaneType (enum-backed join table)
        modelBuilder.Entity<WordSpeechPane>(entity =>
        {
            entity.HasKey(wsp => new { wsp.WordId, wsp.SpeechPaneType });
            entity.HasOne(wsp => wsp.Word)
                  .WithMany(w => w.SpeechPanes)
                  .HasForeignKey(wsp => wsp.WordId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Name).IsRequired().HasMaxLength(100);
            entity.HasIndex(c => c.Name).IsUnique();
        });

        // Many-to-many: Word <-> Category (explicit join entity)
        modelBuilder.Entity<WordCategory>(entity =>
        {
            entity.HasKey(wc => new { wc.WordId, wc.CategoryId });
            entity.HasOne(wc => wc.Word)
                  .WithMany(w => w.WordCategories)
                  .HasForeignKey(wc => wc.WordId)
                  .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(wc => wc.Category)
                  .WithMany(c => c.WordCategories)
                  .HasForeignKey(wc => wc.CategoryId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<WordMeans>(entity =>
        {
            entity.HasKey(wm => wm.Id);
            entity.Property(wm => wm.Meaning).IsRequired().HasMaxLength(500);
            entity.Property(wm => wm.Locate).HasMaxLength(100);
            entity.HasOne(wm => wm.Word)
                  .WithMany(w => w.Meanings)
                  .HasForeignKey(wm => wm.WordId)
                  .OnDelete(DeleteBehavior.Cascade);
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

        // ── Seed data ──────────────────────────────────────────────────────────
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
            "کێکی سادە", "کێکی شوکولاتە", "پسکیتی چای", "قەیسی وشككراو", "تەمر هیندی", "قەمەرەدین", "جەلی", "مەهەلەبی"
        };

        // Seed canonical categories
        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "خواردنی سەرەکی" },
            new Category { Id = 2, Name = "میوە و سەوزە" },
            new Category { Id = 3, Name = "کەلوپەل و بەهارات" }
        );

        var words = new List<Word>();
        var relations = new List<RelatedWord>();
        int relId = 1;

        for (int i = 10000; i <= 11000; i++)
        {
            int baseIndex = (i - 10000) % baseKurdishWords.Count;
            string prefix = i > 10500 ? "جۆری نایابی " : "";
            string suffix = i > 10500 ? $" ({i})" : "";

            words.Add(new Word
            {
                Id = i,
                Kurdish = $"{prefix}{baseKurdishWords[baseIndex]}{suffix}",
                Gender = GrammaticalGender.None,
                Description = $"ئەم وشەیە پەیوەستە بە لقی {baseKurdishWords[baseIndex]}",
                CreatedAt = new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc)
            });
        }

        for (int i = 10000; i <= 11000; i++)
        {
            string[] relationTypes = { "synonym", "antonym", "related", "contextual", "example", "usage" };
            for (int j = 0; j < 6; j++)
            {
                int targetId = 10000 + ((i - 10000 + (j + 1) * 17) % 1001);
                if (targetId == i) targetId = 10000 + ((targetId - 10000 + 1) % 1001);

                relations.Add(new RelatedWord
                {
                    Id = relId++,
                    WordId = i,
                    TargetWordId = targetId,
                    RelationType = relationTypes[j]
                });
            }
        }

        modelBuilder.Entity<Word>().HasData(words);
        modelBuilder.Entity<RelatedWord>().HasData(relations);
        // WordSpeechPane and WordCategory seed data intentionally omitted:
        // the seeded word IDs (10000-11000) may not exist in a live database.
    }
}
