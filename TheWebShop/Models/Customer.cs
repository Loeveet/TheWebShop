using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace TheWebShop.Models
{
    [Index(nameof(Email), IsUnique = true)]
    [Index(nameof(CreditCard), IsUnique = true)]
    [Index(nameof(PhoneNumber), IsUnique = true)]

    internal class Customer
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public int ZipCode { get; set; }
        public int CityId { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string CreditCard { get; set; }

        public virtual City City { get; set; }
        public virtual ICollection<Order>? Orders { get; set; }
        public virtual ICollection<Cart> Carts { get; set; }

        internal static void MenuCustomer()
        {
            using var dbContext = new TheWebShopContext();
            Console.Clear();
            Console.WriteLine("Sparade användare\n");
            foreach (var c in dbContext.Customers)
            {
                Console.WriteLine($"[{c.Id}] - {c.FirstName} {c.LastName} - {c.Email}");
            }
            Console.WriteLine("\n[G]äst");
            Console.WriteLine("[B]efintlig kund");
            Console.WriteLine("[N]y kund");
            Console.WriteLine();
            Console.WriteLine("Välj om du vill handla som gäst, använda befintlig kund eller skapa ny kund");
            var choice = Console.ReadKey(true).KeyChar;
            switch (choice)
            {
                case 'G':
                case 'g':
                    CustomerStartPage(new Customer { FirstName = "Gäst", Carts = new List<Cart>() }); // går vidare som gäst
                    break;
                case 'B':
                case 'b':
                    Console.WriteLine("Välj befintligt kundId"); // välj befintlig kund
                    int custId = Managing.TryToParseInput();
                    var customer = dbContext.Customers
                        .Where(c => c.Id == custId)
                        .FirstOrDefault();
                    CustomerStartPage(customer);
                    break;
                case 'N':
                case 'n':
                    var newCustomer = CreateNewCustomer(); // skapa nu kund                    
                    CustomerStartPage(newCustomer);
                    break;
                default:
                    break;
            }

        }
        private static Customer CreateNewCustomer()
        {
            using var dbContext = new TheWebShopContext();

            Console.Write("Ange förnamn: ");
            var firstName = Console.ReadLine();
            Console.Write("Ange efternamn: ");
            var lastName = Console.ReadLine();


            Console.WriteLine("Registrerade länder");
            foreach (var c in dbContext.Countries)
            {
                Console.WriteLine($"[{c.Id}] - {c.Name}");
            }
            Console.Write("Välj landsId från listan eller skriv land om du vill lägga till nytt land: ");
            var country = Console.ReadLine();
            int id;
            if (!int.TryParse(country, out id))
            {
                id = Managing.Create(new Country(), dbContext);
            }


            Console.WriteLine("Registrerade städer i valt land");
            foreach (var city in dbContext.Cities
                .Where(x => x.CountryId == id))
            {
                Console.WriteLine($"[{city.Id}] - {city.Name}");

            }
            Console.Write("Välj stadsId från listan eller skriv in ny stad: ");
            var cityId = Console.ReadLine();
            int cId;
            if (!int.TryParse(cityId, out cId))
            {
                cityId = Managing.Create(new City(), dbContext, id);
            }




            Console.Write("Ange gatuadress: ");
            var adress = Console.ReadLine();
            Console.Write("Ange postnummer: ");
            int zipCode = Convert.ToInt32(Console.ReadLine());
            Console.Write("Ange email: ");
            var email = Console.ReadLine();
            Console.Write("Ange telefonnummer: ");
            var phoneNumber = Console.ReadLine();
            Console.Write("Ange födelseår, fyra siffror: ");
            int birthYear = Convert.ToInt32(Console.ReadLine());
            Console.Write("Ange födelsemånad: ");
            int birthMonth = Convert.ToInt32(Console.ReadLine());
            Console.Write("Ange födelsedatum: ");
            int birthDay = Convert.ToInt32(Console.ReadLine());
            Console.Write("Ange kreditkortsnummer, tolv siffror: ");
            var creditCardNr = Console.ReadLine();

            Customer customer = new Customer
            {
                FirstName = firstName,
                LastName = lastName,
                CityId = Convert.ToInt32(cityId),
                Street = adress,
                ZipCode = zipCode,
                Email = email,
                PhoneNumber = phoneNumber,
                DateOfBirth = new DateTime(birthYear, birthMonth, birthDay),
                CreditCard = creditCardNr,
                //Carts = new Cart{ Products = new List<Product>()},
                Orders = new List<Order>()
            };
            dbContext.Add(customer);
            dbContext.SaveChanges();
            //customer.Carts.CustomerId = customer.Id;
            dbContext.SaveChanges();

            return customer;
        }
        private static void CustomerStartPage(Customer customer)
        {
            using var dbContext = new TheWebShopContext();
            var customerLoop = true;
            //var randomized = new List<Product>();
            while (customerLoop)
            {
                Console.Clear();
                Cart.PrintCart(customer);
                Console.WriteLine($"Välkommen {customer.FirstName} till Webbshoppen!\n");


                Console.WriteLine("Utvalda produkter:");
                var chosenProducts = dbContext.Products
                    .Where(x => x.ChosenProduct)
                    .ToList();

                if (chosenProducts.Count > 3)
                {
                    var products = new List<Product>(chosenProducts);
                    chosenProducts = new List<Product>();
                    chosenProducts.AddRange(Randomize(products)
                        .Take(3));

                    Console.WriteLine($"Id\tPris \t Namn");
                    foreach (var product in chosenProducts)
                    {
                        Console.WriteLine($"[{product.Id}]\t{product.Price} kr\t {product.Name} - {product.DetailedInfo}");
                    }
                }
                else
                {
                    Console.WriteLine($"Id\tPris \t Namn");
                    foreach (var product in chosenProducts)
                    {
                        Console.WriteLine($"[{product.Id}]\t{product.Price} kr\t {product.Name} - {product.DetailedInfo}");
                    }
                }

                Console.WriteLine();
                Console.WriteLine("[K]öp utvald produkt");
                Console.WriteLine("[F]ritextsök på produkter");
                Console.WriteLine("[S]hopsida");
                Console.WriteLine("[V]arukorg");
                Console.WriteLine("[B]acka");

                var choice = Console.ReadKey(true).KeyChar;
                switch (choice)
                {
                    case 'K':
                    case 'k':
                        Console.WriteLine("Välj Id på den produkten du är intresserad av");
                        var input = Console.ReadLine();
                        if (int.TryParse(input, out int id))
                        {
                            var product = chosenProducts
                                .Where(x => x.Id == id)
                                .FirstOrDefault();
                            if (product != null)
                            {
                                Product.ShowProduct(product, customer, dbContext);
                            }
                            else
                            {
                                Console.WriteLine("Vald produkt ingår ej bland de utvalda. Tryck valfri tangent för att fortsätta");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Felaktig inmating. Tryck valfri tangent för att fortsätta");
                        }
                        Console.ReadKey(true);
                        break;
                    case 'F':
                    case 'f':
                        Product.ShowSearchResults(customer);
                        break;
                    case 'S':
                    case 's':
                        Managing.ShoppingPage(customer);
                        break;
                    case 'V':
                    case 'v':
                        ShoppingCart(customer, dbContext);
                        break;
                    case 'B':
                    case 'b':
                        customerLoop = false;
                        break;
                    default:
                        break;
                }

            }
        }

        private static void ShoppingCart(Customer customer, TheWebShopContext dbContext)
        {
            var loop = true;
            while (loop)
            {
                Console.Clear();

                var test = GetShoppingCart(customer, dbContext);
                Console.WriteLine();
                Console.WriteLine("[1] Ändra antal");
                Console.WriteLine("[2] Ta bort produkten");
                Console.WriteLine("[3] Gå till kassan");
                Console.WriteLine("[0] Backa");
                Console.WriteLine();
                var choice = Console.ReadKey(true).KeyChar;
                switch (choice)
                {
                    case '1':

                        Console.WriteLine("Välj Id på den produkten du vill ändra antal på");
                        var id = Managing.TryToParseInput();

                        var products = dbContext.Carts
                            .Include(x => x.Product)
                            .Where(x => x.ProductId == id && x.CustomerId == customer.Id)
                            .ToList();

                        if (products.Any())
                        {
                            ChangeQuantity(customer, dbContext, id); //TODO håller på!
                        }
                        else
                        {
                            Console.WriteLine("Vald produkt ingår ej bland de utvalda. Tryck valfri tangent för att fortsätta");
                        }
                        Console.ReadKey(true);
                        break;
                    case '2':
                        break;
                    case '3':
                        GoToCheckout(customer, dbContext, test);
                        break;
                    case '0':
                        loop = false;
                        break;
                }
            }
        }

        private static void PrintReceipt(TheWebShopContext dbContext, int freightMethodId, IEnumerable<IGrouping<Product, Cart>> test)
        {
            double totalCost = 0;

            foreach (var op in test)
            {
                totalCost += op.Key.Price * op.Count();
                Console.WriteLine($"{op.Key.Name} à {op.Key.Price} kr, antal: {op.Count()} st. Pris: {op.Key.Price * op.Count()} kr");
            }
            var freightCost = dbContext.Freights.Where(x => x.Id == freightMethodId).FirstOrDefault();
            totalCost += freightCost.Price;
            Console.WriteLine($"+ leverans: {freightCost.Price} kr");
            Console.WriteLine($"Totalpris: {totalCost} kr");
        }

        private static void GoToCheckout(Customer customer, TheWebShopContext dbContext, IEnumerable<IGrouping<Product, Cart>> test)
        {
            double totalCost = 0;

            var loop = true;
            while (loop)
            {
                Console.Clear();
                Cart.PrintCart(customer);
                Console.WriteLine("Välj fraktmetod");
                foreach (var f in dbContext.Freights)
                {
                    Console.WriteLine($"[{f.Id}] - {f.Name} - {f.Price}kr");
                }
                int id = Managing.TryToParseInput();
                var freightMethodId = dbContext.Freights
                    .Where(x => x.Id == id)
                    .Select(x => x.Id)
                    .FirstOrDefault();
                Console.WriteLine("Välj betalmetod");
                foreach (var f in dbContext.PaymentMethods)
                {
                    Console.WriteLine($"[{f.Id}] - {f.Name}");
                }
                int id2 = Managing.TryToParseInput();
                Console.WriteLine();
                var paymentMethodId = dbContext.PaymentMethods
                    .Where(x => x.Id == id2)
                    .Select(x => x.Id)
                    .FirstOrDefault();

                Order order = new Order
                {
                    // TODO: customer.Id funkar ej om man är gäst
                    CustomerId = customer.Id,
                    FreightId = freightMethodId,
                    PaymentMethodId = paymentMethodId,
                    OrderDate = DateTime.Now
                };

                List<OrderProduct> orderProducts = new();
                var cartResult = new List<Cart>();


                foreach (var x in cartResult)
                {
                    orderProducts.Add(new OrderProduct
                    {
                        ProductId = x.ProductId,
                        UnitPrice = x.Product.Price

                    });
                }

                PrintReceipt(dbContext, freightMethodId, test);

                var freightCost = dbContext.Freights.Where(x => x.Id == freightMethodId).FirstOrDefault();

                Console.WriteLine();
                Console.WriteLine("Tryck [1] för att slutföra köpet");
                Console.WriteLine("Tryck [2] börja om utcheckningen");
                Console.WriteLine("Tryck [0] för att backa");
                var choice = Console.ReadKey(true).KeyChar;
                switch (choice)
                {
                    case '1':

                        dbContext.Orders.Add(order);
                        dbContext.SaveChanges();

                        var orderId2 = dbContext.Orders
                            .Where(x => x.CustomerId == customer.Id && x.OrderDate == order.OrderDate)
                            .Select(x => x.Id)
                            .FirstOrDefault();

                        foreach (var x in orderProducts)
                        {
                            x.OrderId = orderId2;
                            dbContext.OrderDetails.Add(x);
                        }
                        dbContext.SaveChanges();

                        foreach (var x in dbContext.Carts.Where(x => x.CustomerId == customer.Id))
                        {
                            dbContext.Carts.Remove(x);
                        }
                        dbContext.SaveChanges();

                        Console.Clear();

                        Console.WriteLine("Här är ditt kvitto!");
                        Console.WriteLine("-----------------------------------------------------");
                        PrintReceipt(dbContext, freightMethodId, test);
                        Console.WriteLine("-----------------------------------------------------");
                        Console.WriteLine();
                        Console.WriteLine("Tack för din beställning!");
                        Console.WriteLine("Tryck valfri tangent för att gå tillbaka till menyn");

                        Console.ReadKey();
                        loop = false;

                        break;
                    case '2':
                        break;
                    case '0':
                        loop = false;
                        break;
                }
            }
        }

        private static IEnumerable<IGrouping<Product, Cart>> GetShoppingCart(Customer customer, TheWebShopContext dbContext)
        {
            double totalCost = 0;
            var result = new List<Cart>();
            if (customer.FirstName != "Gäst")
            {
                result = dbContext.Carts
                .Where(x => x.CustomerId == customer.Id)
                .Include(x => x.Product)
                .ToList();
            }
            else
            {
                result = customer.Carts.ToList();
            }
            var myCart = result
                .GroupBy(x => x.Product);

            Console.WriteLine(customer.FirstName);
            Console.WriteLine("-----------------------------------------------------");
            foreach (var c in myCart)
            {
                totalCost += c.Key.Price * c.Count();
                Console.WriteLine($"[{c.Key.Id}] - {c.Key.Name} à {c.Key.Price} kr - {c.Count()} st - Totalt per produkt {c.Key.Price * c.Count()} kr");
            }
            Console.WriteLine("-----------------------------------------------------");
            Console.WriteLine($"Totalt {totalCost} kr");
            return myCart;
        }

        private static void ChangeQuantity(Customer customer, TheWebShopContext dbContext, int productId)
        {
            var loop = true;
            while (loop)
            {
                Console.Clear();
                var customerProduct = dbContext.Carts
                            .Where(x => x.CustomerId == customer.Id && x.ProductId == productId)
                            .Include(x => x.Product)
                            .ToList();

                foreach (var product in customerProduct.Distinct())
                {
                    Console.WriteLine(product.Product.Name + " - " + customerProduct.Count());
                    break;
                }
                

                Console.WriteLine();
                Console.WriteLine("Lägg till produkter med uppåtpil");
                Console.WriteLine("Ta bort produkter med nedåtpil");
                Console.WriteLine("[0] Backa");


                ConsoleKeyInfo key = Console.ReadKey();
                if (customerProduct.Any())
                {
                    switch (key.Key)
                    {
                        case ConsoleKey.UpArrow:
                            dbContext.Carts.Add(new Cart { ProductId = customerProduct[0].ProductId, CustomerId = customer.Id });
                            customerProduct[0].Product.Quantity--;
                            dbContext.SaveChanges();

                            break;
                        case ConsoleKey.DownArrow:
                            dbContext.Carts.Remove(customerProduct[0]);
                            customerProduct[0].Product.Quantity++;
                            dbContext.SaveChanges();
                            break;
                        case ConsoleKey.D0:
                            loop = false;
                            break;
                    }
                }
                else if (key.Key == ConsoleKey.D0)
                {
                    loop = false;
                }
            }

        }

        private static List<Product> Randomize(List<Product> products)
        {
            var random = new Random();
            var randomized = products.OrderBy(x => random.Next()).ToList();
            return randomized;
        }
    }
}
