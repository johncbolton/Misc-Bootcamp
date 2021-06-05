using Serilog;
using Service;
using StoreModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UI
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
            var repeat = true;
            do
            {
                Console.Clear();
                Console.WriteLine("Main Menu, please input your selection:");
                Console.WriteLine("[0] Exit");
                Console.WriteLine("[1] Search For Customer");
                Console.WriteLine("[2] View Orders");
                Console.WriteLine("[3] Create Order");
                Console.WriteLine("[4] Admin Menu");

                var input = Console.ReadLine();
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
            } while (repeat);
        }

        private void SearchForCustomer()
        {
            string str;
            str = _validate.ValidationPrompt("Enter the Customer Name:", ValidationService.ValidatePersonName);
            Customer target = null;
            try
            {
                target = _services.SearchCustomers(str);
                Console.Clear();
                Console.WriteLine("Customer found: {0}", target.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void ViewOrders()
        {
            var repeat = true;
            do
            {
                Console.Clear();
                Console.WriteLine("View Orders:");
                Console.WriteLine("[0] Exit");
                Console.WriteLine("[1] View By customer");
                Console.WriteLine("[2] View By Location");
                var str = Console.ReadLine();
                switch (str)
                {
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
            try
            {
                var objecs = _services.GetAllCustomers().Cast<object>().ToList<object>();

                var ret = SelectFromList.Start(objecs);
                var customer = (Customer) ret;

                var inputss = true;
                var price = true;
                var asc = true;
                string str;
                do
                {
                    Console.Clear();
                    Console.WriteLine("[0] Sort By Price Ascending");
                    Console.WriteLine("[1] Sort By Price Descending");
                    str = Console.ReadLine();
                    switch (str)
                    {
                        case "0":
                            price = true;
                            asc = true;
                            inputss = false;
                            break;

                        case "1":
                            price = true;
                            asc = false;
                            inputss = false;
                            break;

                        default:
                            Console.WriteLine("Invalid entry.");
                            break;
                    }
                } while (inputss);

                var orderList = _services.GetOrders(customer, price, asc).Cast<object>().ToList<object>();
                ret = SelectFromList.Start(orderList);
                var o = (Order) ret;
                var items = o.Items;
                items.ForEach(d => Console.WriteLine(d.ToString()));
            }
            catch (NullReferenceException ex)
            {
                Console.WriteLine("Cancelled Selection");
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
            }
        }

        private void ViewByLocation()
        {
            try
            {
                var objecs = _services.GetAllLocations().Cast<object>().ToList<object>();

                var ret = SelectFromList.Start(objecs);
                var location = (Location) ret;

                var inputss = true;
                var price = true;
                var asc = true;
                string str;
                do
                {
                    Console.Clear();
                    Console.WriteLine("[0] Sort By Price Ascending");
                    Console.WriteLine("[1] Sort By Price Descending");
                    str = Console.ReadLine();
                    switch (str)
                    {
                        case "0":
                            asc = true;
                            inputss = false;
                            break;

                        case "1":
                            asc = false;
                            inputss = false;
                            break;

                        default:
                            Console.WriteLine("Invalid entry.");
                            break;
                    }
                } while (inputss);

                var orderList = _services.GetOrders(location, price, asc).Cast<object>().ToList<object>();
                ret = SelectFromList.Start(orderList);
                var o = (Order) ret;
                var items = o.Items;
                items.ForEach(d => Console.WriteLine(d.ToString()));
            }
            catch (NullReferenceException ex)
            {
                Console.WriteLine("Cancelled Selection");
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
            }
        }

        private List<Item> GetItems(Location loc)
        {
            var selectedItem = new List<Item>();
            string str;
            var cont = true;
            try
            {
                do
                {
                    Console.Clear();
                    var objectList = loc.Inventory.Cast<object>().ToList<object>();

                    var ret = SelectFromList.Start(objectList);
                    var itm = (Item) ret;
                    var p = itm.Product;
                    str = _validate.ValidationPrompt("Enter Quantity to Order:", ValidationService.ValidatePositiveInt);

                    selectedItem.Add(new Item(p, int.Parse(str)));
                    var innercont = true;
                    do
                    {
                        Console.Clear();
                        Console.WriteLine("[0] Order");
                        Console.WriteLine("[1] Add  item");
                        str = Console.ReadLine();
                        switch (str)
                        {
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
                    } while (innercont);
                } while (cont);
            }
            catch (NullReferenceException ex)
            {
                Console.WriteLine("Cancelled Item Selection");
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
            }

            return selectedItem;
        }

        private void CreateNewOrder()
        {
            var cust = GetCustomer();
            if (cust == null) return;
            var loc = GetLocation();
            if (loc == null) return;
            var itemss = GetItems(loc);
            if (itemss.Count == 0) return;
            var total = _services.CalculateOrderTotal(itemss);
            Console.WriteLine("The order Total is: {0}", total);
            Console.WriteLine("Press C to conforim \n");
            var key = Console.ReadKey();
            if (key.Key.ToString().ToLower() == "C")
                try
                {
                    _services.PlaceOrder(loc, cust, itemss);
                    Console.WriteLine("Order has been Placed!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
        }

        private Customer GetCustomer()
        {
            Customer cust = null;
            try
            {
                var objecs = _services.GetAllCustomers().Cast<object>().ToList<object>();

                var ret = SelectFromList.Start(objecs);
                cust = (Customer) ret;

                Console.Clear();
                Console.WriteLine("Customer selected: {0}", cust.ToString());
            }
            catch (NullReferenceException ex)
            {
                Console.WriteLine("Cancelled Selection!");
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
            }

            return cust;
        }

        private Location GetLocation()
        {
            Location loc = null;
            try
            {
                var objectList = _services.GetAllLocations().Cast<object>().ToList<object>();

                var ret = SelectFromList.Start(objectList);
                loc = (Location) ret;
                Console.Clear();
                Console.WriteLine("Location selected: {0}", loc.ToString());
            }
            catch (NullReferenceException ex)
            {
                Console.WriteLine("Cancelled Location Selection");
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
            }

            return loc;
        }
    }
}