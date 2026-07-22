using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration
{
    public class WholesaleInventoryConfig : IEntityTypeConfiguration<WholesaleInventory>
    {
        public void Configure(EntityTypeBuilder<WholesaleInventory> builder)
        {
            builder.ToTable("WholesaleInventories");
            
            // Composite key
            builder.HasKey(wi => new { wi.WholesalerId, wi.BeerId });

            builder.HasOne(wi => wi.Wholesaler)
                   .WithMany(w => w.WholesaleInventories)
                   .HasForeignKey(wi => wi.WholesalerId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(wi => wi.Beer)
                   .WithMany(b => b.WholesaleInventories)
                   .HasForeignKey(wi => wi.BeerId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Property(wi => wi.StockQuantity).IsRequired().HasDefaultValue(0);

            // Data seeding
            builder.HasData(
                new WholesaleInventory { WholesalerId = Guid.Parse("66666666-6666-6666-6666-666666666666"), BeerId = Guid.Parse("33333333-3333-3333-3333-333333333333"), StockQuantity = 50 },
                new WholesaleInventory { WholesalerId = Guid.Parse("66666666-6666-6666-6666-666666666666"), BeerId = Guid.Parse("44444444-4444-4444-4444-444444444444"), StockQuantity = 20 },
                new WholesaleInventory { WholesalerId = Guid.Parse("77777777-7777-7777-7777-777777777777"), BeerId = Guid.Parse("33333333-3333-3333-3333-333333333333"), StockQuantity = 0 },
                new WholesaleInventory { WholesalerId = Guid.Parse("77777777-7777-7777-7777-777777777777"), BeerId = Guid.Parse("55555555-5555-5555-5555-555555555555"), StockQuantity = 100 }
            );
        }
    }
}
