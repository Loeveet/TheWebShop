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

                Console.WriteLine($"Id\tNamn");
                Console.WriteLine("-------------");
                foreach (var pm in dbContext.PaymentMethods)
                {
                    Console.WriteLine($"[{pm.Id}]\t{pm.Name}");
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
                        Create(new PaymentMethod(), dbContext);
                        break;
                    case '2':
                        Console.WriteLine("Ange id på den betalmetod du vill ta bort");
                        int id = Managing.TryToParseInput();

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

                        int id2 = Managing.TryToParseInput();

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
        internal static void Create(PaymentMethod paymentMethod, TheWebShopContext dbContext)
        {
            Console.WriteLine("Ange namn på ny betalmetod");
            paymentMethod.Name = Console.ReadLine();
            dbContext.Add(paymentMethod);
            dbContext.SaveChanges();
        }

    }
}
