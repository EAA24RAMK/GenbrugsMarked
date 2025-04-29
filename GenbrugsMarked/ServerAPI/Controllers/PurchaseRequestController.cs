using Core.Models;
using Microsoft.AspNetCore.Mvc;
using ServerAPI.Repositories;

namespace ServerAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PurchaseRequestController : ControllerBase
{
        private readonly PurchaseRequestRepository _repo;

        //Dependency-injection
        //PurchaseRequestRepository bliver injectec
        //Gør det muligt at kalde Repository-metoder i controllerens endpoints
        public PurchaseRequestController(PurchaseRequestRepository repo)
        {
            _repo = repo;
        }
        
        //Opret ny købsanmodning, modtager en ny købsanmodning som json fra frontend
        //Sætter standardværdierne (dato, status)
        //Gemmer anmodning i Databasen
        [HttpPost]
        public async Task<ActionResult<PurchaseRequest>> Create(PurchaseRequest request)
        {
            request.Date = DateTime.Now;
            request.Status = "Venter";
            var result = await _repo.CreateAsync(request);
            return Ok(result);
        }
        
        //Hent anmodninger til en sælger
        //Finder alle anmodninger hvor den angivne bruger er sælger
        //Bruger på "Mine annoncer"-siden, for at vise hvem der har anmodet om køb
        [HttpGet("seller/{id}")]
        public async Task<ActionResult<List<PurchaseRequest>>> GetBySellerId(string id)
        {
            var requests = await _repo.GetBySellerIdAsync(id);
            return Ok(requests);
        }
        
        //Hent anmodninger fra en køber
        //Finder alle anmodninger fra en bestemt køber
        //Bruges på "Mine indkøb"-siden, for at vise egne anmodninger
        [HttpGet("buyer/{id}")]
        public async Task<ActionResult<List<PurchaseRequest>>> GetByBuyer(string id)
        {
            var requests = await _repo.GetByBuyerIdAsync(id);
            return Ok(requests);
        }
        
        //Opdaterer status på en anmodning
        //Opdaterer status til venter, accepteret eller afvist
        //Bruges når sælger trykker accepter eller afslå på en anmodning
        [HttpPut("{id}/status")]
        public async Task<ActionResult> UpdateStatus(string id, [FromBody] string newStatus)
        {
            var updated = await _repo.UpdateStatusAsync(id, newStatus);
            if (!updated)
                return NotFound();
            return NoContent();
        }
        
        //Hent alle accepterede salg
        //Finder alle sales-id på annoncer, hvor en købsanmodning er blevet accepteret
        //Markerer annoncer som solgt i både Market og MySales
        [HttpGet("accepted-sales")]
        public async Task<ActionResult<List<int>>> GetAcceptedSales()
        {
            var salesIds = await _repo.GetAcceptedSalesIdsAsync();
            return Ok(salesIds);
        }
}