using Domain.Common;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Brewery : BaseEntity
    {
        public required string Name { get; set; }

        public ICollection<Beer> Beers { get; set; } = new List<Beer>();
    }
}
