﻿using Chat.Application.Contracts;
using Chat.Domain.Model;
using Chat.Dto.RequestDto;
using MongoDB.Driver;

namespace Chat.Infrastructure.Database;

public class ChatRepository: IChatRepository
{
    private readonly IMongoCollection<Domain.Model.Chat> _chatCollection;
    
    public ChatRepository(IChatDatabaseSettings settings, IMongoClient mongoClient)
    {
        var database = mongoClient.GetDatabase(settings.DatabaseName);
        _chatCollection = database.GetCollection<Domain.Model.Chat>(settings.ChatCollectionName);
    }
    
    public async Task<Domain.Model.Chat> CreateChatAsync(CreateChatRequest newChat)
    {
        var chatId = Guid.NewGuid();
        
        var initNewChat = new Domain.Model.Chat()
        {
            CreatedAt = DateTime.UtcNow,
            Participants = new List<Guid>() { newChat.User1, newChat.User2 },
            Id = chatId,
            Messages = []
        };
        try
        {
            await _chatCollection.InsertOneAsync(initNewChat);
            var response = await _chatCollection.Find(x => x.Id == chatId).FirstOrDefaultAsync();
            return response;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при вставке: {ex.Message}");
            return null;
        }
    }

    public async Task<Domain.Model.Chat> SaveMessageAsync(Guid chatId, Message message)
    {
        var filter = Builders<Domain.Model.Chat>.Filter.Eq(chat => chat.Id, chatId); 
        var update = Builders<Domain.Model.Chat>.Update.Push(chat => chat.Messages, message); 

        await _chatCollection.UpdateOneAsync(filter, update);
        var response = await _chatCollection.Find(x => x.Id == chatId).FirstOrDefaultAsync();

        return response;

    }

    public async Task<List<Message>> GetMessagesByChatIdAsync(Guid chatId)
    {
        var chat = await _chatCollection.Find(x => x.Id == chatId).FirstOrDefaultAsync();
        var messageHistory = chat.Messages;
        return messageHistory;
    }
}