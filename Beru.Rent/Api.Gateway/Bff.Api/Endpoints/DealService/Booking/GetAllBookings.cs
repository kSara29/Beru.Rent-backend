using Bff.Application.Contracts;
using Common;
using Deal.Dto.Booking;
using FastEndpoints;

namespace Bff.Api.Endpoints.DealService;

public class GetAllBookings(IBookingService _service) : Endpoint<RequestByUserId, ResponseModel<List<GetAllBookingsResponseDto>>>
{
    public override void Configure()
    {
        Get("/bff/deal/getallbookings/{id}");
        AllowAnonymous();
    }
    
    public override async Task HandleAsync
        (RequestByUserId? id, CancellationToken ct)
    { 
        if (id is null) await SendAsync(null!, cancellation: ct);
        var response = await _service.GetAllBookingsAsync(id!);
        await SendAsync(response, cancellation: ct);
    }
}