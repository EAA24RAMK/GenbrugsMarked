using Core.Models;
using Microsoft.AspNetCore.Mvc;
using ServerAPI.Repositories;

namespace ServerAPI.Controllers;
[ApiController]
[Route("api/[controller]")]

public class UserController : ControllerBase
{
    private readonly UserRepository _userRepo;

    //Dependency-injection: UserRespository bliver injected ind i Controlleren.
    //Gør det muligt at kalde repository-metoder
    public UserController(UserRepository userRepo)
    {
        _userRepo = userRepo;
    }

    //Modtager en user som json og kalder createasync i UserRepository, for at oprette en ny bruger
    //Returnerer derefter den oprettede bruger
    [HttpPost("register")]
    public async Task<ActionResult<User>> Register(User user)
    {
        var newUser = await _userRepo.CreateAsync(user);
        return Ok(newUser);
    }

    //Finder brugeren baseret på email og password
    //Er brugeren korrekt, så finder den brugeren i DB
    [HttpGet("login")]
    public async Task<ActionResult<User>> Login([FromQuery] string email, [FromQuery] string password)
    {
        var user = await _userRepo.GetByEmailAndPasswordAsync(email, password);
        if (user == null) return Unauthorized("Forkert email eller kodeord.");
        return Ok(user);
    }

    //Modtager en annonce (sale) i body
    //TIlføjer annoncen til brugeren med det angivne id
    //returnerer det opdaterede bruger
    [HttpPost("{id}/sales")]
    public async Task<ActionResult<User>> AddSale(string id, [FromBody] Sale sale)
    {
        var updatedUser = await _userRepo.AddSaleToUserAsync(id, sale);
        if (updatedUser == null)
            return NotFound("Brugeren findes ikke.");
        return Ok(updatedUser);
    }

    //Henter alle annoncer med status 'aktiv' på aktive brugere
    //Bruges til market-siden
    [HttpGet("sales/active")]
    public async Task<ActionResult<List<Sale>>> GetActiveSales()
    {
        var sales = await _userRepo.GetAllActiveSalesAsync();
        return Ok(sales);
    }

    //Returner en liste over alle brugere
    //Bruges fx i MySales til at finde sælgeren/køberens navn
    [HttpGet]
    public async Task<ActionResult<List<User>>> GetAllUsers()
    {
        var users = await _userRepo.GetAllAsync();
        return Ok(users);
    }

    // API til at slette en annonce vha et sales id
    [HttpDelete("{userId}/sales/{salesId}")]
    public async Task<ActionResult<User>> DeleteSale(string userId, int salesId)
    {
        var updatedUser = await _userRepo.DeleteSaleAsync(userId, salesId);
        if (updatedUser == null)
            return NotFound();
        return Ok(updatedUser);
    }

    // API til at opdatere en annonce
    //Opdaterer titel, pris og beskrivelse
    [HttpPut("{userId}/sales")]
    public async Task<ActionResult<User>> UpdateSale(string userId, [FromBody] Sale updatedSale)
    {
        var updatedUser = await _userRepo.UpdateSaleAsync(userId, updatedSale);
        if (updatedUser == null)
            return NotFound();
        return Ok(updatedUser);
    }
}