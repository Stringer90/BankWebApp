namespace WebAPI.Models
{
    public class Transaction
    {
        public string? Date { get; set; }
        public int? Sender { get; set; }
        public int? Receiver { get; set; }
        public string? Type { get; set; }
        public double? Amount { get; set; }
        public string? Description { get; set; }
    }
}
