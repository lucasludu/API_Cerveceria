using Ardalis.Specification;
using Domain.Entities;

namespace Application.Specification._beer
{
    public class ExistsBrewerySpecification : Specification<Brewery>
    {
        public ExistsBrewerySpecification(Guid guid)
        {
            Query.Where(b => b.Id == guid);
        }
    }
}
