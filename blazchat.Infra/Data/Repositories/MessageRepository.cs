namespace blazchat.Infra.Data.Repositories;

public class MessageRepository : IMessageRepository
{
    private readonly IMongoCollection<Message> _chatMessages;

    public MessageRepository(IMongoClient mongoClient)
    {
        var database = mongoClient.GetDatabase("BlazChat");
        _chatMessages = database.GetCollection<Message>("Messages");
    }

    public async Task<List<Message>> GetMessagesByChatIdAsync(Guid chatId)
    {
        var filter = Builders<Message>.Filter.Eq(msg => msg.ChatId, chatId);
        var sort = Builders<Message>.Sort.Descending(msg => msg.Timestamp);
        return await _chatMessages.Find(filter).Sort(sort).ToListAsync();
    }

    public async Task AddMessageAsync(Message message)
    {
        if (message.ChatId == Guid.Empty)
            throw new ArgumentException("ChatId cannot be empty", nameof(message.ChatId));

        await _chatMessages.InsertOneAsync(message);
    }
}