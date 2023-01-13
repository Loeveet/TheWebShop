using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
        //public virtual Cart Cart { get; set; }
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
                    CustomerStartPage(null); // går vidare som gäst
                    break;
                case 'B':
                case 'b':
                    Console.WriteLine("Välj befintligt kundId"); // välj befintlig kund
                    int custId = Convert.ToInt32(Console.ReadLine());
                    var customer = dbContext.Customers                       
                        .Where(c => c.Id == custId)
                        .FirstOrDefault();              //Todo En kontroll så att det finns en kund på det Id.
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
            Console.Write("Ange landsId: ");
            int countryId = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Registrerade städer i valt land");
            foreach (var city in dbContext.Cities
                .Where(x => x.CountryId == countryId))
            {
                Console.WriteLine($"[{city.Id}] - {city.Name}");

            }
            Console.Write("Ange stadsId: ");
            int cityId = Convert.ToInt32(Console.ReadLine());
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
                CityId = cityId,
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
                Console.WriteLine($"Välkommen {(customer == null ? "gäst" : customer.FirstName)} till Webbshoppen!\n");


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
                        //ShoppingCart();
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
        private static List<Product> Randomize(List<Product> products)
        {
            var random = new Random();
            var randomized = products.OrderBy(x => random.Next()).ToList();
            return randomized;
        }
    }
}
