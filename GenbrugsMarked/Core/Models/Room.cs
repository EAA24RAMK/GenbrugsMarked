using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Core.Models
{
    public class Room
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } 

        public int RoomId { get; set; }
        public string Name { get; set; }
        public string Floor { get; set; }
        public string OpeningHours { get; set; }
    }
}