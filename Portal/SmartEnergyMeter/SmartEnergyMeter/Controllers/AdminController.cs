using SmartEnergyMeter.DataAccess;
using SmartEnergyMeter.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SmartEnergyMeter.Controllers
{
    public class AdminController : ApiController
    {
        IRepository repository = new Repository();

        public AdminController() { }

        // POST: api/Customer
        [HttpPost]
        [Route("api/tariff")]
        public bool Post([FromBody]List<Tariff> tariffList)
        {
            return repository.ConfigureTariff(tariffList);
        }

        [HttpGet]
        [Route("api/tariff/get")]
        public List<Tariff> GetTariff()
        {
            return repository.GetTariff();
        }

        [HttpGet]
        [Route("api/smartem/get")]
        public List<SmartEnergyMeter.Entities.SmartEnergyMeter> ListSmartEnergyMeters()
        {
            return repository.SmartEnergyMeterGet();
        }

        [HttpPost]
        [Route("api/smartem")]
        public bool Post([FromBody]SmartEnergyMeter.Entities.SmartEnergyMeter smartEM)
        {
            return repository.InsertSmartEnergyMeter(smartEM);
        }
    }
}
