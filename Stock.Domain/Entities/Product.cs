namespace Stock.Domain.Entities
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int StockQuantity { get; set; }
    }
}