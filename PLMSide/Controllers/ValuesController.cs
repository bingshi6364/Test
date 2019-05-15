using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace PLMSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            var name = User.FindFirst(x => x.Type == ClaimTypes.Role);
            string role = name.Value;
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            //  var name = User.Claims.Select(a=>)().Value;
            //  string name2 = User.Identity.Name;
            var name = User.FindFirst(x => x.Type == ClaimTypes.Name);
            string role = name.Value;
            return "value";
        }

        // GET api/values/5
        [HttpPost("name")]
        public ActionResult<string> GetName([FromBody] test aa)
        {
            string d = aa.name;
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }

    public class test
    {
       public string name { get; set; }
    }
}
