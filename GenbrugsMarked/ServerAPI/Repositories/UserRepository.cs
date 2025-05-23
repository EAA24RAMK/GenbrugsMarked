using Core.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ServerAPI.Repositories;

public class UserRepository 
{
    private readonly IMongoCollection<User> _users;

    //Opretter forbindelse til mongodb via appsettings.json
    public UserRepository(IConfiguration config) 
    {
        var client = new MongoClient(config["MongoDB:ConnectionString"]);
        var database = client.GetDatabase(config["MongoDB:DatabaseName"]);
        _users = database.GetCollection<User>("users");
    }
    
    //Returnerer alle brugere i databasen. Dette bruges eksempelvis hvis man skal slå brugernavne op
    public async Task<List<User>> GetAllAsync() =>
    await _users.Find(_ => true).ToListAsync();
    
    //Bruges til login, matcher email+password for at finde en bruger
    public async Task<User?> GetByEmailAndPasswordAsync(string email, string password) =>
    await _users.Find(u => u.Email == email && u.Password == password).FirstOrDefaultAsync();
    
    //Opretter en ny bruger i databasen
    public async Task<User> CreateAsync(User user)
    {
        await _users.InsertOneAsync(user);
        return user;
    }

    //Tilføjer ny annonce til brugerens embedded sale
    public async Task<User?> AddSaleToUserAsync(string userId, Sale sale)
    {
        var filter = Builders<User>.Filter.Eq(u => u.Id, userId);
        var update = Builders<User>.Update.Push(u => u.Sales, sale);

        var result = await _users.FindOneAndUpdateAsync(filter, update, new FindOneAndUpdateOptions<User>
        {
            ReturnDocument = ReturnDocument.After
        });
        return result;
    }

    //Returner alle aktive annoncer på marked siden
    public async Task<List<Sale>> GetAllActiveSalesAsync()
    {
        var users = await _users.Find(_ => true).ToListAsync();
        var allSales = users
            .SelectMany(u => u.Sales)
            .Where(s => s.Status == "Aktiv")
            .ToList();
        
        return allSales;
    }
    
    // Fjerner en annonce fra brugerens sales-liste, MySales siden
    public async Task<User?> DeleteSaleAsync(string userId, int salesId)
    {
        var filter = Builders<User>.Filter.Eq(u => u.Id, userId);
        var update = Builders<User>.Update.PullFilter(u => u.Sales, s => s.SalesId == salesId);

        var result = await _users.FindOneAndUpdateAsync(filter, update, new FindOneAndUpdateOptions<User>
        {
            ReturnDocument = ReturnDocument.After
        });
        
        return result;
    }
    
    // Update metode til at redigere i annoncer (title, description, price)
    // sales[-1] referer til det matchende element i array'et (mongodb)
    public async Task<User?> UpdateSaleAsync(string userId, Sale updatedSale)
    {
        var filter = Builders<User>.Filter.Eq(u => u.Id, userId);

        var update = Builders<User>.Update
            .Set("Sales.$[sale].Title", updatedSale.Title)
            .Set("Sales.$[sale].Description", updatedSale.Description)
            .Set("Sales.$[sale].Price", updatedSale.Price);

        var options = new FindOneAndUpdateOptions<User>
        {
            ReturnDocument = ReturnDocument.After,
            ArrayFilters = new List<ArrayFilterDefinition>
            {
                new BsonDocumentArrayFilterDefinition<BsonDocument>(
                    new BsonDocument("sale.SalesId", updatedSale.SalesId)
                )
            }
        };

        var result = await _users.FindOneAndUpdateAsync(filter, update, options);
        return result;
    }
}