using Core.Models;
using System.Net.Http.Json;

namespace Client.Services
{
    public class RoomService
    {
        private readonly HttpClient _http;

        //Dependency-injection
        //Httpclient bliver injiceret (kommer fra Program.cs i WebApp)
        public RoomService(HttpClient http)
        {
            _http = http;
        }
        
        //Henter alle lokaler
        //Bruger metoden GetFromJsonAsync, som sender en HttpGet request
        //Modtager Json og laver det om til en liste
        //Returnerer liste med alle lokaler
        public async Task<List<Room>> GetAllRoomsAsync()
        {
            return await _http.GetFromJsonAsync<List<Room>>("api/room");
        }
    }
}