using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Gaver.Data;
using Gaver.Data.Entities;
using Gaver.Logic;
using Gaver.Web.Features.WishList;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Infrastructure;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Gaver.Web.Controllers
{
    [Route("api/[controller]")]
    public class WishController : Controller
    {
        private readonly GaverContext gaverContext;
        private readonly IMailSender mailSender;
        private readonly IMediator mediator;
        private readonly IHubContext hub;

        public WishController(GaverContext gaverContext, IMailSender mailSender, IMediator mediator, IConnectionManager signalRManager)
        {
            this.gaverContext = gaverContext;
            this.mailSender = mailSender;
            this.mediator = mediator;
            hub = signalRManager.GetHubContext<ListHub>();
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
            var wish = new Wish
            {
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
            hub.Clients.All.hello("World!");
        }

        [HttpPost("Share")]
        public async Task ShareList([FromBody]ShareListRequest request)
        {
            await mediator.SendAsync(request);
        }
    }
}
