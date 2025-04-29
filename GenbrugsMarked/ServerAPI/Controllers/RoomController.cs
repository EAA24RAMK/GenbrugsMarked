using Core.Models;
using Microsoft.AspNetCore.Mvc;
using ServerAPI.Repositories;

namespace ServerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomController : ControllerBase
    {
        private readonly RoomRepository _roomRepo;
        
        //Dependency-Injection: RoomRepository bliver injected
        //GØr det muligt at kalde Repository-metoder i controllerens endpoints
        public RoomController(RoomRepository roomRepo)
        {
            _roomRepo = roomRepo;
        }
        
        //Hent alle lokaler, returnerer en liste over alle lokaler
        //Bruges når bruger skal vælge et lokal ved oprettelse af en annonce
        [HttpGet]
        public async Task<ActionResult<List<Room>>> GetAllRooms()
        {
            var rooms = await _roomRepo.GetAllAsync();
            return Ok(rooms);
        }

        //Hent specifik lokale via id, kalder GetByIdAsync i RoomRepository
        //Returnerer lokalet hvis det findes
        [HttpGet("{roomId}")]
        public async Task<ActionResult<Room>> GetRoomById(int roomId)
        {
            var room = await _roomRepo.GetByIdAsync(roomId);
            if (room == null)
                return NotFound();
            return Ok(room);
        }
    }
}