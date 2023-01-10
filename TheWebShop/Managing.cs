using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheWebShop.Models;

namespace TheWebShop
{
    internal class Managing
    {
        private static readonly TheWebShopContext _dbContext = new();

        public static void RunTheWebShop()
        {
            var loop = true;
            while (loop)
            {
                Console.Clear();

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
                        // Go to customer-page

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

        public static void Admin()
        {
            var adminLoop = true;
            while (adminLoop)
            {
                Console.Clear();
                Console.WriteLine("[1] Hantera produkter");
                Console.WriteLine("[2] Hantera kategorier");
                Console.WriteLine("[3] Hantera leverantör");
                Console.WriteLine("[4] Hantera kunder");
                Console.WriteLine("[5] Hantera betalmetoder");
                Console.WriteLine("[6] Hantera fraktmetoder");
                Console.WriteLine("[7] Hantera ordrar");
                Console.WriteLine("[8] Hantera städer");
                Console.WriteLine("[9] Hantera länder");
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
                    case '4':
                        break;
                    case '5':
                        PaymentMethods();
                        break;
                    case '6':
                        FreightMethods();
                        break;
                    case '7':
                        break;
                    case '8':
                        Cities();
                        break;
                    case '9':
                        Countries();
                        break;
                    case '0':
                        adminLoop = false;
                        break;
                    default:
                        break;
                }
            }
        }

        private static void FreightMethods()
        {
            var freightLoop = true;
            while (freightLoop)
            {
                Console.Clear();

                Console.WriteLine($"Id Namn\tPris");
                foreach (var freight in _dbContext.Freights)
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
                        _dbContext.Freights.Add(new Freight { Name = name, Price = price });
                        _dbContext.SaveChanges();
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

                        var freight = _dbContext.Freights.Where(x => x.Id == id).FirstOrDefault();
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
                                    _dbContext.Freights.Remove(freight);
                                    _dbContext.SaveChanges();
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

                        var freight2 = _dbContext.Freights.Where(x => x.Id == id2).FirstOrDefault();
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
                                    _dbContext.SaveChanges();
                                    break;
                                case 'P':
                                case 'p':
                                    Console.WriteLine("Nuvarande pris: " + freight2.Price);
                                    Console.WriteLine("Ange nytt pris");
                                    freight2.Price = Convert.ToInt32(Console.ReadLine());
                                    _dbContext.SaveChanges();
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
            var paymentLoop = true;
            while (paymentLoop)
            {
                Console.Clear();

                Console.WriteLine($"Id Namn");
                foreach (var pm in _dbContext.PaymentMethods)
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
                        _dbContext.PaymentMethods.Add(new PaymentMethod { Name = name });
                        _dbContext.SaveChanges();
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

                        var paymentMethod = _dbContext.PaymentMethods.Where(x => x.Id == id).FirstOrDefault();
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
                                    _dbContext.PaymentMethods.Remove(paymentMethod);
                                    _dbContext.SaveChanges();
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

                        var paymentMethod2 = _dbContext.PaymentMethods.Where(x => x.Id == id2).FirstOrDefault();
                        if (paymentMethod2 is not null)
                        {
                            Console.WriteLine($"Ange nytt namn på \"{paymentMethod2.Name}\"");
                            paymentMethod2.Name = Console.ReadLine();
                            _dbContext.SaveChanges();
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
            var exitLoop = false;
            var productLoop = true;
            while (productLoop)
            {
                Console.Clear();

                Console.WriteLine($"Id Namn");
                foreach (var product in _dbContext.Products)
                {
                    Console.WriteLine($"{product.Id} {product.Name}");
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
                        foreach (var product in _dbContext.Products)
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
                        Console.Write("Utvald produkt: ");
                        var chosenProduct = Console.ReadLine().ToLower(); //ternery i constructorn
                        foreach (var s in _dbContext.Suppliers)
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

                            var supplier = _dbContext.Suppliers.Where(x => x.Id == supplierId).FirstOrDefault();
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
                        foreach (var c in _dbContext.Categories)
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

                            var category = _dbContext.Categories.Where(x => x.Id == categoryId).FirstOrDefault();
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



                        _dbContext.Products.Add(new Product
                        {
                            Name = name,
                            Price = price,
                            DetailedInfo = detailedInfo,
                            Quantity = quantity,
                            CategoryId = categoryId,
                            SupplierId = supplierId,
                            ChosenProduct = chosenProduct == "ja" ? true : false
                        });
                        _dbContext.SaveChanges();



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

                        var product2 = _dbContext.Products.Where(x => x.Id == id2).FirstOrDefault();
                        if (product2 is not null)
                        {
                            Console.WriteLine($"Är du säker på att du vill radera {product2.Name} med id {product2.Id}?");
                            Console.WriteLine("[J] för ja");
                            Console.WriteLine("[N] för nej");
                            var answer = Console.ReadKey(true).KeyChar;

                            if (answer == 'j' || answer == 'J')
                            {
                                _dbContext.Products.Remove(product2);
                                _dbContext.SaveChanges();
                            }
                            else
                            {
                                Console.WriteLine("Du valde att inte ta bort produkten. Tryck valfri tangent");
                                Console.ReadKey(true);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Staden fanns ej. Tryck valfri tangent");
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

                        var product3 = _dbContext.Products.Where(x => x.Id == id3).FirstOrDefault();
                        if (product3 is not null)
                        {
                            Console.WriteLine($"Ange vad du vill ändra på");
                            Console.WriteLine("[N]amn");
                            Console.WriteLine("[P]ris");
                            Console.WriteLine("[B]eskrivning");
                            Console.WriteLine("[A]ntal");
                            Console.WriteLine("[U]tvald");

                            var answer2 = Console.ReadKey(true).KeyChar;
                            switch (answer2)
                            {
                                case 'N':
                                case 'n':
                                    Console.Write("Ange nytt namn: ");
                                    product3.Name = Console.ReadLine();
                                    _dbContext.SaveChanges();
                                    break;
                                case 'P':
                                case 'p':
                                    Console.WriteLine("Nuvarande pris: " + product3.Price);
                                    Console.Write("Ange nytt pris: ");
                                    product3.Price = Convert.ToInt32(Console.ReadLine());
                                    _dbContext.SaveChanges();
                                    break;
                                case 'B':
                                case 'b':
                                    Console.WriteLine("Nuvarande beskrivning: " + product3.DetailedInfo);
                                    Console.Write("Ange ny beskrivning: ");
                                    product3.DetailedInfo = Console.ReadLine();
                                    _dbContext.SaveChanges();
                                    break;
                                case 'A':
                                case 'a':
                                    Console.WriteLine("Nuvarande antal: " + product3.Quantity);
                                    Console.Write("Ange nytt antal: ");
                                    product3.Quantity = Convert.ToInt32(Console.ReadLine());
                                    _dbContext.SaveChanges();
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
                                        _dbContext.SaveChanges();
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
            var cityLoop = true;
            while (cityLoop)
            {
                Console.Clear();

                Console.WriteLine($"Id Namn");
                foreach (var city in _dbContext.Cities)
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
                        foreach (var country1 in _dbContext.Countries)
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

                        var country = _dbContext.Countries.Where(x => x.Id == id).FirstOrDefault();
                        if (country is not null)
                        {
                            Console.WriteLine("Ange ny stad");
                            var name = Console.ReadLine();
                            _dbContext.Cities.Add(new City { Name = name, CountryId = id });
                            _dbContext.SaveChanges();
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

                        var city = _dbContext.Cities.Where(x => x.Id == id2).FirstOrDefault();
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
                                    _dbContext.Cities.Remove(city);
                                    _dbContext.SaveChanges();
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

                        var city2 = _dbContext.Cities.Where(x => x.Id == id3).FirstOrDefault();
                        if (city2 is not null)
                        {
                            Console.WriteLine($"Ange nytt namn på \"{city2.Name}\"");
                            city2.Name = Console.ReadLine();
                            _dbContext.SaveChanges();
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
            var countryLoop = true;
            while (countryLoop)
            {
                Console.Clear();

                Console.WriteLine($"Id Namn");
                foreach (var country in _dbContext.Countries)
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
                        _dbContext.Countries.Add(new Country { Name = name });
                        _dbContext.SaveChanges();
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

                        var country = _dbContext.Countries.Where(x => x.Id == id).FirstOrDefault();
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
                                    _dbContext.Countries.Remove(country);
                                    _dbContext.SaveChanges();
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

                        var country2 = _dbContext.Countries.Where(x => x.Id == id2).FirstOrDefault();
                        if (country2 is not null)
                        {
                            Console.WriteLine($"Ange nytt namn på \"{country2.Name}\"");
                            country2.Name = Console.ReadLine();
                            _dbContext.SaveChanges();
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
            var supplierLoop = true;
            while (supplierLoop)
            {
                Console.Clear();

                Console.WriteLine($"Id Namn");
                foreach (var supplier in _dbContext.Suppliers)
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
                        _dbContext.Suppliers.Add(new Supplier { Name = name });
                        _dbContext.SaveChanges();
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

                        var supplier = _dbContext.Suppliers.Where(x => x.Id == id).FirstOrDefault();
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
                                    _dbContext.Suppliers.Remove(supplier);
                                    _dbContext.SaveChanges();
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

                        var supplier2 = _dbContext.Suppliers.Where(x => x.Id == id2).FirstOrDefault();
                        if (supplier2 is not null)
                        {
                            Console.WriteLine($"Ange nytt namn på \"{supplier2.Name}\"");
                            supplier2.Name = Console.ReadLine();
                            _dbContext.SaveChanges();
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
            var categoryLoop = true;
            while (categoryLoop)
            {
                Console.Clear();

                Console.WriteLine($"Id Namn");
                foreach (var category in _dbContext.Categories)
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
                        _dbContext.Categories.Add(new Category { Name = name, Description = description });
                        _dbContext.SaveChanges();
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

                        var category = _dbContext.Categories.Where(x => x.Id == id).FirstOrDefault();
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
                                    _dbContext.Categories.Remove(category);
                                    _dbContext.SaveChanges();
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

                        var category2 = _dbContext.Categories.Where(x => x.Id == id2).FirstOrDefault();
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
                                    _dbContext.SaveChanges();
                                    break;
                                case 'B':
                                case 'b':
                                    Console.WriteLine("Nuvarande beskrivning: " + category2.Description);
                                    Console.WriteLine("Ange ny beskrivning");
                                    category2.Description = Console.ReadLine();
                                    _dbContext.SaveChanges();
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
    }
}
