using Serilog;
using Service;
using System;

namespace UI
{
    public class AdminMenu : IMenu
    {
        private IService _services;
        private IValidationUI _validate;

        public AdminMenu(IService services, IValidationUI validate)
        {
            _services = services;
            _validate = validate;
        }

        public void Start()
        {
            var repeat = true;
            do
            {
                Console.Clear();
                Console.WriteLine("Admin Menu:");
                Console.WriteLine("[0] Back");
                Console.WriteLine("[1] Add Customer");
                Console.WriteLine("[2] Add Location");
                Console.WriteLine("[3] Add Product");
                Console.WriteLine("[4] Add Inventory ");

                var input = Console.ReadLine();
                switch (input)
                {
                    case "0":
                        Console.WriteLine("Goodbye");
                        Log.Information("program exit from Admin menu");
                        repeat = false;
                        break;

                    case "1":

                        var name = _validate.ValidationPrompt("Enter First and Last Name:",
                            ValidationService.ValidatePersonName);
                        var address = _validate.ValidationPrompt("Enter Customer Address:",
                            ValidationService.ValidateAddress);
                        var email = _validate.ValidationPrompt("Enter Email For customer",
                            ValidationService.ValidateEmail);
                        try
                        {
                            _services.AddCustomer(name, address, email);
                            Console.WriteLine("Customer Added");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                        break;

                    case "2":

                        var locationName =
                            _validate.ValidationPrompt("Enter Location Name:", ValidationService.ValidateString);
                        Console.Clear();
                        var locationAddress = _validate.ValidationPrompt("Enter Location Address:",
                            ValidationService.ValidateAddress);
                        Console.Clear();
                        try
                        {
                            _services.AddLocation(locationName, locationAddress);
                            Console.WriteLine("Location added");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                        break;

                    case "3":
                        var productName =
                            _validate.ValidationPrompt("Enter Product Name:", ValidationService.ValidateString);
                        var productPrice = _validate.ValidationPrompt(
                            string.Format("Enter price for {0}:", productName), ValidationService.ValidateDouble);
                        try
                        {
                            _services.AddProduct(productName, Convert.ToDouble(productPrice));
                            Console.WriteLine("Product added");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                        break;

                    case "4":

                        MenuFactory.GetMenu("inventorymenu").Start();

                        break;

                    case "5":

                        break;

                    default:
                        Console.WriteLine("Choose valid option");
                        break;
                }
            } while (repeat);
        }
    }
}