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
        bool ConfigureTariff(List<Tariff> tariffList);
        List<Tariff> GetTariff();
        List<Customer> ListCustomer();
        List<ConsumptionLog> ListConsumptionLog(string customerId);
        bool ConsumptionLogInsert(string smartEnergyMeterId, decimal unit);
        List<SmartEnergyMeter.Entities.SmartEnergyMeter> SmartEnergyMeterGet();
        bool InsertSmartEnergyMeter(SmartEnergyMeter.Entities.SmartEnergyMeter smaertEnergyMeter);
    }
}