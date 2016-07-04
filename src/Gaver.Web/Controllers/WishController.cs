using System.Collections.Generic;
using System.Linq;
using Gaver.Data;
using Gaver.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Gaver.Web.Controllers
{
    [Route("api/[controller]")]
    public class WishController : Controller
    {
        private readonly GaverContext gaverContext;

        public WishController(GaverContext gaverContext) {
            this.gaverContext = gaverContext;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            var tableName = gaverContext.Model.GetEntityTypes().First().Relational().TableName;
            return gaverContext.Set<Wish>().Select(w => w.Title);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public Wish Post([FromBody]string title)
        {
            var wish = new Wish {
                Title = title
            };
            var entry = gaverContext.Set<Wish>().Add(wish);
            gaverContext.SaveChanges();
            return wish;
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
