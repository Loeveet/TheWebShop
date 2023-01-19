using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheWebShop.Models
{
    internal class Freight
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }

        public virtual ICollection<Order> Orders { get; set; }

        internal static void HandlingFreightMethod()
        {
            using var dbContext = new TheWebShopContext();
            var freightLoop = true;
            while (freightLoop)
            {
                Console.Clear();


                Console.WriteLine($"Id\tNamn\tPris");
                Console.WriteLine("---------------------");
                foreach (var freight in dbContext.Freights)
                {
                    Console.WriteLine($"[{freight.Id}]\t{freight.Name}\t{freight.Price}");
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
                        int price = Managing.TryToParseInput();
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

                        int id2 = Managing.TryToParseInput();

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

    }
}
