using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartEnergyMeter.Entities
{
    public class ConsumptionLog
    {
        public int Id { get; set; }
        public string CustomerId { get; set; }
        public string SmartEnergyMeterId { get; set; }
        public decimal Unit { get; set; }
        public DateTime CreatedDateTime { get; set; }
    }
}