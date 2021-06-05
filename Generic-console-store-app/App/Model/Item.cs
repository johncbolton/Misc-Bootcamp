using System;

namespace StoreModels
{
    /// <summary>
    /// This data structure models a product and its quantity.
    /// </summary>
    public class Item
    {
        private int _quantity;

        public Item(Product product, int quantity)
        {
            Product = product;
            Quantity = quantity;
        }

        public override string ToString()
        {
            return string.Format("{0} Quantity: {1}", Product.ToString(), Quantity);
        }

        public Product Product { get; set; }

        public int Quantity
        {
            get => _quantity;
            set
            {
                if (value < 0)
                    throw new Exception("You can not have a negative quantity");
                else
                    _quantity = value;
            }
        }

        public void ChangeQuantity(int num)
        {
            Quantity = Quantity + num;
        }
    }
}