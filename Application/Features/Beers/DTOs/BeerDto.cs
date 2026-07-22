using System;

namespace Application.Features.Beers.DTOs
{
    public class BeerDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public decimal AlcoholPercentage { get; set; }
        public decimal Price { get; set; }
        public Guid BreweryId { get; set; }
    }
}
