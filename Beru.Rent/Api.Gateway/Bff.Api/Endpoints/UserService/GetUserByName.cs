﻿using Bff.Application.Contracts;
using Common;
using FastEndpoints;
using User.Dto;
using User.Dto.RequestDto;
using User.Dto.ResponseDto;

namespace Bff.Api.Endpoints.UserService;

public class GetUserByName(IUserService service, ILogger<GetUserByName> logger) : Endpoint<GetUserByUserNameRequest, ResponseModel<UserDtoResponce>>
{
    public override void Configure()
    {
        Get("/bff/user/getByName");
        AllowAnonymous();
    }
    
    public override async Task HandleAsync
        (GetUserByUserNameRequest? request, CancellationToken ct)
    { 
        if (request is null) await SendAsync(null!, cancellation: ct);
        var response = await service.GetUserByNameAsync(request!.UserName);
        
        logger.LogInformation("Ответ от userService: {@reposnse}", response);
        await SendAsync(response, cancellation: ct);
    }
}