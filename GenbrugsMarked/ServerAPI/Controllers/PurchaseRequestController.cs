using Core.Models;
using Microsoft.AspNetCore.Mvc;
using ServerAPI.Repositories;

namespace ServerAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PurchaseRequestController : ControllerBase
{
        private readonly PurchaseRequestRepository _repo;

        public PurchaseRequestController(PurchaseRequestRepository repo)
        {
            _repo = repo;
        }
        
        [HttpPost]
        public async Task<ActionResult<PurchaseRequest>> Create(PurchaseRequest request)
        {
            request.Date = DateTime.Now;
            request.Status = "Venter";
            var result = await _repo.CreateAsync(request);
            return Ok(result);
        }

        [HttpGet("seller/{id}")]
        public async Task<ActionResult<List<PurchaseRequest>>> GetBySellerId(string id)
        {
            var requests = await _repo.GetBySellerIdAsync(id);
            return Ok(requests);
        }
}