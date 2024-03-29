﻿using System.Security.Claims;
using Ad.Dto.GetDtos;
using Bff.Application.Contracts;
using Common;
using FastEndpoints;

namespace Bff.Api.Endpoints.AdService;

public class GetAdsByUserId (IAdService service) : Endpoint<RequestById, ResponseModel<List<AdDto>>>
{
    public override void Configure()
    {
        Get("/bff/ad/getAllAdsByUserId");
        AllowAnonymous();
    }
    
    public override async Task HandleAsync
        (RequestById request, CancellationToken ct)
    { 
        if (request is null) await SendAsync(null!, cancellation: ct);
        
        if (string.IsNullOrWhiteSpace(request.Id.ToString()))
            request.Id = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var response = await service.GetAdsByUserIdAsync(request);
        await SendAsync(response, cancellation: ct);
    }
}
