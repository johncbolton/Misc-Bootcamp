using StoreModels;
using System.Collections.Generic;

namespace Service
{
    public interface IService
    {
        public void AddCustomer(string name, string address, string email);

        public List<Customer> GetAllCustomers();

        public List<Location> GetAllLocations();

        public List<Product> GetAllProducts();

        public Customer SearchCustomers(string name);

        public List<Order> GetOrders(Customer customer, bool price, bool asc);

        public List<Order> GetOrders(Location location, bool price, bool asc);

        public void AddProduct(string productName, double productPrice);

        public void AddProductToInventory(Location location, Product product, int stock);

        public void AddLocation(string name, string address);

        public void PlaceOrder(Location location, Customer customer, List<Item> items);

        public void updateItemInStock(Location location, Item item, int amount);

        public double CalculateOrderTotal(List<Item> items);
    }
}