
using System;
using System.Collections.Generic;
namespace StoreModel
{

    public class Customer
    {
        public Customer(string name, string Address)
        {
            this.Name = name;
            this.Address = Address;

        }
        public Customer(string name, string Address, int id) : this(name,Address)
        {
           this.ID = id;
        }

        public override string ToString()
        {
            return String.Format("{0}, {1}, {2}", this.Name, this.Address);
        }
        public string Name { get; set; }
        public string Address { get; set; }
        public int ID { get; set; }
        
    }
}