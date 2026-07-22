using Domain.Common;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Beer : BaseEntity
    {
        public required string Name { get; set; }
        public decimal AlcoholPercentage { get; set; }
        public decimal Price { get; set; }

        public Guid BreweryId { get; set; }
        public Brewery Brewery { get; set; } = null!;
        
        public ICollection<WholesaleInventory> WholesaleInventories { get; set; } = new List<WholesaleInventory>();
    }
}
