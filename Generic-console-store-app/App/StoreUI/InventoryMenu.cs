using Serilog;
using Service;
using StoreModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UI
{
    public class InventoryMenu : IMenu
    {
        private IService _services;
        private IValidationUI _validate;

        private Location _location;

        public InventoryMenu(IService services, IValidationUI validate)
        {
            _services = services;
            _validate = validate;
        }

        public void Start()
        {
            //First Get A Location
            try
            {
                var objectList = _services.GetAllLocations().Cast<object>().ToList<object>();

                var ret = SelectFromList.Start(objectList);
                _location = (Location) ret;
            }
            catch (NullReferenceException ex)
            {
                Console.WriteLine("Cancelled Location Selection");

                return;
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return;
            }

            //Do actions with inventory
            var repeat = true;
            string str;
            do
            {
                Console.Clear();
                Console.WriteLine("Inventory Menu For Location:\n{0}", _location.ToString());
                Console.WriteLine("[0] Exit");
                Console.WriteLine("[1] View Inventory Of Location");
                Console.WriteLine("[2] Update Inventory");
                Console.WriteLine("[3] Add Product To Inventory");

                var input = Console.ReadLine();
                switch (input)
                {
                    case "0":
                        //Exit Menu
                        Console.WriteLine("Goodbye");
                        Log.Information("program exit from menu");
                        repeat = false;
                        break;

                    case "1":
                        try
                        {
                            Console.WriteLine("{0} Inventory:", _location.ToString());
                            foreach (var item in _location.Inventory) Console.WriteLine(item.ToString());
                            Console.WriteLine();
                        }
                        catch (Exception ex)
                        {
                            Log.Error("Error Viewing Location", ex.Message);
                        }

                        break;

                    case "2":
                        Item selectedItem;
                        try
                        {
                            var objectList = _location.Inventory.Cast<object>().ToList<object>();

                            var ret = SelectFromList.Start(objectList);
                            selectedItem = (Item) ret;
                        }
                        catch (NullReferenceException ex)
                        {
                            Console.WriteLine("Cancelled Item Selection");
                            break;
                        }
                        catch (Exception ex)
                        {
                            Log.Error(ex, ex.Message);
                            break;
                        }

                        //increase or decrease stock
                        str = _validate.ValidationPrompt("Enter Amount increase/decrease stock by:",
                            ValidationService.ValidateInt);

                        try
                        {
                            //try updating item quantity
                            _services.updateItemInStock(_location, selectedItem, int.Parse(str));
                            Console.WriteLine("Stock updated");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            Console.WriteLine("Stock Could Not Be Updated");
                        }

                        break;

                    case "3":
                        Product prod;
                        //Add New Product to Inventory
                        try
                        {
                            var objectList = _services.GetAllProducts().Cast<object>().ToList<object>();

                            var ret = SelectFromList.Start(objectList);
                            prod = (Product) ret;
                        }
                        catch (NullReferenceException ex)
                        {
                            Console.WriteLine("Cancelled Product Selection");
                            break;
                        }
                        catch (Exception ex)
                        {
                            Log.Error(ex, ex.Message);
                            break;
                        }

                        //Get Number for stock
                        str = _validate.ValidationPrompt("Enter Initial number of Products in Stock",
                            ValidationService.ValidatePositiveInt);
                        var stock = int.Parse(str);
                        // Add Product to Inventory
                        try
                        {
                            _services.AddProductToInventory(_location, prod, stock);
                            Console.WriteLine("Product Added");
                        }
                        catch (Exception ex)
                        {
                            Log.Warning(ex.Message);
                            Console.WriteLine(ex.Message);
                        }

                        break;

                    default:
                        Console.WriteLine("Choose valid option");
                        break;
                }
            } while (repeat);
        }
    }
}