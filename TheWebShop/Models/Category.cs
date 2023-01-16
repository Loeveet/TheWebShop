using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheWebShop.Models
{
    internal class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Product> Products { get; set; }

        internal static void HandlingCategory()
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
                        Create(new Category(), dbContext);
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
        internal static void Create(Category category, TheWebShopContext dbContext)
        {
            Console.WriteLine("Ange namn på ny kategori");
            var name = Console.ReadLine();
            dbContext.Add(new Category { Name = name });
            dbContext.SaveChanges();
        }
    }
}
