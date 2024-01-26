using System.Runtime.InteropServices.JavaScript;
using Deal.Api.DTO.Booking;
using Deal.Application.Contracts.Booking;
using Deal.Application.DTO.Booking;
using FastEndpoints;
namespace Deal.Api.Endpoints;

public class GetAllBookings(IBookingService service): Endpoint<Guid>    
{
    public override void Configure()
    {
        Get("api/booking/getallbookings/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Guid id, CancellationToken ct)
    {
        var results = await service.GetAllBookingsAsync(id);
        if (results != null)
        {
        DateArray[] dateTimes = new DateArray[results.Length/2];
        for (int i = 0; i < results.Length/2; i++)
        {
            dateTimes[i] = new DateArray()
            {
                from = results[i, 0],
                to = results[i, 1]
            };
        }
        await SendAsync(dateTimes, cancellation: ct);
        }
        else
        {
            await SendAsync(new DateArray[0], cancellation: ct);
        }
    }
}