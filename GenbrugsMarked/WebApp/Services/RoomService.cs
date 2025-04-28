using Core.Models;
using System.Net.Http.Json;

namespace Client.Services
{
    public class RoomService
    {
        private readonly HttpClient _http;

        public RoomService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<Room>> GetAllRoomsAsync()
        {
            return await _http.GetFromJsonAsync<List<Room>>("api/room");
        }
    }
}