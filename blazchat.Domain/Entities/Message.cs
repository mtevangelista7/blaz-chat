using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace blazchat.Domain.Entities;

public class Message
{
    [BsonId] public ObjectId Id { get; set; }

    [BsonElement("ChatId")] public Guid ChatId { get; set; }

    [BsonElement("UserId")] public Guid UserId { get; set; }

    [BsonElement("Text")] public string Text { get; set; }

    [BsonElement("Timestamp")] public DateTime Timestamp { get; set; }
}