using SmartEnergyMeter.Entities;
using System.Threading.Tasks;

namespace SmartEnergyMeter.API
{
    public interface IAPIClient
    {
        Task<bool> AddCustomer(Customer customer);
        Task<Customer> AuthenticateCustomer(Customer customerinput);
        Task<AdminUser> AuthenticateAdmin(AdminUser admin);
    }
}