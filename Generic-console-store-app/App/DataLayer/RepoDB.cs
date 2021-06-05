using System.Runtime.CompilerServices;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using Models =StoreModels;
using Entity =Data.Entities;
using System;
using Microsoft.EntityFrameworkCore.Storage;
using Serilog;

namespace DataLayer
{
    public class RepoDB : IRepository
    {
        private Entity.p0runningContext _context;
        public RepoDB(Entity.p0runningContext context)
        {
            _context = context;
        }
        private IDbContextTransaction _transaction;
        public void AddCustomer(Models.Customer customer)
        {
            _context.Customers.Add(
                new Entity.Customer
                {
                    Name = customer.Name,
                    Address = customer.Address,
                }
            );
            _context.SaveChanges();
            _context.ChangeTracker.Clear();
        }

        public void AddLocation(Models.Location location)
        {
            _context.Locations.Add(
                new Entity.Location
                {
                   LocationName = location.LocationName,
                   Address = location.Address
                }
            );
           _context.SaveChanges();
           _context.ChangeTracker.Clear();
        }

        public void AddProduct(Models.Product product)
        {
            _context.Products.Add(
               new Entity.Product
               {
                    Name = product.ProductName,
                    Price = product.Price
               }
            );
            _context.SaveChanges();
            _context.ChangeTracker.Clear();
        }

        public void AddProductToInventory(Models.Location location, Models.Item item)
        {
            _context.InventoryItems.Add( 
                new Entity.InventoryItem
                {
                    Location = GetLocation(location),
                    Product = GetProduct(item.Product),
                    Quantity = item.Quantity
                }
           );
           _context.SaveChanges();
           _context.ChangeTracker.Clear();
        }

        public List<Models.Customer> GetAllCustomers()
        {
            return _context.Customers.Select(
                customer => new Models.Customer(customer.Name, customer.Address, customer.Id)
            ).ToList();
        }

        public List<Models.Location> GetAllLocations()
        {
            return _context.Locations.Select(
                location => new Models.Location(
                    location.LocationName, 
                    location.Address, 
                    location.InventoryItems.Select( 
                        i => new Models.Item(
                            new Models.Product(
                                i.Product.Name, 
                                (double) i.Product.Price
                            ),
                            (int) i.Quantity
                        )
                    ).ToList()
                )
            ).ToList();
        }

        public List<Models.Product> GetAllProducts()
        {
            return _context.Products.Select(
                product => new Models.Product(product.Name, (double) product.Price)
            ).ToList();
        }

        public List<Models.Order> GetOrders(Models.Customer customer, bool price, bool asc)
        {
         
            List<Models.Order> moreOrders=  _context.Orders.Select(
                order => new Models.Order(
                   new Models.Customer(order.Customer.Name, order.Customer.Address, order.Customer.Id),
                   new Models.Location(order.Location.LocationName, order.Location.Address),
                   order.OrderItems.Select(
                       i => new Models.Item(
                           new Models.Product(
                               i.Product.Name,
                               (double) i.Product.Price),
                               (int) i.Quantity)).ToList()
            ).AsEnumerable().Where(order => order.Customer.Name == customer.Name).ToList();

            Func<Models.Order, double> orderbyprice = order => order.Total;
            IOrderedEnumerable<StoreModel.Order> temp = null;

            if(price){
              temp =  moreOrders.OrderBy(orderbyprice);
            }
            if(!asc){
                moreOrders = temp.Reverse().ToList();
            }else{
                moreOrders = temp.ToList();
            }            
            return moreOrders;         
        }

        public List<Models.Order> GetOrders(Models.Location location, bool price, bool asc)
        {
            List<Models.Order> moreOrders=  _context.Orders.Select(
                order => new Models.Order(
                   new Models.Customer(order.Customer.Name, order.Customer.Address, order.Customer.Id),
                   new Models.Location(order.Location.LocationName, order.Location.Address),
                   order.OrderItems.Select(
                       i => new Models.Item(
                           new Models.Product(
                               i.Product.Name,
                               (double) i.Product.Price),
                               (int) i.Quantity)).ToList()

            ).AsEnumerable().Where(order => order.Location.LocationName == location.LocationName).ToList();

            Func<Models.Order, double> orderbyprice = order => order.Total;


            IOrderedEnumerable<StoreModels.Order> temp = null;
            if(price){
              temp =  moreOrders.OrderBy(orderbyprice);
            }
            if(!asc){
                moreOrders = temp.Reverse().ToList();
            }else{
                moreOrders = temp.ToList();
            }            
            return moreOrders;
        }
        
       
        public void PlaceOrder(Models.Order mOrder)
        {  
            List<Entity.OrderItem> items = new List<Entity.OrderItem>{};
            mOrder.Items.ForEach(item => 
                items.Add(
                    new Entity.OrderItem
                    {
 
                        Product = GetProduct(item.Product),
                        Quantity = item.Quantity,
                    })
            );         

            Entity.Order eOrder=  new Entity.Order
            {
                Customer = GetCustomer(mOrder.Customer),
                Location = GetLocation(mOrder.Location),
                Total = mOrder.Total,
                OrderItems = items
            };
            
            try{
            _context.Orders.Add(eOrder);

            _context.SaveChanges();
            _context.ChangeTracker.Clear();
            }catch(Exception ex){
                throw new Exception("Order Failed");
            }


        }
        
        private Entity.Product GetProduct(Models.Product mProduct)
        {
            Entity.Product found = _context.Products.FirstOrDefault(o => (o.Name == mProduct.ProductName)&& (o.Price == mProduct.Price));
            return found;
        }
        public void UpdateInventoryItem(Models.Location location, Models.Item item)
        {
            Entity.Location eLocation = GetLocation(location);
            Entity.InventoryItem eItem = GetInventoryItem(item, eLocation);
            eItem.Quantity = item.Quantity;
            var thing =  _context.InventoryItems.Update(eItem);            
            _context.SaveChanges();
            _context.ChangeTracker.Clear();
        }
        private Entity.InventoryItem GetInventoryItem(Models.Item item, Entity.Location eLocation)
        {
            Entity.InventoryItem found = _context.InventoryItems.FirstOrDefault(o=> (o.Product.Name == item.Product.ProductName) && (o.LocationId == eLocation.Id));
            return found;
        }
                private Entity.Location GetLocation(Models.Location mLocation)
        {
            Entity.Location found =  _context.Locations.FirstOrDefault( o => (o.LocationName == mLocation.LocationName) && (o.Address == mLocation.Address));
            return found;
        }
        private Entity.Customer GetCustomer(Models.Customer mCustomer)
        {
            Entity.Customer found =  _context.Customers.FirstOrDefault( o => o.Id == mCustomer.ID);
            return found;
        }
        public void StartTransaction()
        {
          _transaction = _context.Database.BeginTransaction();
        }

        public void EndTransaction(bool success)
        {
            if(success){
                _transaction.Commit();
            }else{
                _transaction.Rollback();
            }
        }
    }
}