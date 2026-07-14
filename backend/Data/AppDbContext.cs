using backend.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace backend.Data;

public class AppDbContext : IdentityDbContext<AppUser, AppRole, Guid>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Word> Words => Set<Word>();
    public DbSet<RelatedWord> RelatedWords => Set<RelatedWord>();
    public DbSet<WordMeans> WordMeans => Set<WordMeans>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<WordSpeechPane> WordSpeechPanes => Set<WordSpeechPane>();
    public DbSet<WordCategory> WordCategories => Set<WordCategory>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
    public DbSet<AnalyticsEvent> AnalyticsEvents => Set<AnalyticsEvent>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Word>(entity =>
        {
            entity.HasKey(w => w.Id);
            entity.Property(w => w.Kurdish).IsRequired().HasMaxLength(200);
            entity.Property(w => w.Description).HasMaxLength(1000);
            entity.Property(w => w.Gender).HasDefaultValue(GrammaticalGender.None);

            // SetNull, not Cascade: deleting a user must never delete the words they wrote.
            entity.HasOne(w => w.CreatedByUser)
                  .WithMany()
                  .HasForeignKey(w => w.CreatedByUserId)
                  .OnDelete(DeleteBehavior.SetNull);

            // NoAction, even though SetNull is what we semantically want: SQL Server refuses two
            // SET NULL paths from the same table to the same principal ("may cause cycles or
            // multiple cascade paths", error 1785). AuthController.DeleteUser clears this column
            // by hand before deleting a user, which gets us the same result.
            entity.HasOne(w => w.UpdatedByUser)
                  .WithMany()
                  .HasForeignKey(w => w.UpdatedByUserId)
                  .OnDelete(DeleteBehavior.NoAction);

            entity.HasIndex(w => w.CreatedByUserId);
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

        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.HasKey(a => a.Id);
            entity.Property(a => a.Action).IsRequired().HasMaxLength(50);
            entity.Property(a => a.EntityType).IsRequired().HasMaxLength(50);
            entity.Property(a => a.Summary).HasMaxLength(500);
            entity.Property(a => a.IpAddress).HasMaxLength(64);
            entity.Property(a => a.UserAgent).HasMaxLength(512);
            entity.Property(a => a.UserName).HasMaxLength(256);
            entity.Property(a => a.Country).HasMaxLength(100);
            entity.Property(a => a.City).HasMaxLength(100);
            entity.HasIndex(a => a.CreatedAt);
            entity.HasIndex(a => a.UserId);
            entity.HasIndex(a => new { a.EntityType, a.EntityId });

            // Keep the audit row even if the user is deleted — that's the whole point of an audit trail.
            entity.HasOne(a => a.User)
                  .WithMany()
                  .HasForeignKey(a => a.UserId)
                  .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<AnalyticsEvent>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.EventType).IsRequired().HasMaxLength(32);
            entity.Property(e => e.Path).HasMaxLength(512);
            entity.Property(e => e.SearchTerm).HasMaxLength(200);
            entity.Property(e => e.SessionId).HasMaxLength(64);
            entity.Property(e => e.Referrer).HasMaxLength(512);
            entity.Property(e => e.IpAddress).HasMaxLength(64);
            entity.Property(e => e.Country).HasMaxLength(100);
            entity.Property(e => e.CountryCode).HasMaxLength(8);
            entity.Property(e => e.City).HasMaxLength(100);
            entity.Property(e => e.Region).HasMaxLength(100);
            entity.Property(e => e.Isp).HasMaxLength(200);
            entity.Property(e => e.Timezone).HasMaxLength(64);
            entity.Property(e => e.UserAgent).HasMaxLength(512);
            entity.Property(e => e.Browser).HasMaxLength(64);
            entity.Property(e => e.Os).HasMaxLength(64);
            entity.Property(e => e.DeviceType).HasMaxLength(16);
            entity.Property(e => e.Language).HasMaxLength(32);

            // The dashboard queries are all "recent events", "recent searches", "by country".
            entity.HasIndex(e => e.CreatedAt);
            entity.HasIndex(e => new { e.EventType, e.CreatedAt });
            entity.HasIndex(e => e.SearchTerm);
            entity.HasIndex(e => e.SessionId);
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

            entity.HasOne(wm => wm.CreatedByUser)
                  .WithMany()
                  .HasForeignKey(wm => wm.CreatedByUserId)
                  .OnDelete(DeleteBehavior.SetNull);
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

        // Deleting a user must not delete the relations they authored — just orphan them.
        modelBuilder.Entity<RelatedWord>()
            .HasOne(r => r.CreatedByUser)
            .WithMany()
            .HasForeignKey(r => r.CreatedByUserId)
            .OnDelete(DeleteBehavior.SetNull);

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
