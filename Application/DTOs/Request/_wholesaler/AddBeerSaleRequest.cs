using System;

namespace Application.DTOs.Request._wholesaler
{
    public class AddBeerSaleRequest
    {
        public Guid BeerId { get; set; }
        public Guid WholesalerId { get; set; }
        public int Quantity { get; set; }
    }
}
