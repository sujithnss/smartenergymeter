using Newtonsoft.Json;
using SmartEnergyMeter.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace SmartEnergyMeterr.Utility
{
    public static class Utility
    {

        public static HttpCookie EncryptAndSet(CustomPrincipalSerializeModel serializeModel)
        {
            string userData = JsonConvert.SerializeObject(serializeModel);
            FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                     1,
                    serializeModel.FirstName + " " + serializeModel.LastName,
                     DateTime.Now,
                     DateTime.Now.AddMinutes(15),
                     false,
                     userData);

            string encTicket = FormsAuthentication.Encrypt(authTicket);
            HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
            return faCookie;
        }

        public static CustomPrincipalSerializeModel DecryptAndGetCustomPrincipal(HttpCookie authCookie)
        {
            CustomPrincipalSerializeModel serializeModel = new CustomPrincipalSerializeModel();
            if (authCookie != null)
            {
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);

                serializeModel = JsonConvert.DeserializeObject<CustomPrincipalSerializeModel>(authTicket.UserData);
            }
            return serializeModel;
        }
    }
}