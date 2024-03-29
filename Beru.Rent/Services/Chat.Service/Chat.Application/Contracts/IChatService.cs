﻿using Chat.Domain.Model;
using Chat.Dto.RequestDto;
using Chat.Dto.ResponseModel;
using Common;

namespace Chat.Application.Contracts;

public interface IChatService
{
    public Task<Domain.Model.Chat> CreateChatAsync(CreateChatRequest newChat);
    public Task<Domain.Model.Chat?> SaveMessageAsync(Guid ChatId, Domain.Model.Message message);
    public Task<ResponseModel<List<MessageDto>>?> GetMessagesByChatIdAsync(Guid chatId);
    public Task<List<string>> GetChatParticipants(Guid ChatId);
    public Task<ResponseModel<List<GetAllChatsResponse>>> GetAllChats(string UserId);
}