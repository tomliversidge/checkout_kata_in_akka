using System.Text;
using System.Threading.Tasks;
using Akka.TestKit;

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
}
