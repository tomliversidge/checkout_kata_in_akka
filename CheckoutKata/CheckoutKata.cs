using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.TestKit;
using Akka.TestKit.Xunit2;
using Xunit;

namespace CheckoutKata
{
    public class Offer
    {
        public string ItemId { get; }
        public int DiscountQualificationQuantity { get; }
        public double Discount { get; }

        public Offer(string itemId, int discountQualificationQuantity, double discount)
        {
            ItemId = itemId;
            DiscountQualificationQuantity = discountQualificationQuantity;
            Discount = discount;
        }
    }

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

    public class Checkout : ReceiveActor
    {
        public Checkout(IEnumerable<Offer> offers)
        {
            Receive<ScanItems>(msg =>
            {
                Sender.Tell(new PaymentRequired(Subtotal(msg.Items) - Discount(msg.Items, offers))); 
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

    public class PaymentRequired
    {
        public double Total { get; set; }

        public PaymentRequired(double total)
        {
            Total = total;
        }
    }

    public class ScanItems
    {
        public IEnumerable<Item> Items { get; }

        public ScanItems(IEnumerable<Item> items)
        {
            Items = items;
        }
    }

    public static class Extensions
    {
        public static double CalculateDiscount(this double quantity, int discountQualificationQuantity, double discount) =>
         Math.Floor(quantity / discountQualificationQuantity) * discount;
    }

    public class CheckoutTests : TestKit
    {
        private readonly IEnumerable<Item> _items = new[]
        {
            new Item("A", 50.00, 3),
            new Item("A", 50.00, 2),
            new Item("A", 50.00, 1),
            new Item("B", 30.00, 1),
            new Item("B", 30.00, 1),
            new Item("B", 30.00, 1),
            new Item("C", 20.00, 1),
        };

        [Fact]
        public void when_no_discounts_are_applied()
        {
            var checkout = Sys.ActorOf(Props.Create(() => new Checkout(new List<Offer>())), "checkout");
            checkout.Tell(new ScanItems(_items));
            var payment = ExpectMsg<PaymentRequired>();
            Assert.Equal(410, payment.Total);
        }

        [Fact]
        public void when_discounts_are_applied()
        {
            var offers = new[]
              {
                new Offer("A", 3, 20),
                new Offer("B", 2, 15)
            };
            var checkout = Sys.ActorOf(Props.Create(() => new Checkout(offers)), "checkout");
            checkout.Tell(new ScanItems(_items));
            var payment = ExpectMsg<PaymentRequired>();
            Assert.Equal(355, payment.Total);
        }
    }
}
