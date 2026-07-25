using Ardalis.Specification;
using Domain.Entities;

namespace Application.Specification._wholesaler
{
    public class WholesaleInventoryByBeerAndWholesalerSpecification : SingleResultSpecification<WholesaleInventory>
    {
        public WholesaleInventoryByBeerAndWholesalerSpecification(Guid beerId, Guid wholesalerId)
        {
            Query.Where(wi => wi.BeerId == beerId && wi.WholesalerId == wholesalerId);
        }
    }
}
