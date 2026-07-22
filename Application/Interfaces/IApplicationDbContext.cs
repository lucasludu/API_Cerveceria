using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IApplicationDbContext
    {
        
        DbSet<Brewery> Breweries { get; set; }
        DbSet<Beer> Beers { get; set; }
        DbSet<Wholesaler> Wholesalers { get; set; }
        DbSet<WholesaleInventory> WholesaleInventories { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        DatabaseFacade Database { get; }
    }
}
