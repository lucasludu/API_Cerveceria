using Ardalis.Specification;
using Domain.Entities;
using System;

namespace Application.Specification._beer
{
    public class BeersByBreweryIdSpecification : Specification<Beer>
    {
        public BeersByBreweryIdSpecification(Guid breweryId, int pageNumber, int pageSize)
        {
            Query.Where(b => b.BreweryId == breweryId)
                 .Skip((pageNumber - 1) * pageSize)
                 .Take(pageSize);
        }
    }
}
