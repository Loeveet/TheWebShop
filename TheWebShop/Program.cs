using Microsoft.EntityFrameworkCore;
using TheWebShop.Models;

namespace TheWebShop
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using var db = new TheWebShopContext();
            foreach (var p in db.Products.Include(c => c.Category))
            {
                Console.WriteLine(p.Name + " - " + p.Category.Name);
            }
            
            //Managing.RunTheWebShop();

        }
    }
}