﻿using Bff.Application.Contracts;
using Common;
using Deal.Dto.Booking;
using FastEndpoints;

namespace Bff.Api.Endpoints.DealService;

public class CreateBooking(IDealServiceCreate serviceCreate) : Endpoint<CreateBookingDto, ResponseModel<BookingDto>>
{
    public override void Configure()
    {
        Post("/bff/deal/createBooking");
        AllowAnonymous();
    }
    
    public override async Task HandleAsync
        (CreateBookingDto? request, CancellationToken ct)
    { 
        if (request is null) await SendAsync(null!, cancellation: ct);
        var response = await serviceCreate.CreateBookingAsync(request!);
        await SendAsync(response, cancellation: ct);
    }
}