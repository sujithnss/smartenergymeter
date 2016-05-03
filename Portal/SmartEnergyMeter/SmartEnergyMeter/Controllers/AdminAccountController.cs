using System.Web.Http;
using System.Web.Mvc;

namespace SmartEnergyMeter.Controllers
{
    public class AdminAccountController : Controller
    {
       
        public ActionResult ManageCustomer(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            return View();
        }

        public ActionResult ConfigureTariif(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            return View();
        }

        public ActionResult GenerateBill(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            return View();
        }
    }
}
