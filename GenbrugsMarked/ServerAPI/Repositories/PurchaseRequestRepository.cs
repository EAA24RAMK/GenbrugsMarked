using Core.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace ServerAPI.Repositories;

public class PurchaseRequestRepository
{
    private readonly IMongoCollection<PurchaseRequest> _requests;

    //Forbinder til MongoDB baseret på en connectionstring i appsettings.json
    public PurchaseRequestRepository(IConfiguration config)
    {
        var client = new MongoClient(config["MongoDB:ConnectionString"]);
        var database = client.GetDatabase(config["MongoDB:DatabaseName"]);
        _requests = database.GetCollection<PurchaseRequest>("purchaseRequests");
    }

    //Opret ny købsanmodning, gemmer ny købsanmodning i databasen
    //bruges når bruger klikker "anmod om køb" på market
    public async Task<PurchaseRequest> CreateAsync(PurchaseRequest request)
    {
        await _requests.InsertOneAsync(request);
        return request;
    }

    //Hent anmodninger for en sælger
    //Finder alle anmodninger, hvor en bestemt bruger er sælger
    //Bruges på MySales, hvor sælgeren kan se sine modtagne anmodninger
    public async Task<List<PurchaseRequest>> GetBySellerIdAsync(string sellerId)
    {
        var filter = Builders<PurchaseRequest>.Filter.Eq(r => r.SellerUserId, sellerId);
        return await _requests.Find(filter).ToListAsync();
    }

    //Hent anmodninger for en køber
    //Finder alle anmodninger som en køber har sendt
    //Bruges på "Mine indkøb" siden
    public async Task<List<PurchaseRequest>> GetByBuyerIdAsync(string buyerId)
    {
        var filter = Builders<PurchaseRequest>.Filter.Eq(r => r.BuyerUserId, buyerId);
        return await _requests.Find(filter).ToListAsync();
    }
    
    // Update metode, opdaterer status til accepteret eller afslået
    //Finder en specifik anmodning via dens Id
    //Opdaterer dens status (venter, accepteret, afvist)
    public async Task<bool> UpdateStatusAsync(string requestId, string newStatus)
    {
        var filter = Builders<PurchaseRequest>.Filter.Eq(r => r.Id, requestId);
        var update = Builders<PurchaseRequest>.Update.Set(r => r.Status, newStatus);
        var result = await _requests.UpdateOneAsync(filter, update);
        return result.ModifiedCount == 1;
    }
    
    //Find alle accepterede anmodninger, med status accepteret
    //Returnerer SalesId, for de annoncer der nu er solgt
    //Viser "Solgt"-badge på annoncer på markedet
    public async Task<List<int>> GetAcceptedSalesIdsAsync()
    {
        var filter = Builders<PurchaseRequest>.Filter.Eq(r => r.Status, "Accepteret");
        var acceptedRequests = await _requests.Find(filter).ToListAsync();
        return acceptedRequests.Select(r => r.SalesId).ToList();
    }
}