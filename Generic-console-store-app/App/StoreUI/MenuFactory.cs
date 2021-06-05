using Service;
using DataLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;
using Data.Entities;
using System.Net;

namespace StoreUI
{
    public class MenuFactory
    {

        public static IMenu GetMenu(string menuType)
        {
            
            var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

            string connectionString = configuration.GetConnectionString("p0running");

            DbContextOptions<p0runningContext> options = new DbContextOptionsBuilder<p0runningContext>()
            .UseSqlServer(connectionString)
            .Options;
            
            var context = new p0runningContext(options);

            switch(menuType.ToLower()){
                case "mainmenu":
                    return new MainMenu(new Services(new RepoDB(context)),  new ValidationUI());
                case "adminmenu":
                    return new AdminMenu(new Services(new RepoDB(context)),  new ValidationUI());
                case "inventorymenu":
                    return new InventoryMenu(new Services(new RepoDB(context)), new ValidationUI());
                default:
                    return null;
            }
        }
    }
}