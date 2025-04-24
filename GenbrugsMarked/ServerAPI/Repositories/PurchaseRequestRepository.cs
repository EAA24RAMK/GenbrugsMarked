using Core.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace ServerAPI.Repositories;

public class PurchaseRequestRepository
{
    private readonly IMongoCollection<PurchaseRequest> _requests;

    public PurchaseRequestRepository(IConfiguration config)
    {
        var client = new MongoClient(config["MongoDB:ConnectionString"]);
        var database = client.GetDatabase(config["MongoDB:DatabaseName"]);
        _requests = database.GetCollection<PurchaseRequest>("purchaseRequests");
    }

    public async Task<PurchaseRequest> CreateAsync(PurchaseRequest request)
    {
        await _requests.InsertOneAsync(request);
        return request;
    }

    public async Task<List<PurchaseRequest>> GetBySellerIdAsync(string sellerId)
    {
        var filter = Builders<PurchaseRequest>.Filter.Eq(r => r.SellerUserId, sellerId);
        return await _requests.Find(filter).ToListAsync();
    }

    public async Task<List<PurchaseRequest>> GetByBuyerIdAsync(string buyerId)
    {
        var filter = Builders<PurchaseRequest>.Filter.Eq(r => r.BuyerUserId, buyerId);
        return await _requests.Find(filter).ToListAsync();
    }
}