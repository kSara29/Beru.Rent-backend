using Deal.Application.Contracts.Deal;
using Deal.Dto.Deal;
using FastEndpoints;

namespace Deal.Api.Endpoints;

public class CreateDeal(IDealService _service): Endpoint<Guid, CreateDealDto>
{
    public override void Configure()
    {
        Post("api/booking/createDeal/{idbooking}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Guid idbooking, CancellationToken ct)
    {
        var result = await _service.CreateDealAsync(idbooking);
        SendAsync(result);
    }
}