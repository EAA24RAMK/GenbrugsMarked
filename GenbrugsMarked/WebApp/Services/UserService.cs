using System.Net.Http.Json;
using Core.Models;

namespace WebApp.Services;

public class UserService
{
    private readonly HttpClient _http;
    
    public UserService(HttpClient http)
    {
        _http = http;
    }
    
    public async Task<User?> LoginAsync(string email, string password)
    {
        var response = await _http.GetAsync($"api/user/login?email={email}&password={password}");
        if (response.IsSuccessStatusCode)
            return await response.Content.ReadFromJsonAsync<User>();
        return null;
    }

    public async Task<User> RegisterAsync(User user)
    {
        var response = await _http.PostAsJsonAsync("api/user/register", user);
        if (response.IsSuccessStatusCode)
            return await response.Content.ReadFromJsonAsync<User>();
        return null;
    }

    public async Task<User?> AddSaleAsync(string userId, Sale sale)
    {
        var response = await _http.PostAsJsonAsync($"api/user/{userId}/sales", sale);
        if (response.IsSuccessStatusCode)
            return await response.Content.ReadFromJsonAsync<User>();
        return null;
    }
}