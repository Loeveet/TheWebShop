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
        public virtual Customer Customer { get; set; }
        public virtual List<Product>? Products { get; set; }
        internal static void HandlingShoppingCart()
        {
            // Metod för att hantera varukorg
        }
    }
}
