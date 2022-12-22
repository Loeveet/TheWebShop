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
                        break;
                    case '6':
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
                        Console.WriteLine("Ange id på landet du vill lägga till en stad i");
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
