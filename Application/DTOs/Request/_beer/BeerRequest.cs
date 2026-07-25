namespace Application.DTOs.Request._beer
{
    public class BeerRequest
    {
        public required string Name { get; set; }
        public decimal AlcoholPercentage { get; set; }
        public decimal Price { get; set; }
        public Guid BreweryId { get; set; }
    }
}
