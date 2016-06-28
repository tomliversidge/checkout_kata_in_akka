using System.Collections.Generic;
using Akka.Actor;
using Akka.TestKit.Xunit2;
using Xunit;

namespace CheckoutKata
{
    public class when_discounts_are_applied : TestKit
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
        public void the_total_should_be_the_total_cost_minus_the_discount()
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