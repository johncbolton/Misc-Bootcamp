using System.Linq;
using System;
using System.Collections.Generic;
using Serilog;
using Service;
using StoreModels;


namespace StoreUI
{
    public class MainMenu : IMenu
    {   
        private IService _services;
        private IValidationUI _validate;

        public MainMenu(IService services, IValidationUI validation)
        {
            _services = services;
            _validate = validation;
        }

        public void Start()
        {   

            bool repeat = true;
            do{
                Console.Clear();
                Console.WriteLine("Main Menu, please input your selection:");
                Console.WriteLine("[0] Exit");
                Console.WriteLine("[1] Search For Customer");
                Console.WriteLine("[2] View Orders");
                Console.WriteLine("[3] Create Order");
                Console.WriteLine("[4] Admin Menu");

                
                string input = Console.ReadLine();
                switch (input)
                {
                    case "0":
                        Console.WriteLine("Goodbye");
                        repeat = false;
                    break;
                    case "1":
                        SearchForCustomer();
                    break;

                    case "2":
                        ViewOrders();
                    break;
                    case "3":
                        CreateNewOrder();

                    break;
                    case "4":
                    MenuFactory.GetMenu("adminmenu").Start();
                           
                    break;
                    default:
                        Console.WriteLine("Please input a valid choice.");
                    break;

                }
            } while(repeat);
            
        }

        private void SearchForCustomer()
        {
            string str;
            str = _validate.ValidationPrompt("Enter the Customer Name:", ValidationService.ValidatePersonName);
            Customer target = null;
            try{
                target =  _services.SearchCustomers(str);
                Console.Clear();
                Console.WriteLine("Customer found: {0}", target.ToString());
            }catch(Exception ex){

                Console.WriteLine(ex.Message);
            }
        }


        private void ViewOrders()
        {

            bool repeat = true;
                do
                {
                    Console.Clear();
                    Console.WriteLine("View Orders:");
                    Console.WriteLine("[0] Exit");
                    Console.WriteLine("[1] View By customer");
                    Console.WriteLine("[2] View By Location");
                    String str = Console.ReadLine();
                    switch (str) {
                        case "0":
                           repeat = false;
                        break;
                        case "1":
                            ViewByCustomer();
                        break;
                        case "2":

                            ViewByLocation();
                        break;
                    }
                } while (repeat);
        }

        private void ViewByCustomer()
        {
            try{ 
                List<Object> objs = _services.GetAllCustomers().Cast<Object>().ToList<Object>();
                
                Object ret = SelectFromList.Start(objs);
                Customer customer = (Customer) ret;

  
                bool inpt = true;
                bool price = true;
                bool asc = true;
                string str;
                do{
                    Console.Clear();
                    Console.WriteLine("[0] Sort By Price Ascending");
                    Console.WriteLine("[1] Sort By Price Descending");
                    str = Console.ReadLine();
                    switch(str){
                        case "0":
                            price = true;
                            asc = true;
                            inpt = false;
                        break;
                        case "1":
                            price = true;
                            asc = false;
                            inpt = false;
                        break;
                        default:
                            Console.WriteLine("Invalid entry.");
                        break;
                    }
                }while(inpt);


                List<Object> orderList =  _services.GetOrders(customer, price, asc).Cast<Object>().ToList<Object>();
                ret = SelectFromList.Start(orderList);
                Order o = (Order) ret;
                List<Item> items = o.Items;
                items.ForEach(d => Console.WriteLine(d.ToString()));

            }catch(NullReferenceException ex){
                Console.WriteLine("Cancelled Selection");
            }catch(Exception ex){
                Log.Error(ex, ex.Message);
            }
        }
        private void ViewByLocation()
        {
            try{ 
                List<Object> objs = _services.GetAllLocations().Cast<Object>().ToList<Object>();
                
                Object ret = SelectFromList.Start(objs);
                Location location = (Location) ret;

                bool inpt = true;
                bool price = true;
                bool asc = true;
                string str;
                do{
                    Console.Clear();
                    Console.WriteLine("[0] Sort By Price Ascending");
                    Console.WriteLine("[1] Sort By Price Descending");
                    str = Console.ReadLine();
                    switch(str){
                        case "0":
                            price = true;
                            asc = true;
                            inpt = false;
                        break;
                        case "1":
                            price = true;
                            asc = false;
                            inpt = false;
                        break;
                        default:
                            Console.WriteLine("Invalid entry.");
                        break;
                    }
                }while(inpt);


                List<Object> orderList =  _services.GetOrders(location, price, asc).Cast<Object>().ToList<Object>();
                ret = SelectFromList.Start(orderList);
                Order o = (Order) ret;
                List<Item> items = o.Items;
                items.ForEach(d => Console.WriteLine(d.ToString()));

            }catch(NullReferenceException ex){
                Console.WriteLine("Cancelled Selection");
            }catch(Exception ex){
                Log.Error(ex, ex.Message);
            }
        }


