using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PampaLeche.Domain.Entities;

namespace PampaLeche.Infrastructure.Persistence.Configurations;

public class MilkBatchConfiguration : IEntityTypeConfiguration<MilkBatch>
{
    public void Configure(EntityTypeBuilder<MilkBatch> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.BatchCode).IsRequired().HasMaxLength(50);
        builder.Property(x => x.Status).IsRequired();
    }
}
