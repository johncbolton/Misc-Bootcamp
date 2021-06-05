using System.Collections.Generic;
using StoreModels;
namespace DataLayer
{
    public interface IRepository
    {

        public void PlaceOrder(Order order);
        public List<Order> GetOrders(Customer customer, bool price, bool asc);
        public List<Order> GetOrders(Location location, bool price, bool asc);
        void UpdateInventoryItem(Location location, Item item);
        public void AddProductToInventory(Location location, Item item);
        public void StartTransaction();
        public void EndTransaction(bool success);
        public void AddCustomer(Customer customer);
        public void AddLocation(Location location);
        public void AddProduct(Product product);
        public List<Customer> GetAllCustomers();
        public List<Location> GetAllLocations();
        public List<Product> GetAllProducts();
    }
}