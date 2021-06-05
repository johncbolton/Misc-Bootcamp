using System;
namespace StoreModel
{

    public class Product
    {
        private double _price;
        private string _productName;
        public Product(string productName, double price)
        {
            this.ProductName = productName;
            this.Price = price;
        }
        public override string ToString()
        {
            return String.Format("{0}, ${1}",this.ProductName, this.Price);
        }
        public string ProductName 
        { 
            get => _productName; 
            set{
                if(String.IsNullOrWhiteSpace(value)){
                    throw new Exception("You must enter a value and name can not have white spaces!");
                }else{
                    _productName= value;
                }
            }
        }
        public double Price 
        { 
            get => _price; 
            set{
                if (value < 0){
                    throw new Exception("Price cannot be < 0");
                }else{
                    _price = value;
                }
            }
        }
    }
}