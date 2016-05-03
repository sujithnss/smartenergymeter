using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SmartEnergyMeter.Controllers
{
    public class SmartEMController : ApiController
    {
        // GET: api/SmartEM
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/SmartEM/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/SmartEM
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/SmartEM/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/SmartEM/5
        public void Delete(int id)
        {
        }
    }
}
