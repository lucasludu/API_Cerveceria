using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration
{
    public class BreweryConfig : IEntityTypeConfiguration<Brewery>
    {
        public void Configure(EntityTypeBuilder<Brewery> builder)
        {
            builder.ToTable("Breweries");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(150);

            builder.HasMany(b => b.Beers)
                   .WithOne(b => b.Brewery)
                   .HasForeignKey(b => b.BreweryId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Data seeding
            builder.HasData(
                new Brewery { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Name = "Brouwerij Abbaye de Leffe" },
                new Brewery { Id = Guid.Parse("22222222-2222-2222-2222-222222222222"), Name = "Brouwerij Hoegaarden" }
            );
        }
    }
}
