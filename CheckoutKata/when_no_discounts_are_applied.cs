using System.Collections.Generic;
using Akka.Actor;
using Akka.TestKit.Xunit2;
using Xunit;

namespace CheckoutKata
{
    public class when_no_discounts_are_applied : TestKit
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
        public void the_total_should_be_the_total_cost_of_the_items()
        {
            var checkout = Sys.ActorOf(Props.Create(() => new Checkout(new List<Offer>())), "checkout");
            checkout.Tell(new ScanItems(_items));
            var payment = ExpectMsg<PaymentRequired>();
            Assert.Equal(410, payment.Total);
        }
    }
}