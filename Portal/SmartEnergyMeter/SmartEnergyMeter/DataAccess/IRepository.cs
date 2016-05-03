using SmartEnergyMeter.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartEnergyMeter.DataAccess
{
    public interface IRepository
    {
        bool AddCustomer(Customer customer);
        Customer AuthenticateCustomer(string email, string password);
        AdminUser AuthenticateAdminUser(string email, string password);
    }
}