namespace Application.DTOs.Response._wholesaler
{
    public class QuoteSummaryResponse
    {
        public decimal TotalPrice { get; set; }
        public required string Summary { get; set; }
    }
}
