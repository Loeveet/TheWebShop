using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheWebShop.Models
{
    internal class Country
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<City> Cities { get; set; }

        internal static void HandlingCountry()
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


    }
}
