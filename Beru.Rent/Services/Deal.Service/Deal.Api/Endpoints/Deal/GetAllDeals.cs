using Common;
using Deal.Application.Services;
using Deal.Dto.Booking;
using FastEndpoints;

namespace Deal.Api.Endpoints;

public class GetAllDeals(DealService _service): Endpoint<RequestByUserId,ResponseModel<List<GetAllDealsResponseDto>>>
{
    public override void Configure()
    {
        Get("api/booking/GetAllDeals");
        AllowAnonymous();
    }

    public override async Task HandleAsync(RequestByUserId Id, CancellationToken ct)
    {
        var results = await _service.GetAllDealsAsync(Id);
        await SendAsync(results, cancellation: ct);
    }
}