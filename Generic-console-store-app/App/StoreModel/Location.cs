using System;
using System.Globalization;
using System.Collections.Generic;
namespace StoreModel
{

    public class Location
    {
        public Location(string locationName, string address)
        {
            this.Address = address;
            this.LocationName = locationName;
            Inventory  = new List<Item>();
        }
        public Location(string locationName, string address, List<Item> inventory)
        {
            this.Address = address;
            this.LocationName = locationName;
            this.Inventory  = inventory;
        }
        public override string ToString()
        {
            return String.Format("{0} Address: {1}",this.LocationName,this.Address);
        }
        public string Address { get; set; }
        public string LocationName { get; set; }
        public List<Item> Inventory { get; set; }
    }
}