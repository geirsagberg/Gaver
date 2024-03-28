using Gaver.Web.Features.Shared.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Gaver.Web.Features.MyList;

public class MyListController(IMediator mediator) : GaverControllerBase {
    private readonly IMediator mediator = mediator;

    [HttpGet]
    public Task<MyListDto> GetMyList() => mediator.Send(new GetMyListRequest());

    [HttpPost]
    public Task<WishDto> Post(AddWishRequest request) => mediator.Send(request);

    [HttpPost("Order")]
    public Task SetWishesOrder(SetWishesOrderRequest request) => mediator.Send(request);

    [HttpPost("Share")]
    public Task ShareList(ShareListRequest request) => mediator.Send(request);

    [HttpPatch("{wishId:int}")]
    public Task UpdateWish(UpdateWishRequest request) => mediator.Send(request);

    [HttpDelete("{wishId:int}")]
    public Task<DeleteWishResponse> Delete(DeleteWishRequest request) => mediator.Send(request);

    [HttpPost("{wishId:int}/Option")]
    public Task<WishOptionDto> AddWishOption(AddWishOptionRequest request) => mediator.Send(request);

    [HttpPost("Reset")]
    public Task ResetList(ResetListRequest request) => mediator.Send(request);
}
