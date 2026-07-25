using System;
using System.Collections.Generic;

namespace Application.DTOs.Request._wholesaler
{
    public class OrderItemRequest
    {
        public Guid BeerId { get; set; }
        public int Quantity { get; set; }
    }

    public class RequestQuoteRequest
    {
        public Guid WholesalerId { get; set; }
        public List<OrderItemRequest> Items { get; set; } = new List<OrderItemRequest>();
    }
}
