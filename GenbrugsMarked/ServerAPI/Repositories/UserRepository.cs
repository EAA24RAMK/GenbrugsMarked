using Core.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace ServerAPI.Repositories;

public class UserRepository
{
    private readonly IMongoCollection<User> _users;

    public UserRepository(IConfiguration config)
    {
        var client = new MongoClient(config["MongoDB:ConnectionString"]);
        var database = client.GetDatabase(config["MongoDB:DatabaseName"]);
        _users = database.GetCollection<User>("users");
    }
    
    public async Task<List<User>> GetAllAsync() =>
    await _users.Find(_ => true).ToListAsync();
    
    public async Task<User?> GetByEmailAndPasswordAsync(string email, string password) =>
    await _users.Find(u => u.Email == email && u.Password == password).FirstOrDefaultAsync();
    
    public async Task<User> CreateAsync(User user)
    {
        await _users.InsertOneAsync(user);
        return user;
    }
}