using System;
using SmartEnergyMeter.Entities;
using System.Net.Http;
using System.Configuration;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace SmartEnergyMeter.API
{
    public class APIClient : IAPIClient
    {
        HttpClient client;
        string url = ConfigurationManager.AppSettings["APIUrl"].ToString();
        public APIClient()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        public async Task<bool> AddCustomer(Customer customer)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync(url + "Customer", customer);
            return response.StatusCode == System.Net.HttpStatusCode.OK;
        }

        public async Task<AdminUser> AuthenticateAdmin(AdminUser admininput)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync(url + "admin/Authenticate", admininput);
            AdminUser admin = new AdminUser();
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var customerdata = Newtonsoft.Json.JsonConvert.DeserializeObject<Customer>(data);
                if (customerdata != null)
                {
                    admin.Id = Convert.ToInt32( customerdata.Id);
                    admin.Email = customerdata.Email;
                }
            }

            return admin;
        }

        public async Task<Customer> AuthenticateCustomer(Customer customerinput)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync(url + "Customer/Authenticate", customerinput);
            Customer customer = new Customer();
            if(response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var customerdata = Newtonsoft.Json.JsonConvert.DeserializeObject<Customer>(data);
                if (customerdata != null)
                {
                    customer.Id = customerdata.Id;
                    customer.Name = customerdata.Name;
                    customer.Email = customerdata.Email;
                    customer.Password = customerdata.Password;
                    customer.AddressLine1 = customerdata.AddressLine1;
                    customer.AddressLine2 = customerdata.AddressLine2;
                    customer.PinCode = customerdata.PinCode;
                }
            }

            return customer;
           
        }
    }
}