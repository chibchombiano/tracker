using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Tracker.Util;

namespace Tracker.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        public List<Tracker.Util.Tracker> Get()
        {
            var repo = new ProjectRepository();
            var result = repo.listadoPuntos("355227042112508", DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString(), "14");
            return result;
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
