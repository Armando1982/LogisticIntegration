using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using LogisticIntegration.Domain.Settlement;

namespace LogisticIntegration.Infrastructure.Persistence.Configurations
{
    public class TripSettlementConfiguration : IEntityTypeConfiguration<TripSettlement>
    {
        public void Configure(EntityTypeBuilder<TripSettlement> builder)
        {
            builder.ToTable("TripSettlements");

            builder.HasKey(x => x.Id);

            // Configure Penalty as an Owned Type (Value Object)
            builder.OwnsOne(x => x.Penalty);

            // Configure ProviderLoads relationship
            // Access the backing field for the collection navigation
            builder.Metadata
                .FindNavigation(nameof(TripSettlement.ProviderLoads))!
                .SetPropertyAccessMode(PropertyAccessMode.Field);

            // Configure the HasMany/WithOne relationship with shadow foreign key
            builder.HasMany(x => x.ProviderLoads)
                .WithOne()
                .HasForeignKey("TripSettlementId")
                .IsRequired();
        }
    }
}
