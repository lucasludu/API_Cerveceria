using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration
{
    public class WholesalerConfig : IEntityTypeConfiguration<Wholesaler>
    {
        public void Configure(EntityTypeBuilder<Wholesaler> builder)
        {
            builder.ToTable("Wholesalers");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(150);

            // Data seeding
            builder.HasData(
                new Wholesaler { Id = Guid.Parse("66666666-6666-6666-6666-666666666666"), Name = "Geneva Drinks" },
                new Wholesaler { Id = Guid.Parse("77777777-7777-7777-7777-777777777777"), Name = "Bruxelles Beverages" }
            );
        }
    }
}
