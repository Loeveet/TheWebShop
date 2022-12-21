﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheWebShop.Models
{
    internal class Customer
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public int ZipCode { get; set; }
        public int CityId { get; set; }
        public string Email { get; set; }
        public int PhoneNumber { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public int CreditCard { get; set; }
    }
}
