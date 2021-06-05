
using System;

namespace StoreModel
{

    /// <summary>
    /// This data structure models a product and its quantity.
    /// </summary>
    public class Item
    {
        private int _quantity;
        public Item(Product product, int quantity)
        {
            this.Product = product;
            this.Quantity = quantity;
        }
        public override string ToString()
        {
            return String.Format("{0} Quantity: {1}",Product.ToString(), Quantity);
        }
        public Product Product { get; set; }

        public int Quantity 
        { 
            get => _quantity; 
            set
            {
                if (value < 0){
                    throw new Exception("You cannot have a negative quantity");
                }else{
                    _quantity = value;
                }
            } 
        }
        public void ChangeQuantity(int num)
        {
            this.Quantity = this.Quantity + num;
        }

    }
}