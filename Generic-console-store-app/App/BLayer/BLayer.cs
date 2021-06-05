
using System;
using StoreModel;
using DataLayer;
using System.Collections.Generic;
using Serilog;
using System.ComponentModel;

namespace Blayer
{
    public class Services : IService
    {
        private IRepository _repo;
      
       
        public Services(IRepository repo)
        {
            _repo = repo;
         
        }

        public void AddCustomer(string name, string address)
        {   


             Customer newCustomer = null;
            try{

                newCustomer = new Customer(name, address);
            }catch(Exception ex){
                Log.Error("Could not create, {0}\n{1}",ex.Message, ex.StackTrace);
            }

            if(CheckForCustomer(newCustomer, _repo.GetAllCustomers()))
            {

                throw new Exception("Customer Already Exits!");
            }
            try{
                _repo.AddCustomer(newCustomer);

            }catch(Exception ex){

                Log.Error("Failed to Add Customer. {0}\n{1}",ex.Message, ex.StackTrace);
                throw new Exception("Failed to Add Customer!");
            }
        }
        

        public void AddLocation(string name, string address)
        {
            Location newLocation = new Location(name, address);
            if(CheckForLocations(newLocation, _repo.GetAllLocations()))
            {

                throw new Exception("Location Already Exits");
            }
            try{
                _repo.AddLocation(newLocation);
            }catch(Exception ex){
                Log.Error("Failed to Add Location. {0}",ex.Message);
                throw new Exception("Failed to Add Location");
            }
        }

        public void AddProduct(string productName, double productPrice)
        {
            Product newProduct = new Product(productName, productPrice);
            if(CheckForProduct(newProduct, _repo.GetAllProducts()))
            {
                throw new Exception("Product Already Exits");
            }
            try{
                _repo.AddProduct(newProduct);
            }catch(Exception ex){
                throw new Exception("Failed to Add Product");
            }
        }

        public void AddProductToInventory(Location location, Product product, int stock)
        {

            Item newItem = new Item(product, stock);
            foreach (Item item in location.Inventory)
            {
                if(newItem.Product.ProductName == item.Product.ProductName)
                {
                    throw new Exception("In Inventory");
                }
            }

            try{
                location.Inventory.Add(newItem);
                _repo.AddProductToInventory(location, newItem);
            }catch(Exception ex){
                throw new Exception("Failed to Add product");
            }
        }

        public List<Customer> GetAllCustomers()
        {
            List<Customer> retVal;
            retVal = _repo.GetAllCustomers();
            return retVal;
        }
        public List<Location> GetAllLocations()
        {
            List<Location> retVal;
            retVal = _repo.GetAllLocations();
            return retVal;
        }

        public List<Order> GetOrders(Customer customer, bool price, bool asc)
        {
           return _repo.GetOrders(customer, price, asc);
        }

        public List<Order> GetOrders(Location location, bool price, bool asc)
        {
            return _repo.GetOrders(location, price, asc);
        }

        public List<Product> GetAllProducts()
        {
           return _repo.GetAllProducts();
        }

        public void PlaceOrder(Location location, Customer customer, List<Item> items)
        {
            Order order = new Order(customer, location, items);

            _repo.StartTransaction();
            try{
                foreach (Item item in items)
                {
                    SellItems(location, item);
                }
            }catch(Exception ex){
                _repo.EndTransaction(false);
                throw new Exception("Not enough of an Item in stock. Order Failed.");
            }

            try{
            _repo.PlaceOrder(order);
            _repo.EndTransaction(true);

            }catch(Exception ex )
            {
                _repo.EndTransaction(false);
                throw new Exception("Order Failed");
            }
        }
        private void SellItems(Location location, Item oItem)
        {

            List<Item> sItems = location.Inventory;
            Item lItem = sItems.Find(i => i.Product == oItem.Product);
            lItem.ChangeQuantity(-oItem.Quantity); 
            _repo.UpdateInventoryItem(location, lItem);
        }

        public Customer SearchCustomers(string name)
        {
            Log.Verbose("Searching for Customer: {0}",name);         
            List<Customer> customers = GetAllCustomers();
            
            foreach (Customer item in customers)
            {
                if(name == item.Name)
                {
                    Log.Verbose("Found Customer {0}",item.Name);
                    return item;
                }
            }
            Log.Verbose("Customer: {name} not found", name);
            throw new Exception("Customer not found");
                        
        }

        public void updateItemInStock(Location location, Item item, int amount)
        {   

            item.ChangeQuantity(amount);
            try{
                _repo.UpdateInventoryItem(location, item);

            }catch(Exception ex){
                item.ChangeQuantity(-amount);
            }
        }

        private bool CheckForCustomer(Customer customer, List<Customer> Customers)
        {

            foreach (Customer item in Customers)
            {
                if(customer.Name == item.Name && customer.Address == item.Address)
                {
                    return true;
                }
            }
            return false;
        }
        private bool CheckForLocations(Location location, List<Location> locations)
        {

            foreach (Location item in locations)
            {
                if((location.LocationName == item.LocationName)&&(location.Address == location.Address))
                {
                    return true;
                }
            }

            return false;
        }
        private bool CheckForProduct(Product product, List<Product> products)
        {
            foreach (Product item in products)
            {
                if(item.ProductName == product.ProductName)
                {
                    return true;
                }
            }
            return false;
        }

        public double CalculateOrderTotal(List<Item> items)
        {
            double total = 0;
            foreach(Item item in items)
            {
               total += item.Product.Price * item.Quantity;
            }
            return total;
        }
    }
}