using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartEnergyMeter.Entities
{
    public class Tariff
    {
        public int Id { get; set; }
        public double Rate { get; set; }
        public string Name { get; set; }
        public TariffType TariffType { get; set; }
    }
}