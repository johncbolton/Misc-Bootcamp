using System;
using System.Collections.Generic;

namespace StoreModel
{
    /// <summary>
    /// This class should contain all the fields and properties that define a customer order. 
    /// </summary>
    public class Order
    {

        private double _total;

        public Order(Customer customer, Location location, List<Item> items)
        {
            this.Customer = customer;
            this.Location = location;
            this.Items = items;
            CalculateTotal();
        }

        public override string ToString()
        {
            return String.Format("Customer: {0} Location: {1}\n\tOn: {2} Total:{3} ", this.Customer,this.Location,this._total);
        }

        public Customer Customer { get; set; }
        public Location Location { get; set; }
        public List<Item> Items { get; set; }

        public double Total { get=> _total; set{CalculateTotal();}}
        
        private void CalculateTotal()
        {
            _total = 0;
            foreach (var Item in this.Items)
            {
                _total += Item.Product.Price * Item.Quantity;
            }
        }
    }
}