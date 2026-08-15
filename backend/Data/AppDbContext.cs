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

    /// <summary>The contribution ledger. Append-only — see <see cref="ContributionEvent"/>.</summary>
    public DbSet<ContributionEvent> ContributionEvents => Set<ContributionEvent>();

    /// <summary>Keyless read model over <c>vw_ContributionStats</c>. Never written to.</summary>
    public DbSet<ContributionStats> ContributionStats => Set<ContributionStats>();

    // ── Schema v3: the taxonomy engine (پڕۆمپت ٣) ──────────────────────────────
    // Added ALONGSIDE the old tables. Words/WordMeans/Categories/WordSpeechPanes still drive the
    // running app; nothing is dropped until پڕۆمپت ٤ has migrated the rows and you have reviewed
    // the result. Configuration lives in TaxonomyModelConfiguration.
    public DbSet<PartOfSpeech> PartsOfSpeech => Set<PartOfSpeech>();
    public DbSet<FeatureAxis> FeatureAxes => Set<FeatureAxis>();
    public DbSet<FeatureValue> FeatureValues => Set<FeatureValue>();
    public DbSet<PartOfSpeechAxis> PartOfSpeechAxes => Set<PartOfSpeechAxis>();
    public DbSet<Domain> Domains => Set<Domain>();
    public DbSet<Sense> Senses => Set<Sense>();
    public DbSet<SenseFeature> SenseFeatures => Set<SenseFeature>();
    public DbSet<SenseTranslation> SenseTranslations => Set<SenseTranslation>();
    public DbSet<WordFormType> WordFormTypes => Set<WordFormType>();
    public DbSet<WordForm> WordForms => Set<WordForm>();
    public DbSet<RelationTypeDef> RelationTypes => Set<RelationTypeDef>();
    public DbSet<WordRelation> WordRelations => Set<WordRelation>();
    public DbSet<SenseRelation> SenseRelations => Set<SenseRelation>();

    // ── The operations room (پڕۆمپت ٧) ────────────────────────────────────────
    public DbSet<SenseClaim> SenseClaims => Set<SenseClaim>();
    public DbSet<SenseDisagreement> SenseDisagreements => Set<SenseDisagreement>();
    public DbSet<ConsistencySample> ConsistencySamples => Set<ConsistencySample>();

    /// <summary>Durable last-seen (پڕۆمپت ٩). Live status is in memory on the web tier.</summary>
    public DbSet<UserPresence> UserPresences => Set<UserPresence>();
    public DbSet<UserWorkDay> UserWorkDays => Set<UserWorkDay>();
    public DbSet<UserWalkPosition> UserWalkPositions => Set<UserWalkPosition>();
    public DbSet<DictionarySection> DictionarySections => Set<DictionarySection>();

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
            entity.HasIndex(w => w.DictionarySectionId);

            entity.HasOne(w => w.DictionarySection)
                  .WithMany(d => d.Words)
                  .HasForeignKey(w => w.DictionarySectionId)
                  // A section that is retired must not take its words with it.
                  .OnDelete(DeleteBehavior.SetNull);
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

        // ── Contribution ledger ────────────────────────────────────────────────
        modelBuilder.Entity<ContributionEvent>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.EntityType).IsRequired().HasMaxLength(50);
            entity.Property(e => e.FieldName).HasMaxLength(100);
            entity.Property(e => e.OldValue).HasMaxLength(4000);
            entity.Property(e => e.NewValue).HasMaxLength(4000);
            entity.Property(e => e.Note).HasMaxLength(2000);

            // The three questions this table is asked: what has this person contributed, what is the
            // history of this word, and what happened recently.
            entity.HasIndex(e => new { e.UserId, e.EventType });
            entity.HasIndex(e => new { e.WordId, e.OccurredAt });
            entity.HasIndex(e => e.OccurredAt);

            // Restrict, not SetNull: UserId is non-null by design, and a contributor who leaves must
            // not be erasable from the credits. Deactivate the account instead (AppUser.IsActive).
            entity.HasOne(e => e.User)
                  .WithMany()
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Read model — a view, so the numbers can never disagree with the ledger they come from.
        modelBuilder.Entity<ContributionStats>()
                    .HasNoKey()
                    .ToView("vw_ContributionStats");

        // ── Soft delete ────────────────────────────────────────────────────────
        // A global filter on every lexicon entity, so a deleted row is invisible to ordinary queries
        // without a single call site remembering to ask. IgnoreQueryFilters() is the deliberate
        // escape hatch for history and restore screens.
        modelBuilder.Entity<Word>().HasQueryFilter(w => !w.IsDeleted);
        modelBuilder.Entity<Category>().HasQueryFilter(c => !c.IsDeleted);

        // Dependents must repeat their principal's filter. EF requires it explicitly on any entity
        // with a required relationship to a filtered one, or it warns and the join silently returns
        // rows whose parent is hidden.
        modelBuilder.Entity<WordMeans>().HasQueryFilter(m => !m.IsDeleted && !m.Word.IsDeleted);
        modelBuilder.Entity<RelatedWord>().HasQueryFilter(r => !r.IsDeleted && !r.Word.IsDeleted);
        modelBuilder.Entity<WordSpeechPane>().HasQueryFilter(sp => !sp.IsDeleted && !sp.Word.IsDeleted);
        modelBuilder.Entity<WordCategory>().HasQueryFilter(wc => !wc.IsDeleted && !wc.Word.IsDeleted && !wc.Category.IsDeleted);

        // Partial indexes: every list query filters IsDeleted = 0, and once rows start accumulating
        // the deleted ones are dead weight in the index.
        modelBuilder.Entity<Word>().HasIndex(w => w.IsDeleted).HasFilter("[IsDeleted] = 0");
        modelBuilder.Entity<Category>().HasIndex(c => c.IsDeleted).HasFilter("[IsDeleted] = 0");

        modelBuilder.Entity<DictionarySection>(entity =>
        {
            entity.Property(d => d.NameKu).HasMaxLength(128).IsRequired();
            entity.Property(d => d.Normalized).HasMaxLength(128).IsRequired();

            // Unique on the FOLDED name, not the typed one. «کیمیا» with a Persian kaf and with
            // an Arabic one are the same section, and without this the list grows a near-duplicate
            // every time somebody types on a different keyboard — which is exactly how 26
            // categories became 79.
            entity.HasIndex(d => d.Normalized).IsUnique().HasFilter("[IsDeleted] = 0");

            entity.HasQueryFilter(d => !d.IsDeleted);
        });

        modelBuilder.Entity<UserWorkDay>(entity =>
        {
            // One row per person per day. The unique index is what makes the credit an UPSERT
            // rather than an append — without it a restart mid-flush would start a second row for
            // the same day and the total would quietly split in two.
            entity.HasIndex(w => new { w.UserId, w.Date }).IsUnique();

            entity.HasOne(w => w.User)
                  .WithMany()
                  .HasForeignKey(w => w.UserId)
                  .OnDelete(DeleteBehavior.Cascade);

            // Deliberately NOT a presence history. This stores a total per day, never a trail of
            // when somebody was at their desk — پڕۆمپت ٧ is explicit that presence must not
            // become a surveillance log, and a per-minute table is exactly that.
            entity.Property(w => w.Date).HasColumnType("date");
        });

        modelBuilder.Entity<UserWalkPosition>(entity =>
        {
            // One row per person per walk. Unique, so saving a position is an UPSERT — an append
            // would leave a trail of stale places and "where was I" would depend on which row won.
            entity.HasIndex(p => new { p.UserId, p.Walk }).IsUnique();

            entity.Property(p => p.Walk).HasMaxLength(32);

            entity.HasOne(p => p.User)
                  .WithMany()
                  .HasForeignKey(p => p.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<UserPresence>(entity =>
        {
            // The user is the key: one row per person, updated in place. A history of presence is
            // a surveillance log, and پڕۆمپت ٧ is explicit that this must not become one.
            entity.HasKey(p => p.UserId);
            entity.Property(p => p.CurrentPage).HasMaxLength(256);

            entity.HasOne(p => p.User)
                  .WithOne()
                  .HasForeignKey<UserPresence>(p => p.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // ── Schema v3: the taxonomy engine ─────────────────────────────────────
        TaxonomyModelConfiguration.Configure(modelBuilder);

        // The headword's folded form. Indexed because every search will hit it (پڕۆمپت ٥).
        modelBuilder.Entity<Word>().HasIndex(w => w.Normalized);

        // ── NO SEED DATA ───────────────────────────────────────────────────────
        // This method used to HasData() 1,001 synthetic words (Id 10000-11000), 6,006 synthetic
        // relations and 3 food categories. HasData is not a test fixture — EF materialises it into
        // every database the migrations touch, so those rows were indistinguishable from the real
        // lexicon and inflated every count on the dashboard.
        //
        // The lexicon is entered by the team. Nothing about it is seeded here, and nothing should be:
        // see .claude/skills and «md files/claude-code-prompt.md» پڕۆمپت ٣ — "NO SEED DATA, this is a
        // configuration engine". Roles and the first admin account are seeded in DbSeeder at runtime,
        // which is a different thing: those are access, not content.
    }
}