        private List<Item> GetItems(Location loc)
        {
            List<Item> selectedItem = new List<Item>();
            string str;
            bool cont = true;
                try{
                    do{
                        Console.Clear();
                        List<Object> objectList = loc.Inventory.Cast<Object>().ToList<Object>();
                        
                        Object ret = SelectFromList.Start(objectList);
                        Item itm = (Item) ret;
                        Product p = itm.Product;
                        str = _validate.ValidationPrompt("Enter Quantity to Order:", ValidationService.ValidatePositiveInt);

                        selectedItem.Add(new Item(p, int.Parse(str)));
                        bool innercont = true;
                        do{
                            Console.Clear();
                            Console.WriteLine("[0] Continue With Order");
                            Console.WriteLine("[1] Add Another Item");
                            str = Console.ReadLine();
                            switch(str){
                                case "0":
                                    innercont = false;
                                    cont = false;
                                break;
                                case "1":
                                    innercont = false;
                                break;
                                default:
                                    Console.WriteLine("Invalid entry.");
                                break;
                            }
                        }while(innercont);

                    }while(cont);
                    
                }catch(NullReferenceException ex){
                    Console.WriteLine("Cancelled Item Selection");
                    
                }catch(Exception ex){
                    Log.Error(ex, ex.Message);
                    
                }
                return selectedItem;
        }

        private void CreateNewOrder()
        {

            Customer cust = GetCustomer();
            if (cust == null) return;
            Location loc = GetLocation();
            if(loc == null) return;
            List<Item> itms = GetItems(loc);
            if(itms.Count == 0) return;
            Double total = _services.CalculateOrderTotal(itms);
            Console.WriteLine("The order Total is: {0}", total);
            Console.WriteLine("Press C to complete order\n");
            ConsoleKeyInfo key = Console.ReadKey();
            if (key.Key.ToString().ToLower()== "C")
            {
                try{
                    _services.PlaceOrder(loc, cust, itms);
                    Console.WriteLine("Order has been Placed!");

                }catch(Exception ex ){
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private Customer GetCustomer()
        {
            Customer cust = null;
             try{ 
                List<Object> objs = _services.GetAllCustomers().Cast<Object>().ToList<Object>();
                
                Object ret = SelectFromList.Start(objs);
                cust = (Customer) ret;

                Console.Clear();
                Console.WriteLine("Customer selected: {0}", cust.ToString());

            }catch(NullReferenceException ex){
                Console.WriteLine("Cancelled Selection!");
                
            }catch(Exception ex){
               Log.Error(ex, ex.Message);
            }
            return cust;
        }
        private Location GetLocation()
        {
            Location loc = null;
            try{ 
                List<Object> objectList = _services.GetAllLocations().Cast<Object>().ToList<Object>();
                
                Object ret = SelectFromList.Start(objectList);
                loc = (Location) ret;
                Console.Clear();
                Console.WriteLine("Location selected: {0}", loc.ToString());
                

            }catch(NullReferenceException ex){
                Console.WriteLine("Cancelled Location Selection");
               
            }catch(Exception ex){
                Log.Error(ex, ex.Message);
              
            }
            return loc;
        }
    }
}