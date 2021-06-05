using System;
using System.Collections.Generic;

namespace StoreModels
{
    public class Location
    {
        public Location(string locationName, string address)
        {
            Address = address;
            LocationName = locationName;
            Inventory = new List<Item>();
        }

        public Location(string locationName, string address, List<Item> inventory)
        {
            Address = address;
            LocationName = locationName;
            Inventory = inventory;
        }

        public override string ToString()
        {
            return string.Format("{0} Address: {1}", LocationName, Address);
        }

        public string Address { get; set; }
        public string LocationName { get; set; }
        public List<Item> Inventory { get; set; }
    }
}