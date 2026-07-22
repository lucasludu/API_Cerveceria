using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration
{
    public class BeerConfig : IEntityTypeConfiguration<Beer>
    {
        public void Configure(EntityTypeBuilder<Beer> builder)
        {
            builder.ToTable("Beers");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
            builder.Property(x => x.AlcoholPercentage).HasColumnType("decimal(5,2)");
            builder.Property(x => x.Price).HasColumnType("decimal(18,2)");

            // Data seeding
            builder.HasData(
                new Beer { Id = Guid.Parse("33333333-3333-3333-3333-333333333333"), Name = "Leffe Blonde", AlcoholPercentage = 6.6m, Price = 2.20m, BreweryId = Guid.Parse("11111111-1111-1111-1111-111111111111") },
                new Beer { Id = Guid.Parse("44444444-4444-4444-4444-444444444444"), Name = "Leffe Brune", AlcoholPercentage = 6.5m, Price = 2.40m, BreweryId = Guid.Parse("11111111-1111-1111-1111-111111111111") },
                new Beer { Id = Guid.Parse("55555555-5555-5555-5555-555555555555"), Name = "Hoegaarden White", AlcoholPercentage = 4.9m, Price = 1.90m, BreweryId = Guid.Parse("22222222-2222-2222-2222-222222222222") }
            );
        }
    }
}
