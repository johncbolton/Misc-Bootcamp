using System;
using System.Collections.Generic;

namespace StoreModels
{
    /// <summary>
    /// This class should contain all the fields and properties that define a
    /// customer order.
    /// </summary>
    public class Order
    {
        public readonly DateTime _date;
        private double _total;

        public Order(Customer customer, Location location, List<Item> items)
        {
            Customer = customer;
            Location = location;
            Items = items;
            _date = DateTime.Now;
            CalculateTotal();
        }

        public Order(Customer customer, Location location, List<Item> items, DateTime date)
        {
            Customer = customer;
            Location = location;
            Items = items;
            _date = date;
            CalculateTotal();
        }

        public override string ToString()
        {
            return string.Format("Customer: {0} Location: {1}\n\tOn: {2} Total:{3} ", Customer, Location, _date,
                _total);
        }

        public Customer Customer { get; set; }
        public Location Location { get; set; }
        public List<Item> Items { get; set; }

        public double Total
        {
            get => _total;
            set => CalculateTotal();
        }

        private void CalculateTotal()
        {
            _total = 0;
            foreach (var Item in Items) _total += Item.Product.Price * Item.Quantity;
        }
    }
}