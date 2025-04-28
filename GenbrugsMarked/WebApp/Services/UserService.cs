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

    public async Task<List<Sale>> GetActiveSalesAsync()
    {
        var response = await _http.GetAsync("api/user/sales/active");
        if (response.IsSuccessStatusCode)
            return await response.Content.ReadFromJsonAsync<List<Sale>>();
        return new List<Sale>();
    }

    public async Task<bool> SendPurchaseRequestAsync(PurchaseRequest request)
    {
        var response = await _http.PostAsJsonAsync("api/purchaserequest", request);
        return response.IsSuccessStatusCode;
    }
    
    public async Task<List<PurchaseRequest>> GetRequestsForSellerAsync(string sellerId)
    {
        var response = await _http.GetAsync($"api/purchaserequest/seller/{sellerId}");
        if (response.IsSuccessStatusCode)
            return await response.Content.ReadFromJsonAsync<List<PurchaseRequest>>();
        return new List<PurchaseRequest>();
    }

    public async Task<List<PurchaseRequest>> GetRequestsForBuyerAsync(string buyerId)
    {
        var response = await _http.GetAsync($"api/purchaserequest/buyer/{buyerId}");
        if (response.IsSuccessStatusCode)
            return await response.Content.ReadFromJsonAsync<List<PurchaseRequest>>();
        return new List<PurchaseRequest>();
    }

    public async Task<List<User>> GetAllUsersAsync()
    {
        var response = await _http.GetAsync("api/user");
        if (response.IsSuccessStatusCode)
            return await response.Content.ReadFromJsonAsync<List<User>>();
        return new List<User>();
    }
    
    // Opdaterer status på en købsanmodning
    public async Task<bool> UpdateRequestStatusAsync(string requestId, string newStatus)
    {
        var response = await _http.PutAsJsonAsync($"api/purchaserequest/{requestId}/status", newStatus);
        return response.IsSuccessStatusCode;
    }

    // Henter alle accepted sales (solgte annoncer) gennem api
    public async Task<List<int>> GetAcceptedSalesIdsAsync()
    {
        var response = await _http.GetAsync("api/purchaserequest/accepted-sales");
        if (response.IsSuccessStatusCode)
            return await response.Content.ReadFromJsonAsync<List<int>>();
        return new List<int>();
    }
    
    // Finder en user og en annonce og sletter annoncen ved at kalde api
    public async Task<User?> DeleteSaleAsync(string userId, int salesId)
    {
        var response = await _http.DeleteAsync($"api/user/{userId}/sales/{salesId}");
        if (response.IsSuccessStatusCode)
            return await response.Content.ReadFromJsonAsync<User>();
        return null;
    }

    // Opdaterer en annonce ved at kalde api
    public async Task<User?> UpdateSaleAsync(string userId, Sale updatedSale)
    {
        var response = await _http.PutAsJsonAsync($"api/user/{userId}/sales", updatedSale);
        if (response.IsSuccessStatusCode)
            return await response.Content.ReadFromJsonAsync<User>();
        return null;
    }
}