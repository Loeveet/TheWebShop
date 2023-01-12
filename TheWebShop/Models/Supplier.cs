using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheWebShop.Models
{
    internal class Supplier
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Product> Products { get; set; }

        internal static void HandlingSupplier()
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
    }
}
