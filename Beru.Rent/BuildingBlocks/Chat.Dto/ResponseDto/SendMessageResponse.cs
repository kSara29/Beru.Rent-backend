﻿namespace Chat.Dto.ResponseModel;

public class SendMessageResponse
{
    public Guid Id { get; set; }
    public List<Guid> Participants { get; set; } = new List<Guid>();
    public DateTime CreatedAt { get; set; }
    public List<MessageDto> Messages { get; set; }
}