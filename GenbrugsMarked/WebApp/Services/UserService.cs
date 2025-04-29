using System.Net.Http.Json;
using Core.Models;

namespace WebApp.Services;

public class UserService
{
    private readonly HttpClient _http;
    
    //Dependency-injection: Httpclient bliver injiceret (kommer fra Program.cs i WebApp)
    // Bruges til at sende Http-requests direkte til vores API
    public UserService(HttpClient http)
    {
        _http = http;
    }
    
    //Returnerer en bruger hvis email+password matcher
    //Bruges på login
    public async Task<User?> LoginAsync(string email, string password)
    {
        var response = await _http.GetAsync($"api/user/login?email={email}&password={password}");
        if (response.IsSuccessStatusCode)
            return await response.Content.ReadFromJsonAsync<User>();
        return null;
    }

    //Opretter bruger, bruges på registrer siden
    public async Task<User> RegisterAsync(User user)
    {
        var response = await _http.PostAsJsonAsync("api/user/register", user);
        if (response.IsSuccessStatusCode)
            return await response.Content.ReadFromJsonAsync<User>();
        return null;
    }

    //Tilføj annonce
    //Tilføj ny annonce til en bruger
    //Bruges på MySales
    public async Task<User?> AddSaleAsync(string userId, Sale sale)
    {
        var response = await _http.PostAsJsonAsync($"api/user/{userId}/sales", sale);
        if (response.IsSuccessStatusCode)
            return await response.Content.ReadFromJsonAsync<User>();
        return null;
    }

    //Returnerer alle aktive annoncer på Marked-siden
    public async Task<List<Sale>> GetActiveSalesAsync()
    {
        var response = await _http.GetAsync("api/user/sales/active");
        if (response.IsSuccessStatusCode)
            return await response.Content.ReadFromJsonAsync<List<Sale>>();
        return new List<Sale>();
    }

    // --SKULLE HAVE VÆRET I purchaserequestservice--
    //Sender en købsanmodning
    //Bruges på Markedsiden
    public async Task<bool> SendPurchaseRequestAsync(PurchaseRequest request)
    {
        var response = await _http.PostAsJsonAsync("api/purchaserequest", request);
        return response.IsSuccessStatusCode;
    }
    
    // --SKULLE HAVE VÆRET I purchaserequestservice--
    //Henter alle sælgers anmodninger
    //Bruges på MySales-siden
    public async Task<List<PurchaseRequest>> GetRequestsForSellerAsync(string sellerId)
    {
        var response = await _http.GetAsync($"api/purchaserequest/seller/{sellerId}");
        if (response.IsSuccessStatusCode)
            return await response.Content.ReadFromJsonAsync<List<PurchaseRequest>>();
        return new List<PurchaseRequest>();
    }

    // --SKULLE HAVE VÆRET I purchaserequestservice--
    //Henter alle anmodninger fra en køber
    //Bruges på "Mine indkøb" siden
    public async Task<List<PurchaseRequest>> GetRequestsForBuyerAsync(string buyerId)
    {
        var response = await _http.GetAsync($"api/purchaserequest/buyer/{buyerId}");
        if (response.IsSuccessStatusCode)
            return await response.Content.ReadFromJsonAsync<List<PurchaseRequest>>();
        return new List<PurchaseRequest>();
    }

    //Hent alle brugere
    //Bruges til at vise køber/sælgers navn
    public async Task<List<User>> GetAllUsersAsync()
    {
        var response = await _http.GetAsync("api/user");
        if (response.IsSuccessStatusCode)
            return await response.Content.ReadFromJsonAsync<List<User>>();
        return new List<User>();
    }
    
    // --SKULLE HAVE VÆRET I purchaserequestservice--
    // Opdaterer status på en købsanmodning
    //Opdaterer status fra venter til accepteret/afvist
    public async Task<bool> UpdateRequestStatusAsync(string requestId, string newStatus)
    {
        var response = await _http.PutAsJsonAsync($"api/purchaserequest/{requestId}/status", newStatus);
        return response.IsSuccessStatusCode;
    }

    // --SKULLE HAVE VÆRET I purchaserequestservice--
    // Henter alle accepted sales (solgte annoncer) gennem api
    //Bruges til at vist "Solgt" badges
    public async Task<List<int>> GetAcceptedSalesIdsAsync()
    {
        var response = await _http.GetAsync("api/purchaserequest/accepted-sales");
        if (response.IsSuccessStatusCode)
            return await response.Content.ReadFromJsonAsync<List<int>>();
        return new List<int>();
    }
    
    // Finder en user og en annonce og sletter annoncen ved at kalde api
    //Bruges på MySales siden
    public async Task<User?> DeleteSaleAsync(string userId, int salesId)
    {
        var response = await _http.DeleteAsync($"api/user/{userId}/sales/{salesId}");
        if (response.IsSuccessStatusCode)
            return await response.Content.ReadFromJsonAsync<User>();
        return null;
    }

    // Opdaterer en annonce ved at kalde api
    //Opdaterer en annonces titel, pris og beskrivelse
    //Bruges på MySales, når en sælger gemmer en redigeret annonce
    public async Task<User?> UpdateSaleAsync(string userId, Sale updatedSale)
    {
        var response = await _http.PutAsJsonAsync($"api/user/{userId}/sales", updatedSale);
        if (response.IsSuccessStatusCode)
            return await response.Content.ReadFromJsonAsync<User>();
        return null;
    }
}