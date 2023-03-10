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
                Console.WriteLine("[K]und");
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
            var loop = true;
            while (loop)
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
                int categoryId = TryToParseInput();
                var products = dbContext.Products
                        .Where(x => x.Category.Id == categoryId);
                if (categoryId == 0)
                {
                    loop = false; return;
                }
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
                        Console.Write("Välj Id för att läsa mer om produkten, eller ange [0] för att backa: ");
                        var buyLoop = true;
                        while (buyLoop)
                        {
                            Cart.PrintCart(customer);
                            int productId = TryToParseInput();

                            var product = dbContext.Products
                                .Where(x => x.Id == productId && x.Category.Id == categoryId)
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
                else
                {
                    Console.WriteLine("Det finns ingen kategori på valt Id. Tryck valfri tangent för att fortsätta");
                    Console.ReadKey(true);
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
                Console.WriteLine("[5] Hantera länder och städer");
                Console.WriteLine("[6] Visa statistik");
                Console.WriteLine("[0] Backa meny");

                var choice = Console.ReadKey(true).KeyChar;
                switch (choice)
                {
                    case '1':
                        MenuProductCategorySupplier();
                        break;
                    case '2':
                        HandlingCustomers();
                        break;
                    case '3':
                        PaymentMethod.HandlingPaymentMethod();
                        break;
                    case '4':
                        Freight.HandlingFreightMethod();
                        break;
                    case '5':
                        MenuCountryCity();
                        break;
                    case '6':
                        Query.HandleQueries();
                        break;
                    case '0':
                        adminLoop = false;
                        break;
                    default:
                        break;
                }
            }
        }



        private static void HandlingCustomers()
        {
            using var dbContext = new TheWebShopContext();
            var loop = true;
            while (loop)
            {
                Console.Clear();
                Console.WriteLine("[1] Visa alla registrerade kunder");
                Console.WriteLine("[2] Sök efter en kund");
                Console.WriteLine("[0] För att backa");
                var input = Console.ReadKey(true).KeyChar;
                switch (input)
                {
                    case '1':
                        Console.Clear();
                        Console.WriteLine($"Id\tNamn\t\t\tMail");
                        Console.WriteLine("-------------------------------------");
                        foreach (var customer1 in dbContext.Customers)
                        {
                            Console.WriteLine($"[{customer1.Id}]\t{customer1.FirstName + ' ' + customer1.LastName}\t\t{customer1.Email}");
                        }
                        Console.WriteLine();
                        break;
                    case '2':
                        Console.Clear();
                        Console.WriteLine("Fritextsökning för kunder");

                        var input2 = Console.ReadLine();
                        var show = dbContext.Customers
                        .Include(x => x.City)
                        .Where(x => x.FirstName.Contains(input2) || x.LastName.Contains(input2) || x.Email.Contains(input2) || x.City.Name.Contains(input2) || x.City.Country.Name.Contains(input2));

                        Console.WriteLine();
                        if (show.Any())
                        {
                            Console.WriteLine($"Id\tNamn\t\t\tMail");
                            Console.WriteLine("------------------------------------");
                            foreach (var customer1 in show)
                            {
                                Console.WriteLine($"[{customer1.Id}]\t{customer1.FirstName + ' ' + customer1.LastName}\t\t{customer1.Email}");
                            }
                            Console.WriteLine();
                        }
                        else
                        {
                            Console.WriteLine("Sökningen gav ingen träff. Tryck valfri tangent för att fortsätta");
                            Console.ReadKey(true);
                            continue;

                        }
                        break;
                    case '0':
                        return;
                }
                var customer = new Customer();
                Console.WriteLine("Ange Id på kunden du vill administrera eller [0] för att backa");
                var inputId = TryToParseInput();

                if (inputId is 0)
                {
                    continue;
                }
                else if (dbContext.Customers.Any(x => x.Id == inputId))
                {
                    customer = dbContext.Customers
                        .Include(x => x.City)
                        .Where(x => x.Id == inputId)
                        .FirstOrDefault();
                }
                else
                {
                    Console.WriteLine("Fanns ingen kund på valt id. Tryck valfri tangent för att fortsätta");
                    Console.ReadKey(true);
                    continue;
                }
                var loop2 = true;
                while (loop2)//
                {
                    Console.Clear();
                    Console.WriteLine($"{customer.FirstName} {customer.LastName}");
                    Console.WriteLine();
                    Console.WriteLine("[1] Ändra kunduppgifter");
                    Console.WriteLine("[2] Se vald kunds beställningshistorik");
                    Console.WriteLine("[0] Backa meny");

                    var choice = Console.ReadKey(true).KeyChar;
                    switch (choice)
                    {
                        case '1':

                            var customLoop = true;

                            var customerCountry = dbContext.Cities
                                .Include(x => x.Country)
                                .Where(x => x.Id == customer.City.Id)
                                .FirstOrDefault();
                            while (customLoop)
                            {
                                Console.Clear();
                                Console.WriteLine($"Ange vad du vill ändra på hos {customer.FirstName} {customer.LastName}");
                                Console.WriteLine($"[1] Förnamn - {customer.FirstName}");
                                Console.WriteLine($"[2] Efternamn - {customer.LastName}");
                                Console.WriteLine($"[3] Gata - {customer.Street}");
                                Console.WriteLine($"[4] Postnummer - {customer.ZipCode}");
                                Console.WriteLine($"[5] Stad och land - {customer.City.Name}, {customerCountry.Country.Name}");
                                Console.WriteLine($"[6] Mail - {customer.Email}");
                                Console.WriteLine($"[7] Telefon - {customer.PhoneNumber}");
                                Console.WriteLine($"[8] Kreditkortsnummer - {customer.CreditCard}");
                                Console.WriteLine("[0] Backa");


                                var answer2 = Console.ReadKey(true).KeyChar;
                                switch (answer2)
                                {
                                    case '1':
                                        Console.Write("Ange nytt förnamn: ");
                                        customer.FirstName = Console.ReadLine();
                                        break;
                                    case '2':
                                        Console.Write("Ange nytt efternamn: ");
                                        customer.LastName = Console.ReadLine();
                                        break;
                                    case '3':
                                        Console.Write("Ange en ny gata: ");
                                        customer.Street = Console.ReadLine();
                                        break;
                                    case '4':
                                        Console.Write("Ange ett nytt postnummer: ");
                                        customer.ZipCode = TryToParseInput();
                                        break;
                                    case '5':
                                        Console.WriteLine("Registrerade länder");
                                        foreach (var c in dbContext.Countries)
                                        {
                                            Console.WriteLine($"[{c.Id}] - {c.Name}");
                                        }
                                        Console.Write("Välj [Id] eller skriv \"ny\": ");
                                        var country = Console.ReadLine();
                                        int id;
                                        if (!int.TryParse(country, out id))
                                        {
                                            id = Create(new Country(), dbContext);
                                        }

                                        Console.WriteLine("Registrerade städer i valt land");
                                        foreach (var city in dbContext.Cities
                                            .Where(x => x.CountryId == id))
                                        {
                                            Console.WriteLine($"[{city.Id}] - {city.Name}");

                                        }
                                        Console.Write("Välj [Id] eller skriv \"ny\": ");
                                        var cityId = Console.ReadLine();
                                        int cId;
                                        if (!int.TryParse(cityId, out cId))
                                        {
                                            cityId = Create(new City(), dbContext, id);
                                            customer.CityId = Convert.ToInt32(cityId);
                                        }
                                        else
                                        {
                                            customer.CityId = cId;
                                        }
                                        break;

                                    case '6':
                                        Console.Write("Ange en ny mailadress: ");
                                        customer.Email = Console.ReadLine();
                                        break;
                                    case '7':
                                        Console.Write("Ange ett nytt telefonnummer: ");
                                        customer.PhoneNumber = Console.ReadLine();
                                        break;
                                    case '8':
                                        Console.Write("Ange ett nytt kreditkortsnummer: ");
                                        customer.CreditCard = Console.ReadLine();
                                        break;
                                    case '0':
                                        customLoop = false;
                                        break;
                                }
                                dbContext.SaveChanges();
                            }

                            break;
                        case '2':

                            var orders = dbContext.Orders
                                .Include(x => x.Customer)
                                .Include(x => x.OrderProducts)
                                .Include(x => x.Freight)
                                .Include(x => x.PaymentMethod)
                                .Where(x => x.CustomerId == customer.Id)
                                .ToList();
                            var checkOrderLoop = true;
                            while (checkOrderLoop)
                            {
                                Console.Clear();

                                foreach (var order in orders)
                                {
                                    Console.WriteLine($"Order: [{order.Id}]");

                                }
                                Console.WriteLine("Ange id på order du vill gå in på eller [0] för att backa");
                                var orderId = TryToParseInput();
                                if (orderId is 0)
                                {
                                    checkOrderLoop = false;
                                    break;
                                }
                                else if (orders.Where(x => x.Id == orderId).Any())
                                {
                                    Console.Clear();
                                    var orderHistories = OrderHistory.GetOrderHistories(orderId, customer.Id);
                                    Console.WriteLine($"{customer.FirstName} {customer.LastName} - Order: {orderId}");
                                    Console.WriteLine();
                                    Console.WriteLine($"\tLagd order\t\tProduktnamn\tAntal\tStyckpris\tTotalpris");
                                    Console.WriteLine("\t--------------------------------------------------------------------------");
                                    foreach (var item in orderHistories)
                                    {
                                        Console.WriteLine($"\t{item.OrderDate}\t{item.ProductName}\t{item.Quantity} st\t{item.UnitPrice} kr/st\tTotalt: {item.TotalPrice} kr");

                                    }
                                    Console.ReadKey();
                                }
                            }

                            break;
                        case '0':
                            loop2 = false;
                            break;

                    }
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

        internal static int Create(Country country, TheWebShopContext dbContext)
        {
            Console.WriteLine("Ange namn på nytt land");
            country.Name = Console.ReadLine();
            dbContext.Add(country);
            dbContext.SaveChanges();
            var countryId = dbContext.Countries
                .Where(x => x.Id == country.Id)
                .Select(x => x.Id)
                .FirstOrDefault();
            Console.WriteLine("Vill du även lägga till en stad i landet? [J/N]");
            var answer = Console.ReadKey(true).KeyChar;
            switch (answer)
            {
                case 'J':
                case 'j':
                    Create(new City(), dbContext, countryId);
                    break;
                default:
                    Console.WriteLine("Du valde att inte lägga till en stad");
                    break;
            }
            return countryId;
        }

        internal static string Create(City city, TheWebShopContext dbContext, int countryId)
        {
            city.CountryId = countryId;
            Console.WriteLine("Ange namn på ny stad");
            city.Name = Console.ReadLine();
            dbContext.Add(city);
            dbContext.SaveChanges();

            return city.Id.ToString();
        }

        public static int TryToParseInput()
        {
            var input = Console.ReadLine();
            int id;
            while (!int.TryParse(input, out id))
            {
                Console.WriteLine("Felaktig inmatning, försök igen");
                input = Console.ReadLine();
            }
            return id;
        }
    }
}