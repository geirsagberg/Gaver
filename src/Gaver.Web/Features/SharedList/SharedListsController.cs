using System.Threading.Tasks;
using Gaver.Web.Features.SharedList.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Gaver.Web.Features.SharedList;

public class SharedListsController : GaverControllerBase
{
    private readonly IMediator mediator;

    public SharedListsController(IMediator mediator) => this.mediator = mediator;

    [HttpGet("{wishListId:int}")]
    public Task<SharedListDto> Get(GetSharedListRequest request) => mediator.Send(request);

    [HttpPut("{wishListId:int}/{wishId:int}/Bought")]
    public Task<SharedWishDto> SetBought(SetBoughtRequest request) => mediator.Send(request);

    [HttpGet("{wishListId:int}/Access")]
    public Task<ListAccessStatus> CheckSharedListAccess(CheckSharedListAccessRequest request) => mediator.Send(
        request);
}