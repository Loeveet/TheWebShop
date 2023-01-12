using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheWebShop.Models
{
    internal class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string DetailedInfo { get; set; }
        public int Quantity { get; set; }
        public int CategoryId { get; set; }
        public int SupplierId { get; set; }
        public bool ChosenProduct { get; set; }

        public virtual Category Category { get; set; }
        public virtual Supplier Supplier { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<Cart> Carts { get; set; }

        internal static void HandlingProduct()
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
                            //CategoryId = categoryId,
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
        internal static void ShowProduct(Product product, Customer customer)
        {
            using var dbContext = new TheWebShopContext();
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
                        
                        // TODO: Här är vi!
                        for (int i = 0; i < customeranswer; i++)
                        {
                            customer.Cart.Products
                                .Add(product);
                        }
                        dbContext.Entry(customer).State= EntityState.Modified;
                        dbContext.SaveChanges();

                        showProdLoop = false;
                        Console.WriteLine(customeranswer + " produkter tillagda i varukorg");
                        foreach (var x in dbContext.Carts.Include(x => x.Products).Include(x => x.Customer))
                        {
                            Console.WriteLine(x.Customer.FirstName);
                            foreach (var y in x.Products)
                            {
                                Console.WriteLine("\t" + y.Name);
                            }
                        }
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
        internal static void ShowContains(string input)
        {
            using var dbContext = new TheWebShopContext();
            var show = dbContext.Products
                .Include(x => x.Supplier)
                .Include(x => x.Category)
                .Where(x => x.Name.Contains(input) || x.DetailedInfo.Contains(input) || x.Supplier.Name.Contains(input) || x.Category.Name.Contains(input));
            Console.WriteLine();
            if (show.Count() > 0)
            {
                foreach (var p in show)
                {
                    Console.WriteLine(p.Name + " - " + p.DetailedInfo + " - " + p.Supplier.Name);
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

    }
}
