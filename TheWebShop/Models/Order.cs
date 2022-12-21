using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheWebShop.Models
{
    internal class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public DateOnly OrderDate { get; set; }
        public int PaymentMethodId { get; set; }
        public int FreightId { get; set; }

    }
}
