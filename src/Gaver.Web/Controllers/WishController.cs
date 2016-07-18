using System.Collections.Generic;
using Gaver.Data;
using Gaver.Data.Entities;
using Gaver.Logic;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Gaver.Web.Controllers
{
    [Route("api/[controller]")]
    public class WishController : Controller
    {
        private readonly GaverContext gaverContext;
        private readonly IMailSender mailSender;

        public WishController(GaverContext gaverContext, IMailSender mailSender) {
            this.gaverContext = gaverContext;
            this.mailSender = mailSender;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<Wish> Get()
        {
            return gaverContext.Set<Wish>();
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
            gaverContext.Delete<Wish>(id);
            gaverContext.SaveChanges();
            
        }

        [HttpPost("Share")]
        public void ShareList(string[] emails) {
            var mail = new Mail {
                To = emails,
                From = "noreply@sagberg.net",
                Subject = "Noen har delt en ønskeliste med deg",
                Content = @"<h1>Noen har delt en ønskeliste med deg!</h1>
                <p><a href='http://localhost/5000'>Klikk her for å se listen.</a></p>" 
            };
            mailSender.Send(mail);
        }
    }
}
