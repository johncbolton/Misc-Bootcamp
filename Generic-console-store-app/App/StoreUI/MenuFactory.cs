using Data;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Service;
using System.IO;

namespace UI
{
    public class MenuFactory
    {
        public static IMenu GetMenu(string menuType)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("P0dbrun");

            var options = new DbContextOptionsBuilder<P0dbrunContext>()
                .UseSqlServer(connectionString)
                .Options;

            var context = new P0dbrunContext(options);

            switch (menuType.ToLower())
            {
                case "mainmenu":
                    return new MainMenu(new Services(new RepoDB(context)), new ValidationUI());

                case "adminmenu":
                    return new AdminMenu(new Services(new RepoDB(context)), new ValidationUI());

                case "inventorymenu":
                    return new InventoryMenu(new Services(new RepoDB(context)), new ValidationUI());

                default:
                    return null;
            }
        }
    }
}