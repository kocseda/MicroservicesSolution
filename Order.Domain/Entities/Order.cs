namespace Order.Domain.Entities
{
    public class Order
    {
        public Guid Id { get; set; }
        public string CustomerContact { get; set; }
        public List<OrderItem> Items { get; set; }
        public Order()
        {
            Items = new List<OrderItem>();
        }
    }

    public class OrderItem
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }

    public enum OrderStatus
    {
        Pending,
        Approved,
        Rejected
    }
}
