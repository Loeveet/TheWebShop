using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheWebShop.Models
{
    internal class City
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CountryId { get; set; }

        public virtual Country Country { get; set; }
        public virtual ICollection<Customer> Customers { get; set; }

        internal static void HandlingCity()
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


    }
}
