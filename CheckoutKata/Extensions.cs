using System;

namespace CheckoutKata
{
    public static class Extensions
    {
        public static double CalculateDiscount(this double quantity, int discountQualificationQuantity, double discount) =>
            Math.Floor(quantity / discountQualificationQuantity) * discount;
    }
}