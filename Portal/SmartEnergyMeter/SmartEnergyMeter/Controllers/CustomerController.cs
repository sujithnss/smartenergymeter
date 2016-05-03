using SmartEnergyMeter.DataAccess;
using SmartEnergyMeter.Entities;
using System.Web.Http;

namespace SmartEnergyMeter.Controllers
{
    public class CustomerController : ApiController
    {
        IRepository repository = new Repository();

        public CustomerController()
        {

        }

        // POST: api/Customer
        [HttpPost]
        [Route("api/customer")]
        public bool Post([FromBody]Customer customer)
        {
            return repository.AddCustomer(customer);
        }

        [HttpPost]
        [Route("api/customer/authenticate")]
        public Customer Authenticate([FromBody]Customer customer)
        {
            return repository.AuthenticateCustomer(customer.Email,customer.Password);
        }

        [HttpPost]
        [Route("api/admin/authenticate")]
        public AdminUser AuthenticateAdminUser([FromBody]Customer customer)
        {
            return repository.AuthenticateAdminUser(customer.Email, customer.Password);
        }
    }
}
