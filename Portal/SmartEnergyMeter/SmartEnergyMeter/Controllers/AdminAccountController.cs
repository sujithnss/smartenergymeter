using SmartEnergyMeter.API;
using SmartEnergyMeter.DataAccess;
using SmartEnergyMeter.Entities;
using SmartEnergyMeter.Models;
using SmartEnergyMeter.Security;
using SmartEnergyMeter.Utility;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;
using System.Linq;

namespace SmartEnergyMeter.Controllers
{
    public class AdminAccountController : Controller
    {
        IAPIClient client = new APIClient();

        public async Task<ActionResult> ManageCustomer(string returnUrl)
        {
            ViewBag.Message = "Lists the Customers";
            List<RegisterCustomerViewModel> customerList = new List<RegisterCustomerViewModel>();
            List<Customer> customers = await client.CustomerGetAll();
            foreach(Customer cust in customers)
            {
                RegisterCustomerViewModel temp = new RegisterCustomerViewModel();
                temp.Name = cust.Name;
                temp.Email = cust.Email;
                temp.AddressLine1 = cust.AddressLine1;
                temp.AddressLine2 = cust.AddressLine2;
                temp.PinCode = cust.PinCode;

                customerList.Add(temp);
            }
            return View(customerList);
        }

        public async Task<ActionResult> ConfigureTariff(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            List<Tariff> tariffList = await client.GetTariff();
            foreach (Tariff temp in tariffList)
            {
                if (temp.TariffType == TariffType.TOD)
                {
                    ViewData["TOD"] = temp.Rate;
                }
                if (temp.TariffType == TariffType.Normal)
                {
                    ViewData["Normal"] = temp.Rate;
                }
            }

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ConfigureTariff(TariffViewModels tariff)
        {
            if (ModelState.IsValid)
            {
                List<Tariff> updateTariffList = new List<Tariff>();
                Tariff dotTariff = new Tariff() { TariffType = TariffType.TOD , Rate = tariff.TOD};
                Tariff normalTariff = new Tariff() { TariffType = TariffType.Normal, Rate = tariff.Normal };

                updateTariffList.Add(dotTariff);
                updateTariffList.Add(normalTariff);

                if (await client.ConfigureTariff(updateTariffList)) 
                {
                   return RedirectToAction("Index", "Home");

                }
            }

            return View(tariff);
        }

        public async Task<ActionResult> ManageSmartEnergyMeter(string returnUrl)
        {
            ViewBag.Message = "Lists the Smart Energy Meters";
            List<SmartEnergyMeterViewModels> smartEMList = new List<SmartEnergyMeterViewModels>();
            List<SmartEnergyMeter.Entities.SmartEnergyMeter> smartList = await client.SmartEnergyMeterList();

            List<Customer> customerList = await client.CustomerGetAll();
            
            
            foreach (SmartEnergyMeter.Entities.SmartEnergyMeter smart in smartList)
            {
                SmartEnergyMeterViewModels temp = new SmartEnergyMeterViewModels();

                temp.Id = smart.Id;
                temp.CustomerName = customerList.Where(x => x.Id == smart.CustomerId).FirstOrDefault() != null ? customerList.Where(x => x.Id == smart.CustomerId).FirstOrDefault().Name : "";
                temp.TariffType = Enum.GetName(typeof(TariffType),smart.TariffType);
                
                smartEMList.Add(temp);
            }
            return View(smartEMList);
        }


        public async Task<ActionResult> RegisterSmartEnergyMeter()
        {
            List<Customer> customers = await client.CustomerGetAll();

            ViewBag.CustomerId = new SelectList(customers, "Id", "Name");

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> RegisterSmartEnergyMeter(SmartEnergyMeterViewModels model, FormCollection formcollection)
        {
            if(ModelState.IsValid)
            {
                SmartEnergyMeter.Entities.SmartEnergyMeter smartEM = new Entities.SmartEnergyMeter();
                smartEM.CustomerId = model.CustomerId;
                smartEM.Id = model.Id;
                smartEM.TariffType = Convert.ToInt32(formcollection["userTypeGrp"]);
                await client.SmartEnergyMeterInsert(smartEM);

                return RedirectToAction("ManageSmartEnergyMeter", "AdminAccount");
            }
            

            return View();
        }
    }
}
