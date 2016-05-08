using SmartEnergyMeter.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartEnergyMeter.API
{
    public interface IAPIClient
    {
        Task<bool> AddCustomer(Customer customer);
        Task<Customer> AuthenticateCustomer(Customer customerinput);
        Task<AdminUser> AuthenticateAdmin(AdminUser admin);
        Task<bool> ConfigureTariff(List<Tariff> tariff);
        Task<List<Tariff>> GetTariff();
        Task<List<Customer>> CustomerGetAll();
        Task<List<ConsumptionLog>> ConsumptionLogGetById(string customerId);
        Task<List<SmartEnergyMeter.Entities.SmartEnergyMeter>> SmartEnergyMeterList();
        Task<bool> SmartEnergyMeterInsert(SmartEnergyMeter.Entities.SmartEnergyMeter smartEnergyMeter);
    }
}