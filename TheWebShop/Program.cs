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
            //Kolla hur det ligger till med metodnamn som är samma som klassnamn



            // Test för att se product plus kategori
            //using var db = new TheWebShopContext();
            //foreach (var p in db.Products.Include(c => c.Category))
            //{
            //    Console.WriteLine(p.Name + " - " + p.Category.Name);
            //}

            Managing.RunTheWebShop();
        }
    }
}