using Microsoft.EntityFrameworkCore;
using PampaLeche.Domain.Entities;

namespace PampaLeche.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public DbSet<MilkBatch> Batches { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MilkBatch>(b =>
        {
            b.HasKey(x => x.Id);
            b.Property(x => x.BatchCode).IsRequired().HasMaxLength(50);
            b.Property(x => x.Status).IsRequired();
            b.OwnsOne(x => x.InitialTemp);
            b.OwnsOne(x => x.StorageTemp);
            b.OwnsOne(x => x.Fat);
            b.OwnsOne(x => x.FarmLocation);
            b.OwnsOne(x => x.Origin);
        });
    }
}
