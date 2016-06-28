using System.Collections.Generic;
using System.Linq;
using Akka.Actor;

namespace CheckoutKata
{
    public class Checkout : ReceiveActor
    {
        public Checkout(IEnumerable<Offer> offers)
        {
            Receive<ScanItems>(msg =>
            {
                var total = Subtotal(msg.Items) - Discount(msg.Items, offers);
                if (total > 0)
                {
                    Sender.Tell(new PaymentRequired(total));
                }
                else
                {
                    Sender.Tell(new NoPaymentRequired());
                }
            });
        }
        private static double Subtotal(IEnumerable<Item> items) =>
            items.Aggregate(0.0, (runningTotal, item) => (item.Price * item.Quantity) + runningTotal);

        private static double Discount(IEnumerable<Item> items, IEnumerable<Offer> offers)
            => offers.Aggregate(0.0, (runnintTotal, offer) => CalculateDiscount(items, offer) + runnintTotal);

        private static double CalculateDiscount(IEnumerable<Item> items, Offer offer) =>
            items
                .Where(i => i.Id == offer.ItemId)
                .Aggregate(0.0, (total, item) => item.Quantity + total)
                .CalculateDiscount(offer.DiscountQualificationQuantity, offer.Discount);
    }
}