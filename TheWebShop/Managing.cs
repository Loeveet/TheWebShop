using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheWebShop
{
    internal class Managing
    {
        public static void RunTheWebShop()
        {
            var loop = true;
            while (loop)
            {
                Console.Clear();

                Console.WriteLine("[A]dmin");
                Console.WriteLine("[C]ustomer");
                Console.WriteLine("[L]eave TheWebShop");

                var choice = Console.ReadKey(true).KeyChar;
                switch (choice)
                {
                    case 'A':
                    case 'a':
                        // Go to admin-page
                        break;
                    case 'C':
                    case 'c':
                        // Go to customer-page
                        break;
                    case 'L':
                    case 'l':
                        loop = false;
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
