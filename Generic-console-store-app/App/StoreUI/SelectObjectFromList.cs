using Service;
using System;
using System.Collections.Generic;

namespace UI
{
    public static class SelectFromList
    {
        public static object Start(List<object> objects)
        {
            string input;
            do
            {
                Console.Clear();
                Console.WriteLine("Choose From List:");
                Console.WriteLine("[0] Cancel Selection");
                var i = 1;
                foreach (var item in objects) Console.WriteLine("[{0}] {1}", i++, item.ToString());
                Console.WriteLine("Enter Your Selection:");
                input = Console.ReadLine();
                if (input == "0") throw new NullReferenceException("Selection Canceled");
            } while (!ValidationService.ValidateIntWithinRange(input, 1, objects.Count));

            var selection = int.Parse(input);
            var retVal = objects[selection - 1];
            return retVal;
        }
    }
}