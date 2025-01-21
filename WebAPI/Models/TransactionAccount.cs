namespace WebAPI.Models
{
    public class TransactionAccount
    {
        public string? Date { get; set; }
        public string? Type { get; set; }
        public string? Counterparty { get; set; }
        public double? Amount { get; set; }
        public string? Description { get; set; }
    }
}
