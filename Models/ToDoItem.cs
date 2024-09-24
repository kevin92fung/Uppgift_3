using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ToDoApp.Models;
public class ToDoItem
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("Title")]
    public string? Title { get; set; }

    [BsonElement("IsCompleted")]
    public bool IsCompleted { get; set; } = false;

    [BsonElement("Details")]
    public string? Details { get; set; }
}
