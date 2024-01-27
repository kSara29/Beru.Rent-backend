﻿using Bff.Application.Contracts;
using Common;
using FastEndpoints;
using User.Dto;

namespace Bff.Api.Endpoints.UserService;

public class CreateUser(IUserService service) : Endpoint<CreateUserDto, ResponseModel<UserDtoResponce>>
{
    public override void Configure()
    {
        Post("/bff/user/createUser");
        AllowAnonymous();
    }
    
    public override async Task HandleAsync
        (CreateUserDto? request, CancellationToken ct)
    { 
        if (request is null) await SendAsync(null!, cancellation: ct);
        var response = await service.CreateUserAsync(request!);
        await SendAsync(response, cancellation: ct);
    }
}