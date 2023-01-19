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
        public virtual ICollection<OrderProduct> OrderDetails { get; set; }
        public virtual ICollection<Cart> Carts { get; set; }

        internal static void HandlingProduct()
        {
            using var dbContext = new TheWebShopContext();
            var isNotNullLoop = false;
            var loop = true;
            while (loop)
            {
                Console.Clear();

                Console.WriteLine($"Id\tUtvald\tNamn\t\tKategori\tLagersaldo");
                Console.WriteLine("-----------------------------------------------------------");
                foreach (var product in dbContext.Products.Include(x => x.Category))
                {
                    Console.WriteLine($"[{product.Id}]\t{(product.ChosenProduct ? "Ja" : "Nej")}\t{product.Name}\t{product.Category.Name}\t{product.Quantity}");
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
                        foreach (var product in dbContext.Products)
                        {
                            Console.WriteLine($"[{product.Id}] {product.Name}");
                        }
                        Console.WriteLine("\nAnge information på ny produkt eller ange [0] för att backa");
                        Console.Write("Produktnamn: ");
                        var name = Console.ReadLine();
                        if (name == "0")
                        {
                            break;
                        }
                        Console.Write("Pris: ");
                        var price = Managing.TryToParseInput();
                        Console.Write("Detaljerad info: ");
                        var detailedInfo = Console.ReadLine();
                        Console.Write("Antal: ");
                        var quantity = Managing.TryToParseInput();
                        foreach (var s in dbContext.Suppliers)
                        {
                            Console.WriteLine($"[{s.Id}] {s.Name}");
                        }
                        int supplierId = 0;
                        while (!isNotNullLoop)
                        {
                            Console.Write("Ange id på leverantör: ");

                            supplierId = Managing.TryToParseInput();

                            var supplier = dbContext.Suppliers.Where(x => x.Id == supplierId).FirstOrDefault();
                            if (supplier is not null)
                            {
                                isNotNullLoop = true;
                            }
                            else
                            {
                                Console.WriteLine("Leverantören fanns ej. Tryck valfri tangent");
                                Supplier.Create(new Supplier(), dbContext);
                                Console.ReadKey(true);
                            }
                        }
                        foreach (var c in dbContext.Categories)
                        {
                            Console.WriteLine($"[{c.Id}] {c.Name}");
                        }
                        int categoryId = 0;
                        isNotNullLoop = false;
                        while (!isNotNullLoop)
                        {
                            Console.Write("Ange id på kategori: ");
                            categoryId = Managing.TryToParseInput();

                            var category = dbContext.Categories.Where(x => x.Id == categoryId).FirstOrDefault();
                            if (category is not null)
                            {
                                isNotNullLoop = true;
                            }
                            else
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
                        int removeProductId = Managing.TryToParseInput();

                        var removeProduct = dbContext.Products.Where(x => x.Id == removeProductId).FirstOrDefault();
                        if (removeProduct is not null)
                        {
                            Console.WriteLine($"Är du säker på att du vill radera {removeProduct.Name} med id {removeProduct.Id}?");
                            Console.WriteLine("[J] för ja");
                            Console.WriteLine("[N] för nej");
                            var answer = Console.ReadKey(true).KeyChar;

                            if (answer == 'j' || answer == 'J')
                            {
                                dbContext.Products.Remove(removeProduct);
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

                        int changeProductId = Managing.TryToParseInput();

                        var changeProduct = dbContext.Products.Where(x => x.Id == changeProductId).FirstOrDefault();
                        if (changeProduct is not null)
                        {
                            Console.WriteLine($"Ange vad du vill ändra på");
                            Console.WriteLine("[1] Namn");
                            Console.WriteLine("[2] Pris");
                            Console.WriteLine("[3] Beskrivning");
                            Console.WriteLine("[4] Lagersaldo");
                            Console.WriteLine("[0] Utvald");

                            var answer2 = Console.ReadKey(true).KeyChar;
                            switch (answer2)
                            {
                                case '1':
                                    Console.Write("Ange nytt namn: ");
                                    changeProduct.Name = Console.ReadLine();
                                    break;
                                case '2':
                                    Console.WriteLine("Nuvarande pris: " + changeProduct.Price);
                                    Console.Write("Ange nytt pris: ");
                                    changeProduct.Price = Managing.TryToParseInput();
                                    break;
                                case '3':
                                    Console.WriteLine("Nuvarande beskrivning: " + changeProduct.DetailedInfo);
                                    Console.Write("Ange ny beskrivning: ");
                                    changeProduct.DetailedInfo = Console.ReadLine();
                                    break;
                                case '4':
                                    Console.WriteLine("Nuvarande lagersaldo: " + changeProduct.Quantity);
                                    Console.Write("Ange nytt lagersaldo: ");
                                    changeProduct.Quantity = Managing.TryToParseInput();
                                    break;
                                case '0':
                                    Console.Write(changeProduct.Name + " är ");
                                    Console.WriteLine(changeProduct.ChosenProduct == true ? "utvald produkt" : "ej utvald produkt");
                                    Console.Write("Vill du ändra utvald produkt?: ");
                                    var chosenProduct1 = Console.ReadLine().ToLower();
                                    if (chosenProduct1 == "ja")
                                    {
                                        changeProduct.ChosenProduct = !changeProduct.ChosenProduct;
                                        dbContext.SaveChanges();
                                    }
                                    break;
                                default:
                                    Console.WriteLine("Du valde att inte ändra info om produkten. Tryck valfri tangent");
                                    Console.ReadKey(true);
                                    break;
                            }
                            dbContext.SaveChanges();
                        }
                        else
                        {
                            Console.WriteLine("Produkten fanns ej. Tryck valfri tangent");
                            Console.ReadKey(true);
                        }
                        break;
                    case '0':
                        loop = false;
                        break;
                    default:
                        break;
                }
            }
        }

        internal static void ShowProduct(Product product, Customer customer, TheWebShopContext dbContext)
        {
            var loop = true;
            if (product.Quantity == 0)
            {
                Console.Clear();
                Cart.PrintCart(customer);
                Console.WriteLine($"Id\tPris \t Namn");
                Console.WriteLine($"{product.Id}\t{product.Price} kr\t {product.Name} - {product.DetailedInfo}");
                Console.WriteLine();
                Console.WriteLine(product.Name + " finns ej i lager. Tryck på valfri tangent");
                Console.ReadKey(true);
                
            }
            else
            {
                while (loop)
                {
                    Console.Clear();
                    Cart.PrintCart(customer);
                    Console.WriteLine(product.Quantity + " finns i lager");
                    Console.WriteLine();
                    Console.WriteLine($"[{product.Id}]\t{product.Price} kr\t {product.Name} - {product.DetailedInfo}");
                    Console.WriteLine("Skriv antalet av denna produkt du vill köpa eller 0 för att backa i menyn");
                    var productsToBuy = Managing.TryToParseInput();
                    if (productsToBuy != 0 && productsToBuy <= product.Quantity)
                    {

                        for (int i = 0; i < productsToBuy; i++)
                        {
                            dbContext.Add(new Cart { CustomerId = customer.Id, ProductId = product.Id });
                            product.Quantity--;
                        }
                        dbContext.SaveChanges();

                        Console.WriteLine($"{productsToBuy} st {product.Name} är tillagt i varukorgen");
                        loop = false;
                    }
                    else if (productsToBuy != 0 && productsToBuy > product.Quantity)
                    {
                        Console.WriteLine("Du har valt fler produkter än vad som finns i lager.");
                        Console.ReadKey(true);
                    }
                    else
                    {
                        loop = false;
                    }
                }
            }
        }

        internal static void ShowSearchResults(Customer customer)
        {
            var searchLoop = true;
            while (searchLoop)
            {
                Console.Clear();
                Cart.PrintCart(customer);
                Console.Write("Fritextsökning för produkter: ");

                var input = Console.ReadLine();

                using var dbContext = new TheWebShopContext();
                var searchResult = dbContext.Products
                    .Include(x => x.Supplier)
                    .Include(x => x.Category)
                    .Where(x => x.Name.Contains(input) || x.DetailedInfo.Contains(input) || x.Supplier.Name.Contains(input) || x.Category.Name.Contains(input));

                Console.WriteLine();
                if (searchResult.Any())
                {
                    var loop = true;
                    while (loop)
                    {
                        Console.Clear();
                        Cart.PrintCart(customer);
                        Console.WriteLine("Din sökning gav följande träffar");
                        Console.WriteLine("----------------------------------");
                        foreach (var product in searchResult)
                        {
                            Console.WriteLine($"[{product.Id}] {product.Name} - {product.Price} kr - {product.DetailedInfo}");
                        }
                        Console.WriteLine();

                        Console.WriteLine("Ange Id för att läsa mer om produkten eller [0] för att backa");
                        var productId = Managing.TryToParseInput();

                        if (productId is 0)
                        {
                            loop = false;
                        }
                        else if (searchResult.Any(x => x.Id == productId))
                        {
                            var product = searchResult
                                .Where(x => x.Id == productId)
                                .FirstOrDefault();
                            ShowProduct(product, customer, dbContext);
                            searchLoop = false;
                            loop = false;
                        }
                        else
                        {
                            Console.WriteLine("Fanns ingen produkt i urvalet med angivet produktId. Tryck valfri tangent för att fortsätta");
                            Console.ReadKey(true);

                        }
                    }
                }
                else
                {
                    Console.WriteLine("Sökningen gav ingen träff. Tryck valfri tangent för att fortsätta");
                    Console.ReadKey(true);

                }
            }
        }

    }
}
