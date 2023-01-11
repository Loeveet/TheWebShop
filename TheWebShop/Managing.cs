using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
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
                        Admin();
                        break;
                    case 'K':
                    case 'k':
                        Customer();
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

        private static void Customer()
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
                    CustomerStartPage(); // går vidare som gäst
                    break;
                case 'B':
                case 'b':
                    Console.WriteLine("Välj befintligt kundId"); // välj befintlig kund
                    int custId = Convert.ToInt32(Console.ReadLine());
                    CustomerStartPage();
                    break;
                case 'N':
                case 'n':
                    CreateNewCustomer(); // skapa nu kund
                    CustomerStartPage();
                    break;
                default:
                    break;
            }

        }

        private static void CreateNewCustomer()
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
                CityId= cityId,
                Street = adress,
                ZipCode = zipCode,
                Email = email,
                PhoneNumber = phoneNumber,
                DateOfBirth = new DateTime(birthYear, birthMonth, birthDay),
                CreditCard = creditCardNr
            };
            dbContext.Add(customer);
            dbContext.SaveChanges();
        }

        private static void CustomerStartPage()
        {
            using var dbContext = new TheWebShopContext();
            var customerLoop = true;
            while (customerLoop)
            {
                Console.Clear();

                Console.WriteLine("Välkommen till Webbshoppen!\n");

                Console.WriteLine("Utvalda produkter:");
                var chosenProducts = dbContext.Products
                    .Where(x => x.ChosenProduct)
                    .ToList();

                if (chosenProducts.Count > 3)
                {
                    var randomized = Randomize(chosenProducts)
                        .Take(3);

                    Console.WriteLine($"Id\tPris \t Namn");
                    foreach (var product in randomized)
                    {
                        Console.WriteLine($"{product.Id}\t{product.Price} kr\t {product.Name} - {product.DetailedInfo}");
                    }
                }
                else
                {
                    Console.WriteLine($"Id\tPris \t Namn");
                    foreach (var product in chosenProducts)
                    {
                        Console.WriteLine($"{product.Id}\t{product.Price} kr\t {product.Name} - {product.DetailedInfo}");
                    }
                }

                Console.WriteLine();
                Console.WriteLine("[S]hopsida");
                Console.WriteLine("[V]arukorg");
                Console.WriteLine("[B]acka");

                var choice = Console.ReadKey(true).KeyChar;
                switch (choice)
                {
                    case 'S':
                    case 's':
                        ShoppingPage();
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

        private static void ShoppingCart()
        {
            throw new NotImplementedException();
        }

        private static void ShoppingPage()
        {
            using var dbContext = new TheWebShopContext();
            var shoppingLoop = true;
            while (shoppingLoop)
            {
                Console.Clear();
                Console.WriteLine("Kategorier");
                foreach (var c in dbContext.Categories
                    .Include(x => x.Products))
                {
                    Console.WriteLine($"[{c.Id}] {c.Name}");
                }
                Console.WriteLine();
                Console.Write("Ange text för att fritextsöka, annars ange Id för kategori för att se produkter eller [0] att backa menyn: ");
                var search = Console.ReadLine();
                int categoryId;
                if (!int.TryParse(search, out categoryId))
                {
                    ShowContains(search);
                }
                else
                {
                    if (categoryId.ToString().Length > 2)
                    {
                        ShowContains(search);
                    }
                    else
                    {
                        var products = dbContext.Products
                            .Where(x => x.CategoryId == categoryId);
                        if (products.Count() > 0)
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
                                    Console.WriteLine($"Id [{p.Id}] - {p.Name} - {p.Price} kr - {p.DetailedInfo}");
                                }
                                Console.WriteLine();
                                Console.WriteLine("Välj Id för att lägga till i varukorg, eller ange [0] för att backa");
                                var buyLoop = true;
                                while (buyLoop)
                                {
                                    var input = Console.ReadLine();
                                    int productId = -1;
                                    while (!int.TryParse(input, out productId))
                                    {
                                        Console.WriteLine("Felaktig inmatning, försök igen");
                                        input = Console.ReadLine();
                                    }
                                    var product = dbContext.Products
                                        .Where(x => x.Id == productId && x.CategoryId == categoryId)
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
                                        ShowProduct(product);
                                        buyLoop = false;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (categoryId == 0)
                            {
                                shoppingLoop = false;
                            }
                            else
                            {
                                ShowContains(search);
                            }
                        }
                    }
                }
            }
        }

        private static void ShowProduct(Product product)
        {
            var showProdLoop = true;
            if (product.Quantity == 0)
            {
                Console.Clear();
                Console.WriteLine(product.Name + " finns ej i lager");
                Console.WriteLine();
                Console.WriteLine($"Id\tPris \t Namn");
                Console.WriteLine($"{product.Id}\t{product.Price} kr\t {product.Name} - {product.DetailedInfo}");
            }
            else
            {
                while (showProdLoop)
                {
                    Console.Clear();
                    Console.WriteLine(product.Quantity + " finns i lager");
                    Console.WriteLine();
                    Console.WriteLine($"{product.Id}\t{product.Price} kr\t {product.Name} - {product.DetailedInfo}");
                    Console.WriteLine("Skriv antalet av denna produkt du vill köpa eller 0 för att backa i menyn");
                    var customeranswer = Convert.ToInt32(Console.ReadLine());
                    if (customeranswer != 0 && customeranswer <= product.Quantity)
                    {
                        for (int i = 0; i < customeranswer; i++)
                        {
                            //Todo se till att en kund är med
                        }
                        showProdLoop = false;
                        Console.WriteLine(customeranswer + " produkter tillagda i varukorg");
                        Console.ReadKey();
                    }
                    else if (customeranswer != 0 && customeranswer > product.Quantity)
                    {
                        Console.WriteLine("Du har valt fler produkter än vad som finns i lager.");
                        Console.ReadKey(true);
                    }
                    else
                    {
                        showProdLoop = false;
                    }

                }

            }
        }

        public static void ShowContains(string input)
        {
            using var dbContext = new TheWebShopContext();
            var show = dbContext.Products
                .Include(x => x.Supplier)
                .Include(x => x.Category)
                .Where(x => x.Name.Contains(input) || x.DetailedInfo.Contains(input) || x.Supplier.Name.Contains(input) || x.Category.Name.Contains(input));
            Console.WriteLine();
            if (show.Count() > 0)
            {
                foreach (var c in show)
                {
                    Console.WriteLine(c.Name + " - " + c.DetailedInfo + " - " + c.Supplier.Name);
                }
            }
            else
            {
                Console.WriteLine("Sökningen gav ingen träff");
            }
            Console.WriteLine();
            Console.WriteLine("Tryck på valfri knapp för att söka igen");
            Console.ReadKey(true);
        }

        public static void Admin()
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
                        ProductHandling();
                        break;
                    case '2':
                        // Customer
                        break;
                    case '3':
                        PaymentMethods();
                        break;
                    case '4':
                        FreightMethods();
                        break;
                    case '5':
                        break;
                    case '6':
                        Locations();
                        break;
                    case '0':
                        adminLoop = false;
                        break;
                    default:
                        break;
                }
            }
        }

        private static void ProductHandling()
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
                        Products();
                        break;
                    case '2':
                        Category();
                        break;
                    case '3':
                        Supplier();
                        break;
                    case '0':
                        loop = false;
                        break;
                    default:
                        break;
                }
            }
        }

        private static void Locations()
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
                        Countries();
                        break;
                    case '2':
                        Cities();
                        break;
                    case '0':
                        locationLoop = false;
                        break;
                    default:
                        break;
                }
            }
        }

        private static void FreightMethods()
        {
            using var dbContext = new TheWebShopContext();
            var freightLoop = true;
            while (freightLoop)
            {
                Console.Clear();


                Console.WriteLine($"Id Namn\tPris");
                foreach (var freight in dbContext.Freights)
                {
                    Console.WriteLine($"{freight.Id} {freight.Name}\t{freight.Price}");
                }

                Console.WriteLine();
                Console.WriteLine("[1] Lägga till fraktmetod");
                Console.WriteLine("[2] Ta bort fraktmetod");
                Console.WriteLine("[3] Ändra fraktmetod");
                Console.WriteLine("[0] Backa meny");

                var choice = Console.ReadKey(true).KeyChar;
                switch (choice)
                {
                    case '1':
                        // Klar
                        Console.WriteLine("Ange namn på ny fraktmetod, samt en kort beskrivning");
                        var name = Console.ReadLine();
                        Console.WriteLine("Ange pris för " + name);
                        int price = Convert.ToInt32(Console.ReadLine());
                        dbContext.Freights.Add(new Freight { Name = name, Price = price });
                        dbContext.SaveChanges();
                        break;
                    case '2':
                        // Klar
                        Console.WriteLine("Ange id på den fraktmetod du vill ta bort");
                        var input = Console.ReadLine();
                        int id;
                        while (!int.TryParse(input, out id))
                        {
                            Console.WriteLine("Felaktig inmatning, försök igen");
                            input = Console.ReadLine();
                        }

                        var freight = dbContext.Freights.Where(x => x.Id == id).FirstOrDefault();
                        if (freight is not null)
                        {
                            Console.WriteLine($"Är du säker på att du vill radera fraktmetoden {freight.Name} med id {freight.Id}?");
                            Console.WriteLine("[J] för ja");
                            Console.WriteLine("[N] för nej");
                            var answer = Console.ReadKey(true).KeyChar;
                            switch (answer)
                            {
                                case 'J':
                                case 'j':
                                    dbContext.Freights.Remove(freight);
                                    dbContext.SaveChanges();
                                    break;
                                default:
                                    Console.WriteLine("Du valde att inte ta bort fraktmetoden. Tryck valfri tangent");
                                    Console.ReadKey(true);
                                    break;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Fraktmetoden fanns ej. Tryck valfri tangent");
                            Console.ReadKey(true);
                        }
                        break;
                    case '3':
                        Console.WriteLine("Ange id på den fraktmetod du vill ändra");
                        var input2 = Console.ReadLine();

                        int id2;
                        while (!int.TryParse(input2, out id2))
                        {
                            Console.WriteLine("Felaktig inmatning, försök igen");
                            input2 = Console.ReadLine();
                        }

                        var freight2 = dbContext.Freights.Where(x => x.Id == id2).FirstOrDefault();
                        if (freight2 is not null)
                        {
                            Console.WriteLine($"Vill du ändra på namn \"{freight2.Name}\" eller pris?");
                            Console.WriteLine("[N]amn");
                            Console.WriteLine("[P]ris");
                            var answer2 = Console.ReadKey(true).KeyChar;
                            switch (answer2)
                            {
                                case 'N':
                                case 'n':
                                    Console.WriteLine("Ange nytt namn");
                                    freight2.Name = Console.ReadLine();
                                    dbContext.SaveChanges();
                                    break;
                                case 'P':
                                case 'p':
                                    Console.WriteLine("Nuvarande pris: " + freight2.Price);
                                    Console.WriteLine("Ange nytt pris");
                                    freight2.Price = Convert.ToInt32(Console.ReadLine());
                                    dbContext.SaveChanges();
                                    break;
                                default:
                                    Console.WriteLine("Du valde att ändra fraktmetoden. Tryck valfri tangent");
                                    Console.ReadKey(true);
                                    break;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Fraktmetoden fanns ej. Tryck valfri tangent");
                            Console.ReadKey(true);
                        }
                        break;
                    case '0':
                        freightLoop = !freightLoop;
                        break;
                    default:
                        break;
                }

            }
        }

        private static void PaymentMethods()
        {
            using var dbContext = new TheWebShopContext();
            var paymentLoop = true;
            while (paymentLoop)
            {
                Console.Clear();

                Console.WriteLine($"Id Namn");
                foreach (var pm in dbContext.PaymentMethods)
                {
                    Console.WriteLine($"{pm.Id} {pm.Name}");
                }

                Console.WriteLine();
                Console.WriteLine("[1] Lägga till betalmetod");
                Console.WriteLine("[2] Ta bort betalmetod");
                Console.WriteLine("[3] Ändra namn på betalmetod");
                Console.WriteLine("[0] Backa meny");

                var choice = Console.ReadKey(true).KeyChar;
                switch (choice)
                {
                    case '1':
                        // Klar
                        Console.WriteLine("Ange namn på ny betalmetod");
                        var name = Console.ReadLine();
                        dbContext.PaymentMethods.Add(new PaymentMethod { Name = name });
                        dbContext.SaveChanges();
                        break;
                    case '2':
                        // Klar
                        Console.WriteLine("Ange id på den betalmetod du vill ta bort");
                        var input = Console.ReadLine();
                        int id;
                        while (!int.TryParse(input, out id))
                        {
                            Console.WriteLine("Felaktig inmatning, försök igen");
                            input = Console.ReadLine();
                        }

                        var paymentMethod = dbContext.PaymentMethods.Where(x => x.Id == id).FirstOrDefault();
                        if (paymentMethod is not null)
                        {
                            Console.WriteLine($"Är du säker på att du vill radera {paymentMethod.Name} med id {paymentMethod.Id}?");
                            Console.WriteLine("[J] för ja");
                            Console.WriteLine("[N] för nej");
                            var answer = Console.ReadKey(true).KeyChar;
                            switch (answer)
                            {
                                case 'J':
                                case 'j':
                                    dbContext.PaymentMethods.Remove(paymentMethod);
                                    dbContext.SaveChanges();
                                    break;
                                default:
                                    Console.WriteLine("Du valde att inte ta bort betalmetoden. Tryck valfri tangent");
                                    Console.ReadKey(true);
                                    break;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Betalmetoden fanns ej. Tryck valfri tangent");
                            Console.ReadKey(true);
                        }
                        break;
                    case '3':
                        Console.WriteLine("Ange id på betalmetoden du vill ändra namn på");
                        var input2 = Console.ReadLine();

                        int id2;
                        while (!int.TryParse(input2, out id2))
                        {
                            Console.WriteLine("Felaktig inmatning, försök igen");
                            input2 = Console.ReadLine();
                        }

                        var paymentMethod2 = dbContext.PaymentMethods.Where(x => x.Id == id2).FirstOrDefault();
                        if (paymentMethod2 is not null)
                        {
                            Console.WriteLine($"Ange nytt namn på \"{paymentMethod2.Name}\"");
                            paymentMethod2.Name = Console.ReadLine();
                            dbContext.SaveChanges();
                        }
                        else
                        {
                            Console.WriteLine("Betalmetoden fanns ej. Tryck valfri tangent");
                            Console.ReadKey(true);
                        }
                        break;
                    case '0':
                        paymentLoop = !paymentLoop;
                        break;
                    default:
                        break;
                }
            }
        }

        private static void Products()
        {
            using var dbContext = new TheWebShopContext();
            var exitLoop = false;
            var productLoop = true;
            while (productLoop)
            {
                Console.Clear();

                Console.WriteLine($"Id Utvald\tNamn");
                foreach (var product in dbContext.Products)
                {
                    Console.WriteLine($"{product.Id} {product.ChosenProduct}\t\t{product.Name}");
                }

                Console.WriteLine();
                Console.WriteLine("[1] Lägga till produkt");
                Console.WriteLine("[2] Ta bort produkt");
                Console.WriteLine("[3] Ändra produkt");
                Console.WriteLine("[0] Backa meny");

                var choice = Console.ReadKey(true).KeyChar;
                switch (choice)
                {
                    case '1':
                        Console.Clear();
                        // Klar
                        foreach (var product in dbContext.Products)
                        {
                            Console.WriteLine($"[{product.Id}] {product.Name}");
                        }
                        //Console.WriteLine($"\n[0] för att backa\n");
                        Console.WriteLine("\nAnge information på ny produkt eller ange [0] för att backa");
                        Console.Write("Produktnamn: ");
                        var name = Console.ReadLine();
                        if (name == "0")
                        {
                            break;
                        }
                        Console.Write("Pris: ");
                        int price = Convert.ToInt32(Console.ReadLine());
                        Console.Write("Detaljerad info: ");
                        var detailedInfo = Console.ReadLine();
                        Console.Write("Antal: ");
                        int quantity = Convert.ToInt32(Console.ReadLine());
                        //Console.Write("Utvald produkt: ");
                        //var chosenProduct = Console.ReadLine().ToLower(); //ternery i constructorn
                        foreach (var s in dbContext.Suppliers)
                        {
                            Console.WriteLine($"[{s.Id}] {s.Name}");
                        }
                        int supplierId = 0;
                        while (!exitLoop)
                        {
                            Console.Write("Ange id på leverantör: ");
                            var input4 = Console.ReadLine();

                            while (!int.TryParse(input4, out supplierId))
                            {
                                Console.WriteLine("Felaktig inmatning, försök igen");
                                input4 = Console.ReadLine();
                            }

                            var supplier = dbContext.Suppliers.Where(x => x.Id == supplierId).FirstOrDefault();
                            if (supplier is not null)
                            {
                                exitLoop = true;
                            }
                            else // TODO Lägga in metod för att lägga till supplier
                            {
                                Console.WriteLine("Leverantören fanns ej. Tryck valfri tangent");
                                Console.ReadKey(true);
                            }
                        }
                        foreach (var c in dbContext.Categories)
                        {
                            Console.WriteLine($"[{c.Id}] {c.Name}");
                        }
                        int categoryId = 0;
                        exitLoop = false;
                        while (!exitLoop)
                        {
                            Console.Write("Ange id på kategori: ");
                            var input4 = Console.ReadLine();

                            while (!int.TryParse(input4, out categoryId))
                            {
                                Console.WriteLine("Felaktig inmatning, försök igen");
                                input4 = Console.ReadLine();
                            }

                            var category = dbContext.Categories.Where(x => x.Id == categoryId).FirstOrDefault();
                            if (category is not null)
                            {
                                exitLoop = true;
                            }
                            else // TODO Lägga in metod för att lägga till kategori
                            {
                                Console.WriteLine("Kategorin fanns ej. Tryck valfri tangent");
                                Console.ReadKey(true);
                            }
                        }



                        dbContext.Products.Add(new Product
                        {
                            Name = name,
                            Price = price,
                            DetailedInfo = detailedInfo,
                            Quantity = quantity,
                            CategoryId = categoryId,
                            SupplierId = supplierId
                        });
                        dbContext.SaveChanges();



                        break;
                    case '2':

                        Console.WriteLine("Ange id på produkten du vill ta bort");
                        var input2 = Console.ReadLine();
                        int id2;
                        while (!int.TryParse(input2, out id2))
                        {
                            Console.WriteLine("Felaktig inmatning, försök igen");
                            input2 = Console.ReadLine();
                        }

                        var product2 = dbContext.Products.Where(x => x.Id == id2).FirstOrDefault();
                        if (product2 is not null)
                        {
                            Console.WriteLine($"Är du säker på att du vill radera {product2.Name} med id {product2.Id}?");
                            Console.WriteLine("[J] för ja");
                            Console.WriteLine("[N] för nej");
                            var answer = Console.ReadKey(true).KeyChar;

                            if (answer == 'j' || answer == 'J')
                            {
                                dbContext.Products.Remove(product2);
                                dbContext.SaveChanges();
                            }
                            else
                            {
                                Console.WriteLine("Du valde att inte ta bort produkten. Tryck valfri tangent");
                                Console.ReadKey(true);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Produkten fanns ej. Tryck valfri tangent");
                            Console.ReadKey(true);
                        }
                        break;
                    case '3':
                        Console.WriteLine("Ange id på den produkten du vill ändra");
                        var input3 = Console.ReadLine();

                        int id3;
                        while (!int.TryParse(input3, out id3))
                        {
                            Console.WriteLine("Felaktig inmatning, försök igen");
                            input3 = Console.ReadLine();
                        }

                        var product3 = dbContext.Products.Where(x => x.Id == id3).FirstOrDefault();
                        if (product3 is not null)
                        {
                            Console.WriteLine($"Ange vad du vill ändra på");
                            Console.WriteLine("[N]amn");
                            Console.WriteLine("[P]ris");
                            Console.WriteLine("[B]eskrivning");
                            Console.WriteLine("[L]agersaldo");
                            Console.WriteLine("[U]tvald");

                            var answer2 = Console.ReadKey(true).KeyChar;
                            switch (answer2)
                            {
                                case 'N':
                                case 'n':
                                    Console.Write("Ange nytt namn: ");
                                    product3.Name = Console.ReadLine();
                                    dbContext.SaveChanges();
                                    break;
                                case 'P':
                                case 'p':
                                    Console.WriteLine("Nuvarande pris: " + product3.Price);
                                    Console.Write("Ange nytt pris: ");
                                    product3.Price = Convert.ToInt32(Console.ReadLine());
                                    dbContext.SaveChanges();
                                    break;
                                case 'B':
                                case 'b':
                                    Console.WriteLine("Nuvarande beskrivning: " + product3.DetailedInfo);
                                    Console.Write("Ange ny beskrivning: ");
                                    product3.DetailedInfo = Console.ReadLine();
                                    dbContext.SaveChanges();
                                    break;
                                case 'L':
                                case 'l':
                                    Console.WriteLine("Nuvarande lagersaldo: " + product3.Quantity);
                                    Console.Write("Ange nytt lagersaldo: ");
                                    product3.Quantity = Convert.ToInt32(Console.ReadLine());
                                    dbContext.SaveChanges();
                                    break;
                                case 'U':
                                case 'u':
                                    Console.Write(product3.Name + " är ");
                                    Console.WriteLine(product3.ChosenProduct == true ? "utvald produkt" : "ej utvald produkt");
                                    Console.Write("Vill du ändra utvald produkt?: ");
                                    var chosenProduct1 = Console.ReadLine().ToLower();
                                    if (chosenProduct1 == "ja")
                                    {
                                        product3.ChosenProduct = !product3.ChosenProduct;
                                        dbContext.SaveChanges();
                                    }
                                    break;
                                default:
                                    Console.WriteLine("Du valde att inte ändra info om produkten. Tryck valfri tangent");
                                    Console.ReadKey(true);
                                    break;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Produkten fanns ej. Tryck valfri tangent");
                            Console.ReadKey(true);
                        }
                        break;
                    case '0':
                        productLoop = false;
                        break;
                    default:
                        break;
                }
            }
        }

        private static void Cities()
        {
            using var dbContext = new TheWebShopContext();
            var cityLoop = true;
            while (cityLoop)
            {
                Console.Clear();

                Console.WriteLine($"Id Namn");
                foreach (var city in dbContext.Cities)
                {
                    Console.WriteLine($"{city.Id} {city.Name}");
                }

                Console.WriteLine();
                Console.WriteLine("[1] Lägga till stad");
                Console.WriteLine("[2] Ta bort stad");
                Console.WriteLine("[3] Ändra stad");
                Console.WriteLine("[0] Backa meny");

                var choice = Console.ReadKey(true).KeyChar;
                switch (choice)
                {
                    case '1':
                        Console.Clear();
                        // Klar
                        foreach (var country1 in dbContext.Countries)
                        {
                            Console.WriteLine($"[{country1.Id}] {country1.Name}");
                        }
                        Console.WriteLine($"[0] för att backa\n");
                        Console.WriteLine("Ange id på landet du vill lägga till en stad i");
                        var input = Console.ReadLine();

                        // TODO: Gör till egen metod
                        int id;
                        while (!int.TryParse(input, out id))

                        {
                            Console.WriteLine("Felaktig inmatning, försök igen");
                            input = Console.ReadLine();
                        }
                        if (id == 0)
                        {
                            break;
                        }

                        var country = dbContext.Countries.Where(x => x.Id == id).FirstOrDefault();
                        if (country is not null)
                        {
                            Console.WriteLine("Ange ny stad");
                            var name = Console.ReadLine();
                            dbContext.Cities.Add(new City { Name = name, CountryId = id });
                            dbContext.SaveChanges();
                        }
                        else
                        {
                            Console.WriteLine("Landet fanns ej. Tryck valfri tangent");
                            Console.ReadKey(true);
                        }
                        break;
                    case '2':
                        // Klar
                        Console.WriteLine("Ange id på staden du vill ta bort");
                        var input2 = Console.ReadLine();

                        // TODO: Gör till egen metod
                        int id2;
                        while (!int.TryParse(input2, out id2))
                        {
                            Console.WriteLine("Felaktig inmatning, försök igen");
                            input2 = Console.ReadLine();
                        }

                        var city = dbContext.Cities.Where(x => x.Id == id2).FirstOrDefault();
                        if (city is not null)
                        {
                            Console.WriteLine($"Är du säker på att du vill radera {city.Name} med id {city.Id}?");
                            Console.WriteLine("[J] för ja");
                            Console.WriteLine("[N] för nej");
                            var answer = Console.ReadKey(true).KeyChar;
                            switch (answer)
                            {
                                case 'J':
                                case 'j':
                                    dbContext.Cities.Remove(city);
                                    dbContext.SaveChanges();
                                    break;
                                default:
                                    Console.WriteLine("Du valde att inte ta bort staden. Tryck valfri tangent");
                                    Console.ReadKey(true);
                                    break;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Staden fanns ej. Tryck valfri tangent");
                            Console.ReadKey(true);
                        }
                        break;
                    case '3':
                        Console.WriteLine("Ange id på staden du vill ändra");
                        var input3 = Console.ReadLine();

                        int id3;
                        while (!int.TryParse(input3, out id3))
                        {
                            Console.WriteLine("Felaktig inmatning, försök igen");
                            input3 = Console.ReadLine();
                        }

                        var city2 = dbContext.Cities.Where(x => x.Id == id3).FirstOrDefault();
                        if (city2 is not null)
                        {
                            Console.WriteLine($"Ange nytt namn på \"{city2.Name}\"");
                            city2.Name = Console.ReadLine();
                            dbContext.SaveChanges();
                        }
                        else
                        {
                            Console.WriteLine("Staden fanns ej. Tryck valfri tangent");
                            Console.ReadKey(true);
                        }
                        break;
                    case '0':
                        cityLoop = false;
                        break;
                    default:
                        break;
                }
            }
        }

        private static void Countries()
        {
            using var dbContext = new TheWebShopContext();
            var countryLoop = true;
            while (countryLoop)
            {
                Console.Clear();

                Console.WriteLine($"Id Namn");
                foreach (var country in dbContext.Countries)
                {
                    Console.WriteLine($"{country.Id} {country.Name}");
                }

                Console.WriteLine();
                Console.WriteLine("[1] Lägga till land");
                Console.WriteLine("[2] Ta bort land");
                Console.WriteLine("[3] Ändra land");
                Console.WriteLine("[0] Backa meny");

                var choice = Console.ReadKey(true).KeyChar;
                switch (choice)
                {
                    case '1':
                        // Klar
                        Console.WriteLine("Ange nytt land");
                        var name = Console.ReadLine();
                        dbContext.Countries.Add(new Country { Name = name });
                        dbContext.SaveChanges();
                        break;
                    case '2':
                        // Klar
                        Console.WriteLine("Ange id på landet du vill ta bort");
                        var input = Console.ReadLine();

                        // TODO: Gör till egen metod
                        int id;
                        while (!int.TryParse(input, out id))
                        {
                            Console.WriteLine("Felaktig inmatning, försök igen");
                            input = Console.ReadLine();
                        }

                        var country = dbContext.Countries.Where(x => x.Id == id).FirstOrDefault();
                        if (country is not null)
                        {
                            Console.WriteLine($"Är du säker på att du vill radera {country.Name} med id {country.Id}?");
                            Console.WriteLine("[J] för ja");
                            Console.WriteLine("[N] för nej");
                            var answer = Console.ReadKey(true).KeyChar;
                            switch (answer)
                            {
                                case 'J':
                                case 'j':
                                    dbContext.Countries.Remove(country);
                                    dbContext.SaveChanges();
                                    break;
                                default:
                                    Console.WriteLine("Du valde att inte ta bort landet. Tryck valfri tangent");
                                    Console.ReadKey(true);
                                    break;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Landet fanns ej. Tryck valfri tangent");
                            Console.ReadKey(true);
                        }
                        break;
                    case '3':
                        Console.WriteLine("Ange id på landet du vill ändra");
                        var input2 = Console.ReadLine();

                        int id2;
                        while (!int.TryParse(input2, out id2))
                        {
                            Console.WriteLine("Felaktig inmatning, försök igen");
                            input2 = Console.ReadLine();
                        }

                        var country2 = dbContext.Countries.Where(x => x.Id == id2).FirstOrDefault();
                        if (country2 is not null)
                        {
                            Console.WriteLine($"Ange nytt namn på \"{country2.Name}\"");
                            country2.Name = Console.ReadLine();
                            dbContext.SaveChanges();
                        }
                        else
                        {
                            Console.WriteLine("Landet fanns ej. Tryck valfri tangent");
                            Console.ReadKey(true);
                        }
                        break;
                    case '0':
                        countryLoop = false;
                        break;
                    default:
                        break;
                }
            }
        }

        private static void Supplier()
        {
            using var dbContext = new TheWebShopContext();
            var supplierLoop = true;
            while (supplierLoop)
            {
                Console.Clear();

                Console.WriteLine($"Id Namn");
                foreach (var supplier in dbContext.Suppliers)
                {
                    Console.WriteLine($"{supplier.Id} {supplier.Name}");
                }

                Console.WriteLine();
                Console.WriteLine("[1] Lägga till leverantör");
                Console.WriteLine("[2] Ta bort leverantör");
                Console.WriteLine("[3] Ändra leverantör");
                Console.WriteLine("[0] Backa meny");

                var choice = Console.ReadKey(true).KeyChar;
                switch (choice)
                {
                    case '1':
                        // Klar
                        Console.WriteLine("Ange namn på ny leverantör");
                        var name = Console.ReadLine();
                        dbContext.Suppliers.Add(new Supplier { Name = name });
                        dbContext.SaveChanges();
                        break;
                    case '2':
                        // Klar
                        Console.WriteLine("Ange id på leverantören du vill ta bort");
                        var input = Console.ReadLine();

                        // TODO: Gör till egen metod
                        int id;
                        while (!int.TryParse(input, out id))
                        {
                            Console.WriteLine("Felaktig inmatning, försök igen");
                            input = Console.ReadLine();
                        }

                        var supplier = dbContext.Suppliers.Where(x => x.Id == id).FirstOrDefault();
                        if (supplier is not null)
                        {
                            Console.WriteLine($"Är du säker på att du vill radera {supplier.Name} med id {supplier.Id}?");
                            Console.WriteLine("[J] för ja");
                            Console.WriteLine("[N] för nej");
                            var answer = Console.ReadKey(true).KeyChar;
                            switch (answer)
                            {
                                case 'J':
                                case 'j':
                                    dbContext.Suppliers.Remove(supplier);
                                    dbContext.SaveChanges();
                                    break;
                                default:
                                    Console.WriteLine("Du valde att inte ta bort leverantören. Tryck valfri tangent");
                                    Console.ReadKey(true);
                                    break;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Leverantören fanns ej. Tryck valfri tangent");
                            Console.ReadKey(true);
                        }
                        break;
                    case '3':
                        Console.WriteLine("Ange id på leverantören du vill ändra");
                        var input2 = Console.ReadLine();

                        int id2;
                        while (!int.TryParse(input2, out id2))
                        {
                            Console.WriteLine("Felaktig inmatning, försök igen");
                            input2 = Console.ReadLine();
                        }

                        var supplier2 = dbContext.Suppliers.Where(x => x.Id == id2).FirstOrDefault();
                        if (supplier2 is not null)
                        {
                            Console.WriteLine($"Ange nytt namn på \"{supplier2.Name}\"");
                            supplier2.Name = Console.ReadLine();
                            dbContext.SaveChanges();
                        }
                        else
                        {
                            Console.WriteLine("Leverantören fanns ej. Tryck valfri tangent");
                            Console.ReadKey(true);
                        }
                        break;
                    case '0':
                        supplierLoop = false;
                        break;
                    default:
                        break;
                }
            }
        }

        private static void Category()
        {
            using var dbContext = new TheWebShopContext();
            var categoryLoop = true;
            while (categoryLoop)
            {
                Console.Clear();

                Console.WriteLine($"Id Namn");
                foreach (var category in dbContext.Categories)
                {
                    Console.WriteLine($"{category.Id} {category.Name}\t{category.Description}");
                }

                Console.WriteLine();
                Console.WriteLine("[1] Lägga till kategori");
                Console.WriteLine("[2] Ta bort kategori");
                Console.WriteLine("[3] Ändra kategori");
                Console.WriteLine("[0] Backa meny");

                var choice = Console.ReadKey(true).KeyChar;
                switch (choice)
                {
                    case '1':
                        // Klar
                        Console.WriteLine("Ange namn på ny kategori");
                        var name = Console.ReadLine();
                        Console.WriteLine("Ange beskrivning på " + name);
                        var description = Console.ReadLine();
                        dbContext.Categories.Add(new Category { Name = name, Description = description });
                        dbContext.SaveChanges();
                        break;
                    case '2':
                        // Klar
                        Console.WriteLine("Ange id på den kategorin du vill ta bort");
                        var input = Console.ReadLine();

                        // TODO: Gör till egen metod
                        int id;
                        while (!int.TryParse(input, out id))
                        {
                            Console.WriteLine("Felaktig inmatning, försök igen");
                            input = Console.ReadLine();
                        }

                        var category = dbContext.Categories.Where(x => x.Id == id).FirstOrDefault();
                        if (category is not null)
                        {
                            Console.WriteLine($"Är du säker på att du vill radera {category.Name} med id {category.Id}?");
                            Console.WriteLine("[J] för ja");
                            Console.WriteLine("[N] för nej");
                            var answer = Console.ReadKey(true).KeyChar;
                            switch (answer)
                            {
                                case 'J':
                                case 'j':
                                    dbContext.Categories.Remove(category);
                                    dbContext.SaveChanges();
                                    break;
                                default:
                                    Console.WriteLine("Du valde att inte ta bort kategorin. Tryck valfri tangent");
                                    Console.ReadKey(true);
                                    break;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Kategorin fanns ej. Tryck valfri tangent");
                            Console.ReadKey(true);
                        }
                        break;
                    case '3':
                        Console.WriteLine("Ange id på den kategorin du vill ändra");
                        var input2 = Console.ReadLine();

                        int id2;
                        while (!int.TryParse(input2, out id2))
                        {
                            Console.WriteLine("Felaktig inmatning, försök igen");
                            input2 = Console.ReadLine();
                        }

                        var category2 = dbContext.Categories.Where(x => x.Id == id2).FirstOrDefault();
                        if (category2 is not null)
                        {
                            Console.WriteLine($"Vill du ändra på namn \"{category2.Name}\" eller beskrivning?");
                            Console.WriteLine("[N]amn");
                            Console.WriteLine("[B]eskrivning");
                            var answer2 = Console.ReadKey(true).KeyChar;
                            switch (answer2)
                            {
                                case 'N':
                                case 'n':
                                    Console.WriteLine("Ange nytt namn");
                                    category2.Name = Console.ReadLine();
                                    dbContext.SaveChanges();
                                    break;
                                case 'B':
                                case 'b':
                                    Console.WriteLine("Nuvarande beskrivning: " + category2.Description);
                                    Console.WriteLine("Ange ny beskrivning");
                                    category2.Description = Console.ReadLine();
                                    dbContext.SaveChanges();
                                    break;
                                default:
                                    Console.WriteLine("Du valde att ändra kategorin. Tryck valfri tangent");
                                    Console.ReadKey(true);
                                    break;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Kategorin fanns ej. Tryck valfri tangent");
                            Console.ReadKey(true);
                        }
                        break;
                    case '0':
                        categoryLoop = false;
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
