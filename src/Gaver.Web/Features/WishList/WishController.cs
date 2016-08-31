using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gaver.Data;
using Gaver.Data.Entities;
using Gaver.Logic;
using Gaver.Web.Features.WishList;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Infrastructure;

namespace Gaver.Web.Features.WishList
{
    [Route("api/[controller]")]
    public class WishController : Controller
    {
        private readonly GaverContext gaverContext;
        private readonly IMailSender mailSender;
        private readonly IMediator mediator;
        private readonly IHubContext<ListHub, IListHubClient> hub;

        public WishController(GaverContext gaverContext, IMailSender mailSender, IMediator mediator, IConnectionManager signalRManager)
        {
            this.gaverContext = gaverContext;
            this.mailSender = mailSender;
            this.mediator = mediator;
            hub = signalRManager.GetHubContext<ListHub, IListHubClient>();
        }

        [HttpGet]
        public IEnumerable<Wish> Get()
        {
            return mediator.Send(new GetWishesRequest());
        }

        [HttpPost]
        public Wish Post([FromBody]string title)
        {
            var wish = new Wish
            {
                Title = title
            };
            gaverContext.Set<Wish>().Add(wish);
            gaverContext.SaveChanges();
            RefreshData();
            return wish;
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string title)
        {
            var wish = gaverContext.GetOrDie<Wish>(id);
            wish.Title = title;
            gaverContext.SaveChanges();
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            gaverContext.Delete<Wish>(id);
            gaverContext.SaveChanges();
            RefreshData();
        }

        [HttpPost("Share")]
        public async Task ShareList(ShareListRequest request)
        {
            await mediator.SendAsync(request);
        }

        private void RefreshData() => hub.Clients.Group(ListHub.ListGroup).Refresh(Get());
    }
}
