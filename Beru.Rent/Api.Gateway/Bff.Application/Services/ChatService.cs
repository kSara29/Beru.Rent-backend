﻿using Bff.Application.Contracts;
using Bff.Application.Handlers;
using Bff.Application.JsonOptions;
using Chat.Dto.RequestDto;
using Chat.Dto.ResponseModel;
using Common;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Bff.Application.Services;

public class ChatService(ServiceHandler serviceHandler,IOptions<RequestToChatApi> jsonOptions, ILogger<ChatService> logger): IChatService
{
    public async Task<ResponseModel<ChatDtoResponse>> CreateChatAsync(CreateChatRequest request)
    {
        var url = serviceHandler.CreateConnectionUrlWithoutQuery(jsonOptions.Value.Url, "api/chat/create");
        
        logger.LogInformation("Отправка POST запроса {@url}, данные {@request}", url, request);
        return await serviceHandler.PostConnectionHandler<CreateChatRequest, ChatDtoResponse>(url, request);
    }

    public async Task<ResponseModel<SendMessageResponse>> SendMessageAsync(SendMessageRequest request)
    {
        var url = serviceHandler.CreateConnectionUrlWithoutQuery(jsonOptions.Value.Url, "api/chat/send");
        
        logger.LogInformation("Отправка POST запроса {@url}, данные {@request}", url, request);
        return await serviceHandler.PostConnectionHandler<SendMessageRequest, SendMessageResponse>(url, request);
    }

    public async Task<ResponseModel<List<MessageDto>>> LoadChatHistoryAsync(RequestById request, string userId)
    {
        //var url = serviceHandler.CreateConnectionUrlWithQuery(jsonOptions.Value.Url, "api/chat/history/", request.Id.ToString());
        var url = $"{jsonOptions.Value.Url}api/chat/history/{request.Id}/{userId}";
        return await serviceHandler.GetConnectionHandler<List<MessageDto>>(url);
    }

    public async Task<ResponseModel<List<GetAllChatsResponse>>> GetAllChatsByUserId(GetChatsByIdRequest request)
    {
        var url = $"{jsonOptions.Value.Url}api/chat/chats/{request.Id}";
        return await serviceHandler.GetConnectionHandler<List<GetAllChatsResponse>>(url);
    }
}