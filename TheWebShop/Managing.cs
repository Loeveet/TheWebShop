using Microsoft.EntityFrameworkCore;
using TheWebShop.Models;

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

                Console.WriteLine("Välkommen till Webbshoppen!\n" +
                    "Var vänlig välj om du är kund eller admin\n");

                Console.WriteLine("[A]dmin");
                Console.WriteLine("[K]under");
                Console.WriteLine("[L]ämna TheWebShop");

                var choice = Console.ReadKey(true).KeyChar;
                switch (choice)
                {
                    case 'A':
                    case 'a':
                        MenuAdmin();
                        break;
                    case 'K':
                    case 'k':
                        Customer.MenuCustomer();
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
        internal static void ShoppingPage(Customer customer)
        {
            using var dbContext = new TheWebShopContext();
            var shoppingLoop = true;
            while (shoppingLoop)
            {
                Console.Clear();
                Cart.PrintCart(customer);
                Console.WriteLine("Kategorier");
                foreach (var c in dbContext.Categories
                    .Include(x => x.Products))
                {
                    Console.WriteLine($"[{c.Id}] {c.Name}");
                }
                Console.WriteLine();
                Console.Write("Ange Id för kategori för att se produkter eller [0] att backa menyn: ");
                var search = Console.ReadLine();
                int categoryId;
                if (int.TryParse(search, out categoryId))
                {
                    var products = dbContext.Products
                            .Where(x => x.Category.Id == categoryId);
                    if (products.Any())
                    {
                        var category = dbContext.Categories
                            .Where(x => x.Id == categoryId)
                            .SingleOrDefault();
                        var showProductLoop = true;
                        while (showProductLoop)
                        {
                            Console.Clear();
                            Console.WriteLine(category.Name);
                            foreach (var p in products)
                            {
                                Console.WriteLine($"Id [{p.Id}] - {p.Name} - {p.Price} kr");
                            }
                            Console.WriteLine();
                            Console.WriteLine("Välj Id för att läsa mer om produkten, eller ange [0] för att backa");
                            var buyLoop = true;
                            while (buyLoop)
                            {
                                Cart.PrintCart(customer);
                                var input = Console.ReadLine();
                                int productId = -1;
                                while (!int.TryParse(input, out productId))
                                {
                                    Console.WriteLine("Felaktig inmatning, försök igen");
                                    input = Console.ReadLine();
                                }
                                var product = dbContext.Products
                                    .Where(x => x.Id == productId && x.Category.Id == categoryId)
                                    //.Where(x => x.Id == productId && x.CategoryId == categoryId)
                                    .FirstOrDefault();
                                if (productId == 0)
                                {
                                    buyLoop = false;
                                    showProductLoop = false;
                                }
                                else if (product is null)
                                {
                                    Console.WriteLine("Vald produkt finns ej, försök igen");
                                }
                                else
                                {
                                    Product.ShowProduct(product, customer, dbContext);
                                    buyLoop = false;
                                }
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Felaktig inmatning, försök igen");
                }
                if (categoryId == 0)
                {
                    shoppingLoop = false;
                }

            }
        }

        public static void MenuAdmin()
        {
            var adminLoop = true;
            while (adminLoop)
            {
                Console.Clear();
                Console.WriteLine("[1] Hantera produkter, kategorier och leverantörer");
                Console.WriteLine("[2] Hantera kunder");
                Console.WriteLine("[3] Hantera betalmetoder");
                Console.WriteLine("[4] Hantera fraktmetoder");
                Console.WriteLine("[5] Hantera ordrar");
                Console.WriteLine("[6] Hantera länder och städer");
                Console.WriteLine("[0] Backa meny");

                var choice = Console.ReadKey(true).KeyChar;
                switch (choice)
                {
                    case '1':
                        MenuProductCategorySupplier();
                        break;
                    case '2':
                        // Customer
                        break;
                    case '3':
                        PaymentMethod.HandlingPaymentMethod();
                        break;
                    case '4':
                        Freight.HandlingFreightMethod();
                        break;
                    case '5':
                        // Order
                        break;
                    case '6':
                        MenuCountryCity();
                        break;
                    case '0':
                        adminLoop = false;
                        break;
                    default:
                        break;
                }
            }
        }
        private static void MenuProductCategorySupplier()
        {
            var loop = true;
            while (loop)
            {
                Console.Clear();
                Console.WriteLine("[1] Hantera produkter");
                Console.WriteLine("[2] Hantera kategorier");
                Console.WriteLine("[3] Hantera leverantörer");
                Console.WriteLine("[0] Backa meny");

                var choice = Console.ReadKey(true).KeyChar;
                switch (choice)
                {
                    case '1':
                        Product.HandlingProduct();
                        break;
                    case '2':
                        Category.HandlingCategory();
                        break;
                    case '3':
                        Supplier.HandlingSupplier();
                        break;
                    case '0':
                        loop = false;
                        break;
                    default:
                        break;
                }
            }
        }
        private static void MenuCountryCity()
        {
            var locationLoop = true;
            while (locationLoop)
            {
                Console.Clear();
                Console.WriteLine("[1] Hantera länder");
                Console.WriteLine("[2] Hantera städer");
                Console.WriteLine("[0] Backa meny");

                var choice = Console.ReadKey(true).KeyChar;
                switch (choice)
                {
                    case '1':
                        Country.HandlingCountry();
                        break;
                    case '2':
                        City.HandlingCity();
                        break;
                    case '0':
                        locationLoop = false;
                        break;
                    default:
                        break;
                }
            }
        }

        internal static void Create(PaymentMethod paymentMethod, TheWebShopContext dbContext)
        {
            Console.WriteLine("Ange namn på ny betalmetod");
            var name = Console.ReadLine();
            dbContext.Add(new PaymentMethod { Name = name });
            dbContext.SaveChanges();
        }
        internal static void Create(Country country, TheWebShopContext dbContext)
        {
            Console.WriteLine("Ange namn på nytt land");
            var name = Console.ReadLine();
            dbContext.Add(new Country { Name = name });
            dbContext.SaveChanges();
        }
        internal static void Create(Category category, TheWebShopContext dbContext)
        {
            Console.WriteLine("Ange namn på ny kategori");
            var name = Console.ReadLine();
            dbContext.Add(new Category { Name = name });
            dbContext.SaveChanges();
        }
    }
}