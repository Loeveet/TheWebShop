using Microsoft.EntityFrameworkCore;
using TheWebShop.Models;

namespace TheWebShop
{
    internal class Program
    {
        static void Main(string[] args)
        {

            //Kunna lägga in categories och suppliers när man lägger in ny product
            //Ev kunna lägga in nytt country när man lägger till city
            //Göra metoder för allt
            //Vi har lagt till cart, men ännu inte testat den. 
            //Se över metod/variabelnamn


            //using var dbContext = new TheWebShopContext();

            //foreach (var x in dbContext.Carts.Include(x => x.Products).Include(x => x.Customer))
            //{
            //    Console.WriteLine(x.Customer.FirstName);
            //    foreach (var y in x.Products)
            //    {
            //        Console.WriteLine("\t" + y.Name);
            //    }
            //}
            //Console.WriteLine();

            Managing.RunTheWebShop();
        }
    }
}