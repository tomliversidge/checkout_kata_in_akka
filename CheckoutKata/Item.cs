namespace CheckoutKata
{
    public class Item
    {
        public string Id { get; }
        public int Quantity { get; }
        public double Price { get; }

        public Item(string id, double price, int quantity)
        {
            Id = id;
            Price = price;
            Quantity = quantity;
        }
    }
}