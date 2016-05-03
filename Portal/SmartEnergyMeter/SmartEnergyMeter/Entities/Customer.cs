﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartEnergyMeter.Entities
{
    public class Customer
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public int PinCode { get; set; }
    }
}