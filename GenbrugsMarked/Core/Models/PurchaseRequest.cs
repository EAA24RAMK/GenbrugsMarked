using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Core.Models;

public class PurchaseRequest
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    
    public string BuyerUserId { get; set; } = null!;
    public string SellerUserId { get; set; } = null!;
    public int SalesId { get; set; }
    public DateTime Date { get; set; }
    public string Status { get; set; } = "Venter";
}