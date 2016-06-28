namespace CheckoutKata
{
    public class PaymentRequired
    {
        public double Total { get; }

        public PaymentRequired(double total)
        {
            Total = total;
        }
    }
}