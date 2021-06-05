using System;
using System.Net.Mail;

namespace StoreModels
{
    public class Customer
    {
        public Customer(string name, string Address, MailAddress Email)
        {
            Name = name;
            this.Address = Address;
            this.Email = Email;
        }

        public Customer(string name, string Address, MailAddress Email, int id) : this(name, Address, Email)
        {
            ID = id;
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}, {2}", Name, Address, Email);
        }

        public string Name { get; set; }
        public string Address { get; set; }
        public MailAddress Email { get; set; }
        public int ID { get; set; }
    }
}