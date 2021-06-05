using System;
using Service;
using Serilog;
using StoreModels;
using System.Collections.Generic;
using System.Linq;

namespace StoreUI
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

            try{ 
                List<Object> objectList = _services.GetAllLocations().Cast<Object>().ToList<Object>();         
                Object ret = SelectFromList.Start(objectList);
                _location = (Location) ret;
                
            }catch(NullReferenceException ex){
 
                Console.WriteLine("Cancelled Location Selection");

                return;
            }catch(Exception ex){
                Log.Error(ex, ex.Message);
                return;
            }

            bool repeat = true;
            string str;
            do{
                Console.Clear();
                Console.WriteLine("Inventory Menu For Location:\n{0}",_location.ToString());
                Console.WriteLine("[0] Exit");
                Console.WriteLine("[1] View Inventory Of Location");
                Console.WriteLine("[2] Update Inventory");
                Console.WriteLine("[3] Add Product To Inventory");

                
                string input = Console.ReadLine();
                switch (input)
                {
                    case "0":
                        Console.WriteLine("Goodbye");
                        Log.Information("program exit from menu");
                        repeat = false;
                    break;
                    case "1":
                        try{
                            Console.WriteLine("{0} Inventory:", _location.ToString());
                            foreach (Item item in _location.Inventory)
                            {
                                Console.WriteLine(item.ToString());
                            }
                            Console.WriteLine();

                        }catch(Exception ex){
                            Log.Error("Error Viewing Location", ex.Message);
                        }
                    break;
                    case "2":
                        Item selectedItem;
                        try{ 
                            List<Object> objectList = _location.Inventory.Cast<Object>().ToList<Object>();
                            
                            Object ret = SelectFromList.Start(objectList);
                            selectedItem = (Item) ret;
                            
                        }catch(NullReferenceException ex){
                            Console.WriteLine("Cancelled Item Selection");
                            break;
                        }catch(Exception ex){
                            Log.Error(ex, ex.Message);
                            break;
                        }
                        str = _validate.ValidationPrompt("Enter Amount increase/decrease stock by:", ValidationService.ValidateInt);
                        
                        try{

                            _services.updateItemInStock(_location, selectedItem, int.Parse(str));
                            Console.WriteLine("Stock updated");
                        }catch(Exception ex){

                            Console.WriteLine("Stock Could Not Be Updated");
                        }

                    break;
                    case "3":
                        Product prod;
                        try{ 
                            List<Object> objectList = _services.GetAllProducts().Cast<Object>().ToList<Object>();
                            
                            Object ret = SelectFromList.Start(objectList);
                            prod = (Product) ret;
                            
                        }catch(NullReferenceException ex){
                            Console.WriteLine("Cancelled Product Selection");
                            break;
                        }catch(Exception ex){
                            Log.Error(ex, ex.Message);
                            break;
                        }

                        str = _validate.ValidationPrompt("Enter Initial number of products in the stores stock", ValidationService.ValidatePositiveInt);
                        int stock = int.Parse(str);

                        try{
                            _services.AddProductToInventory(_location,prod, stock);
                            Console.WriteLine("Product Add");
                        }catch(Exception ex){
                            Console.WriteLine(ex.Message);
                        }
                    break;
                    default:
                        Console.WriteLine("Choose valid option!");
                    break;

                }
            } while(repeat);
        }
    }
}