using backend.Data.Models;
using Microsoft.EntityFrameworkCore;

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
    }
}
