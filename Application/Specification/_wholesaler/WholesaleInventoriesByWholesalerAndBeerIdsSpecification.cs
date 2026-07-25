using Ardalis.Specification;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Application.Specification._wholesaler
{
    public class WholesaleInventoriesByWholesalerAndBeerIdsSpecification : Specification<WholesaleInventory>
    {
        public WholesaleInventoriesByWholesalerAndBeerIdsSpecification(Guid wholesalerId, IEnumerable<Guid> beerIds)
        {
            Query.Where(wi => wi.WholesalerId == wholesalerId && beerIds.Contains(wi.BeerId))
                 .Include(wi => wi.Beer);
        }
    }
}
