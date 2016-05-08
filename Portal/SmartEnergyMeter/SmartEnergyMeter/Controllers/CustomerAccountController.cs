using SmartEnergyMeter.API;
using SmartEnergyMeter.Entities;
using SmartEnergyMeter.Models;
using SmartEnergyMeter.Security;
using SmartEnergyMeter.Utility;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;

namespace SmartEnergyMeter.Controllers
{
    public class CustomerAccountController : Controller
    {
        IAPIClient client = new APIClient();
        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterCustomerViewModel customer)
        {
            if (ModelState.IsValid)
            {
                if(await client.AddCustomer(new Customer() { Id= Guid.NewGuid().ToString(),Email = customer.Email, Name = customer.Name, Password = customer.Password, AddressLine1 = customer.AddressLine1, AddressLine2 = customer.AddressLine2, PinCode = customer.PinCode }))
                {
                    LoginViewModel loginModel = new LoginViewModel();
                    loginModel.Email = customer.Email;
                    loginModel.Password = customer.Password;
                    var authcustomer = await client.AuthenticateCustomer(new Customer() { Email=customer.Email, Password = customer.Password});
                    if (authcustomer != null)
                    {
                        CustomPrincipalSerializeModel serializeModel = new CustomPrincipalSerializeModel();
                        serializeModel.CustomerID = authcustomer.Id;
                        serializeModel.FirstName = authcustomer.Name;
                        serializeModel.Email = authcustomer.Email;

                        Response.Cookies.Add(Utility.Utility.EncryptAndSet(serializeModel));
                        return RedirectToAction("Index", "Home");

                    }

                }
            }

            return View(customer);
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl,FormCollection formcollection)
        {
            if (ModelState.IsValid)
            {
                var customer = new Customer();
                var adminUser = new AdminUser();
                if (formcollection["userTypeGrp"].Equals("1"))
                {
                    customer = await client.AuthenticateCustomer(new Customer() { Email = model.Email, Password = model.Password });
                }
                else
                {
                    adminUser = await client.AuthenticateAdmin(new AdminUser() { Email = model.Email, Password = model.Password });
                }
                

                if (customer != null && !string.IsNullOrEmpty(customer.Id))
                {
                    CustomPrincipalSerializeModel serializeModel = new CustomPrincipalSerializeModel();
                    serializeModel.CustomerID = customer.Id;
                    serializeModel.FirstName = customer.Name;
                    serializeModel.Email = customer.Email;
                    serializeModel.UserTyepId = Convert.ToInt16(UserTypes.Customer);

                    Response.Cookies.Add(Utility.Utility.EncryptAndSet(serializeModel));
                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        return Redirect("../" + returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("ConsumptionLog", "CustomerAccount");
                    }


                }

                if (adminUser != null && adminUser.Id > 0)
                {
                    CustomPrincipalSerializeModel serializeModel = new CustomPrincipalSerializeModel();
                    serializeModel.CustomerID = adminUser.Id.ToString();
                    serializeModel.FirstName = "admin";
                    serializeModel.Email = adminUser.Email;
                    serializeModel.UserTyepId = Convert.ToInt16(UserTypes.Admin);

                    Response.Cookies.Add(Utility.Utility.EncryptAndSet(serializeModel));
                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        return Redirect("../" + returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("ManageCustomer", "AdminAccount");
                    }


                }

                ModelState.AddModelError("", "Incorrect username and/or password");
            }

            return View(model);
        }


        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }


        [AllowAnonymous]
        public ActionResult ViewBill(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            return View();
        }

        [AllowAnonymous]
        public async Task<ActionResult> ConsumptionLog(string returnUrl)
        {
            ViewBag.Message = "Consumption Log History";
            List<ConsumptionLogViewModel> customerList = new List<ConsumptionLogViewModel>();

            List<ConsumptionLog> consumptionLogs = await client.ConsumptionLogGetById(Utility.Utility.DecryptAndGetCustomPrincipal(Request.Cookies[FormsAuthentication.FormsCookieName]).CustomerID);
            foreach (ConsumptionLog cons in consumptionLogs)
            {
                ConsumptionLogViewModel temp = new ConsumptionLogViewModel();
                temp.Id = cons.Id;
                temp.CustomerId = cons.CustomerId;
                temp.SmartEnergyMeterId = cons.SmartEnergyMeterId;
                temp.Unit = cons.Unit;
                temp.CreatedDateTime = cons.CreatedDateTime;

                customerList.Add(temp);
            }
            return View(customerList);
        }



    }
}