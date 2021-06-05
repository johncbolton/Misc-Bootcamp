using System;
using Serilog;

namespace StoreUI
{
    class StoreUI
    {
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.File("log.txt", rollingInterval: RollingInterval.Month)
                .CreateLogger();

            Log.Verbose("Main Menu");
            try{
            MenuFactory.GetMenu("mainmenu").Start();
            }catch(Exception ex){
                Log.Error(ex.Message);
            }

            Log.CloseAndFlush();  

        }
    }
}
