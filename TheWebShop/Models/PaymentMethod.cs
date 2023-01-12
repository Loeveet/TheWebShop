using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheWebShop.Models
{
    internal class PaymentMethod
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Order> Orders { get; set; }

        internal static void HandlingPaymentMethod()
        {
            using var dbContext = new TheWebShopContext();
            var paymentLoop = true;
            while (paymentLoop)
            {
                Console.Clear();

                Console.WriteLine($"Id Namn");
                foreach (var pm in dbContext.PaymentMethods)
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
                        dbContext.PaymentMethods.Add(new PaymentMethod { Name = name });
                        dbContext.SaveChanges();
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

                        var paymentMethod = dbContext.PaymentMethods.Where(x => x.Id == id).FirstOrDefault();
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
                                    dbContext.PaymentMethods.Remove(paymentMethod);
                                    dbContext.SaveChanges();
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

                        var paymentMethod2 = dbContext.PaymentMethods.Where(x => x.Id == id2).FirstOrDefault();
                        if (paymentMethod2 is not null)
                        {
                            Console.WriteLine($"Ange nytt namn på \"{paymentMethod2.Name}\"");
                            paymentMethod2.Name = Console.ReadLine();
                            dbContext.SaveChanges();
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

    }
}
