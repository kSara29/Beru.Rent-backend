﻿using System.Security.Claims;
using Bff.Application.Contracts;
using Common;
using FastEndpoints;
using User.Dto;
using User.Dto.RequestDto;
using User.Dto.ResponseDto;

namespace Bff.Api.Endpoints.UserService;

public class GetUserById(IUserService service) : Endpoint<GetUserByIdRequest, ResponseModel<UserDtoResponce>>
{
    public override void Configure()
    {
        Get("/bff/user/getById");
        AllowAnonymous();
    }
    
    public override async Task HandleAsync
        (GetUserByIdRequest request, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.Id))
            request.Id = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        
        var response = await service.GetUserByIdAsync(request!.Id);
        await SendAsync(response, cancellation: ct);
    }
}