using Core.Models;
using Microsoft.AspNetCore.Mvc;
using ServerAPI.Repositories;

namespace ServerAPI.Controllers;
[ApiController]
[Route("api/[controller]")]

public class UserController : ControllerBase
{
    private readonly UserRepository _userRepo;

    public UserController(UserRepository userRepo)
    {
        _userRepo = userRepo;
    }

    [HttpPost("register")]
    public async Task<ActionResult<User>> Register(User user)
    {
        var newUser = await _userRepo.CreateAsync(user);
        return Ok(newUser);
    }

    [HttpGet("login")]
    public async Task<ActionResult<User>> Login([FromQuery] string email, [FromQuery] string password)
    {
        var user = await _userRepo.GetByEmailAndPasswordAsync(email, password);
        if (user == null) return Unauthorized("Forkert email eller kodeord.");
        return Ok(user);
    }

    [HttpPost("{id}/sales")]
    public async Task<ActionResult<User>> AddSale(string id, [FromBody] Sale sale)
    {
        var updatedUser = await _userRepo.AddSaleToUserAsync(id, sale);
        if (updatedUser == null)
            return NotFound("Brugeren findes ikke.");
        return Ok(updatedUser);
    }

    [HttpGet("sales/active")]
    public async Task<ActionResult<List<Sale>>> GetActiveSales()
    {
        var sales = await _userRepo.GetAllActiveSalesAsync();
        return Ok(sales);
    }

    [HttpGet]
    public async Task<ActionResult<List<User>>> GetAllUsers()
    {
        var users = await _userRepo.GetAllAsync();
        return Ok(users);
    }
}