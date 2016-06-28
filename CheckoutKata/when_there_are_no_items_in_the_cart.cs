using System.Collections.Generic;
using Akka.Actor;
using Akka.TestKit.Xunit2;
using Xunit;

namespace CheckoutKata
{
    public class when_there_are_no_items_in_the_cart : TestKit
    {
        [Fact]
        public void no_payment_should_be_required()
        {
            var checkout = Sys.ActorOf(Props.Create(() => new Checkout(new List<Offer>())), "checkout");
            checkout.Tell(new ScanItems(new List<Item>()));
            ExpectMsg<NoPaymentRequired>();
        }
    }
}