using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartEnergyMeter.Entities
{
    public enum UserTypes
    {
        Customer = 1 ,
        Admin = 2
    }

    public enum TariffType
    {
        TOD =1,
        Normal = 2
    }
}