using System;

namespace Application.DTOs.Response._beer
{
    public class BeerResponse
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public decimal AlcoholPercentage { get; set; }
        public decimal Price { get; set; }
        public Guid BreweryId { get; set; }
    }
}
