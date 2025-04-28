using Core.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace ServerAPI.Repositories
{
    public class RoomRepository
    {
        private readonly IMongoCollection<Room> _roomCollection;

        public RoomRepository(IConfiguration config)
        {
            var client = new MongoClient(config["MongoDB:ConnectionString"]);
            var database = client.GetDatabase(config["MongoDB:DatabaseName"]);
            _roomCollection = database.GetCollection<Room>("rooms");
        }

        public async Task<List<Room>> GetAllAsync()
        {
            return await _roomCollection.Find(_ => true).ToListAsync();
        }

        public async Task<Room> GetByIdAsync(int roomId)
        {
            return await _roomCollection.Find(r => r.RoomId == roomId).FirstOrDefaultAsync();
        }
    }
}