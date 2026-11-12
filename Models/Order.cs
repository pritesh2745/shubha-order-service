namespace OrderService.Models
{
    public class Order
    {
        public Guid OrderId { get; set; }
        public int BraceletId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime OrderTimestampUtc { get; set; }
    }
}
