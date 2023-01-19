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

        internal static void Create(Supplier supplier, TheWebShopContext dbContext)
        {
            Console.WriteLine("Ange namn på ny leverantör");
            supplier.Name = Console.ReadLine();
            dbContext.Add(supplier);
            dbContext.SaveChanges();
        }
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
                        Create(new Supplier(), dbContext);
                        break;
                    case '2':
                        // Klar
                        Console.WriteLine("Ange id på leverantören du vill ta bort");
                        int id = Managing.TryToParseInput();

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
                        var id2 = Managing.TryToParseInput();

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
