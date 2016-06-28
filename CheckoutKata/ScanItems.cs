using System.Collections.Generic;

namespace CheckoutKata
{
    public class ScanItems
    {
        public IEnumerable<Item> Items { get; }

        public ScanItems(IEnumerable<Item> items)
        {
            Items = items;
        }
    }
}