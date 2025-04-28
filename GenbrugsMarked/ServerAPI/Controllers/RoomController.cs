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

        public RoomController(RoomRepository roomRepo)
        {
            _roomRepo = roomRepo;
        }

        [HttpGet]
        public async Task<ActionResult<List<Room>>> GetAllRooms()
        {
            var rooms = await _roomRepo.GetAllAsync();
            return Ok(rooms);
        }

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