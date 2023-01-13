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
        internal static void HandlingShoppingCart()
        {
            // Metod för att hantera varukorg
        }
        internal static void PrintCart(Customer customer)
        {
            var n = Console.GetCursorPosition();
            int top = 2;
            int left = 110;
            double totalCost = 0;
            using var dbContext = new TheWebShopContext();
            var result = dbContext.Carts
                .Where(x => x.CustomerId == customer.Id)
                .Include(x => x.Product)
                .ToList();

            var myCart = result
                .GroupBy(x => x.Product);

            Console.SetCursorPosition(left, top);
            Console.WriteLine(customer.FirstName);
            foreach (var c in myCart)
            {
                totalCost += (c.Key.Price * c.Count());
                top++;
                Console.SetCursorPosition(left, top);
                Console.WriteLine($"{c.Key.Name} à {c.Key.Price} kr - {c.Count()} st - Totalt per produkt {totalCost} kr");
            }         
            Console.SetCursorPosition(left, top+1);
            Console.WriteLine($"Totalt totalt {totalCost} kr");
            Console.SetCursorPosition(n.Left, n.Top);
        }
    }
}
