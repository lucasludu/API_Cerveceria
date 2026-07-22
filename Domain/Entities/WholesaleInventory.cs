namespace Domain.Entities
{
    public class WholesaleInventory
    {
        public Guid WholesalerId { get; set; }
        public Wholesaler Wholesaler { get; set; } = null!;

        public Guid BeerId { get; set; }
        public Beer Beer { get; set; } = null!;

        public int StockQuantity { get; set; }
    }
}
