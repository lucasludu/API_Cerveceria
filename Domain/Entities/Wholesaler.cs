using Domain.Common;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Wholesaler : BaseEntity
    {
        public required string Name { get; set; }

        public ICollection<WholesaleInventory> WholesaleInventories { get; set; } = new List<WholesaleInventory>();
    }
}
