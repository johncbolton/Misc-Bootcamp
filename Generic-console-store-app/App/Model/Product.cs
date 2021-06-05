using System;

namespace StoreModels
{
    public class Product
    {
        private double _price;
        private string _productName;

        public Product(string productName, double price)
        {
            ProductName = productName;
            Price = price;
        }

        public override string ToString()
        {
            return string.Format("{0}, ${1}", ProductName, Price);
        }

        public string ProductName
        {
            get => _productName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new Exception("You must enter a value and name can not have white spaces!");
                else
                    _productName = value;
            }
        }

        public double Price
        {
            get => _price;
            set
            {
                if (value < 0)
                    throw new Exception("Price cannot be < 0");
                else
                    _price = value;
            }
        }
    }
}