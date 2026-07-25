using Ardalis.Specification;
using Domain.Entities;

namespace Application.Specification._beer
{
    public class BeerByIdWithBreweriesSpec : Specification<Beer>
    {
        public BeerByIdWithBreweriesSpec(Guid id)
        {
            Query.Where(b => b.Id == id);
        }
    }
}
