using System;
using SmartEnergyMeter.Entities;
using System.Net.Http;
using System.Configuration;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Collections.Generic;

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

        public async Task<bool> ConfigureTariff(List<Tariff> tariff)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync(url + "tariff", tariff);
            return response.StatusCode == System.Net.HttpStatusCode.OK;
        }

        public async Task<List<ConsumptionLog>> ConsumptionLogGetById(string customerId)
        {
            List<ConsumptionLog> consumptionLogList = new List<ConsumptionLog>();
            HttpResponseMessage response = await client.GetAsync(url + "customer/log/"+customerId);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var customerData = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ConsumptionLog>>(data);
                if (customerData != null)
                {
                    foreach (ConsumptionLog consumptionLog in customerData)
                    {
                        ConsumptionLog tempConsumptionLog = new ConsumptionLog();
                        tempConsumptionLog.Id = consumptionLog.Id;
                        tempConsumptionLog.CustomerId = consumptionLog.CustomerId;
                        tempConsumptionLog.SmartEnergyMeterId = consumptionLog.SmartEnergyMeterId;
                        tempConsumptionLog.Unit = consumptionLog.Unit;
                        tempConsumptionLog.CreatedDateTime = consumptionLog.CreatedDateTime;
                       
                        consumptionLogList.Add(tempConsumptionLog);
                    }
                }
            }

            return consumptionLogList;
        }

        public async Task<List<Customer>> CustomerGetAll()
        {
            List<Customer> customerList = new List<Customer>();
            HttpResponseMessage response = await client.GetAsync(url + "customer/listall");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var customerData = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Customer>>(data);
                if (customerData != null)
                {
                    foreach (Customer customer in customerData)
                    {
                        Customer tempCustomer = new Customer();
                        tempCustomer.Id = customer.Id;
                        tempCustomer.Email = customer.Email;
                        tempCustomer.Name = customer.Name;
                        tempCustomer.AddressLine1 = customer.AddressLine1;
                        tempCustomer.AddressLine2 = customer.AddressLine2;
                        tempCustomer.PinCode = customer.PinCode;
                        customerList.Add(tempCustomer);
                    }
                }
            }

            return customerList;
        }

        public async Task<List<Tariff>> GetTariff()
        {
            List<Tariff> tariffList = new List<Tariff>();
            HttpResponseMessage response = await client.GetAsync(url + "tariff/get");
            if(response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var tariffdata = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Tariff>>(data);
                if (tariffdata != null)
                {
                    foreach (Tariff t in tariffdata)
                    {
                        Tariff tempTariff = new Tariff();
                        tempTariff.Id = t.Id;
                        tempTariff.Name = t.Name;
                        tempTariff.TariffType = t.TariffType;
                        tempTariff.Rate = t.Rate;
                        tariffList.Add(tempTariff);
                    }
                }
            }

            return tariffList;
        }

        public async Task<bool> SmartEnergyMeterInsert(Entities.SmartEnergyMeter smartEnergyMeter)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync(url + "smartem", smartEnergyMeter);
            return response.StatusCode == System.Net.HttpStatusCode.OK;
        }

        public async Task<List<Entities.SmartEnergyMeter>> SmartEnergyMeterList()
        {
            List<SmartEnergyMeter.Entities.SmartEnergyMeter> smartEmList = new List<SmartEnergyMeter.Entities.SmartEnergyMeter>();
            HttpResponseMessage response = await client.GetAsync(url + "smartem/get");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var smartData = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SmartEnergyMeter.Entities.SmartEnergyMeter>>(data);
                if (smartData != null)
                {
                    foreach (SmartEnergyMeter.Entities.SmartEnergyMeter t in smartData)
                    {
                        SmartEnergyMeter.Entities.SmartEnergyMeter temp = new SmartEnergyMeter.Entities.SmartEnergyMeter();
                        temp.Id = t.Id;
                        temp.CustomerId = t.CustomerId;
                        temp.TariffType = t.TariffType;

                        smartEmList.Add(temp);
                    }
                }
            }

            return smartEmList;
        }
    }
}