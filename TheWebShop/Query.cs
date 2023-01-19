using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheWebShop.Models;

namespace TheWebShop
{
    internal class Query
    {
        internal static void HandleQueries()
        {
            var loop = true;
            while (loop)
            {
                Console.Clear();

                Console.WriteLine("[1] Bäst säljande produkt");
                Console.WriteLine("[2] Bäst säljande kategori");
                Console.WriteLine("[3] Antal produkter i lager per leverantör");
                Console.WriteLine("[4] Antal sålda produkter per kund");
                Console.WriteLine("[0] Backa");

                var input = Console.ReadKey(true).KeyChar;
                switch (input)
                {
                    case '1':
                        BestSellingProduct();
                        break;
                    case '2':
                        SellingCategory();
                        break;
                    case '3':
                        ProductsPerSupplier();
                        break;
                    case '4':
                        SoldProductsPerCustomer();
                        break;
                    case '0':
                        loop = false;
                        break;
                }
            }
        }

        private static void SoldProductsPerCustomer()
        {
            Console.Clear();
            var productsPerCustomer = ProductsPerCustomer.GetProductsPerCustomer();
            Console.WriteLine("Antal köpta produkter per kund");
            Console.WriteLine("-------------------------------");
            foreach (var p in productsPerCustomer)
            {
                Console.WriteLine($"{p.FirstName} {p.LastName} har köpt {p.Products} produkter");
            }

            Console.ReadKey();
        }

        private static void ProductsPerSupplier()
        {
            using var dbContext = new TheWebShopContext();
            Console.Clear();

            var supplier = dbContext.Products
                .Include(x => x.Supplier)
                .ToList()
                .GroupBy(x => x.Supplier)
                .OrderByDescending(x => x.Key.Products.Count());

            Console.WriteLine("Antal produkter i lager per leverantör");
            Console.WriteLine("-------------------------------------");
            foreach (var s in supplier)
            {
                int quantity = 0;
                foreach (var x in s)
                {
                    quantity+= x.Quantity;
                }
                Console.WriteLine($"{s.Key.Name} har {quantity} produkter i lager hos oss");
            }
            Console.ReadKey();
        }

        private static void SellingCategory()
        {
            Console.Clear();
            var bestCategories = BestSellingCategory.GetBestSellingCategories();
            Console.WriteLine("Bäst säljande kategorierna");
            Console.WriteLine("---------------------------");
            foreach (var c in bestCategories)
            {
                Console.WriteLine($"Produkter i kategorin {c.Name} har sålts {c.Quantity} gånger");
            }

            Console.ReadKey();
        }

        private static void BestSellingProduct()
        {
            using var dbContext = new TheWebShopContext();
            Console.Clear();

            var bestSellingProducts = dbContext.OrderDetails
                .Include(x => x.Product)
                .ToList()
                .GroupBy(x => x.Product)
                .OrderByDescending(x => x.Key.OrderDetails.Count());

            Console.WriteLine("De bäst säljande produkterna");
            Console.WriteLine("-----------------------------");
            foreach (var product in bestSellingProducts)
            {
                Console.WriteLine($"{product.Key.Name} har blivit sålt {product.Key.OrderDetails.Count()} gånger");
            }
            Console.ReadKey();
        }
    }
}
