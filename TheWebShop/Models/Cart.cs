using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheWebShop.Models
{
    internal class Cart
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Product Product { get; set; }
        internal static void PrintCart(Customer customer)
        {
            var (StartLeft, StartTop) = Console.GetCursorPosition();
            int left = 80;
            int top = 2;
            double totalCost = 0;
            using var dbContext = new TheWebShopContext();

            var myCart = dbContext.Carts
                .Where(x => x.CustomerId == customer.Id)
                .Include(x => x.Product)
                .ToList()
                .GroupBy(x => x.Product);

            Console.SetCursorPosition(left, top - 1);
            Console.WriteLine(customer.FirstName);
            Console.SetCursorPosition(left, top);
            Console.WriteLine("-----------------------------------------------------");
            foreach (var c in myCart)
            {
                totalCost += c.Key.Price * c.Count();
                top++;
                Console.SetCursorPosition(left, top);
                Console.WriteLine($"{c.Key.Name} à {c.Key.Price} kr - {c.Count()} st - Totalt per produkt {c.Key.Price * c.Count()} kr");
            }
            Console.SetCursorPosition(left, top + 1);
            Console.WriteLine("-----------------------------------------------------");
            Console.SetCursorPosition(left, top + 2);
            Console.WriteLine($"Totalpris: {totalCost} kr, varav moms: {totalCost * 0.25}");
            Console.SetCursorPosition(StartLeft, StartTop);
        }
    }
}
