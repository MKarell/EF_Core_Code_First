using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using EF_Core_Code_First.Data;
using EF_Core_Code_First.DTOs;
using EF_Core_Code_First.Models;

namespace EF_Core_Code_First.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PcsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PcsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ResponseDTO>>> GetPcs()
        {
            var pcs = await _context.PCs
                .Select(p => new ResponseDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Weight = p.Weight,
                    Warranty = p.Warranty,
                    CreatedAt = p.CreatedAt,
                    Stock = p.Stock
                })
                .ToListAsync();

            return Ok(pcs);
        }
        [HttpGet("{id}/components")]
        public async Task<ActionResult<IEnumerable<ComponentResponseDTO>>> GetPcComponents(int id)
        {
            var pcExists = await _context.PCs.AnyAsync(p => p.Id == id);
            if (!pcExists)
            {
                return NotFound(); // Status 404 Not Found [cite: 95, 149]
            }

            var components = await _context.PCComponents
                .Where(pc => pc.PCId == id)
                .Select(pc => new ComponentResponseDTO
                {
                    ComponentCode = pc.ComponentCode,
                    Name = pc.Component.Name,
                    Amount = pc.Amount
                })
                .ToListAsync();

            return Ok(components);
        }
        [HttpPost]
        public async Task<ActionResult<ResponseDTO>> CreatePc(RequestDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Status 400 [cite: 148]
            }

            var pc = new PCs
            {
                Name = request.Name,
                Weight = request.Weight,
                Warranty = request.Warranty,
                CreatedAt = request.CreatedAt,
                Stock = request.Stock
            };

            _context.PCs.Add(pc);
            await _context.SaveChangesAsync();

            var responseDto = new ResponseDTO
            {
                Id = pc.Id,
                Name = pc.Name,
                Weight = pc.Weight,
                Warranty = pc.Warranty,
                CreatedAt = pc.CreatedAt,
                Stock = pc.Stock
            };

            return CreatedAtAction(nameof(GetPcs), new { id = pc.Id }, responseDto);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePc(int id, RequestDTO request)
        {
            var pc = await _context.PCs.FindAsync(id);
            if (pc == null)
            {
                return NotFound();
            }

            pc.Name = request.Name;
            pc.Weight = request.Weight;
            pc.Warranty = request.Warranty;
            pc.CreatedAt = request.CreatedAt;
            pc.Stock = request.Stock;

            await _context.SaveChangesAsync();

            return Ok(pc);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePc(int id)
        {
            var pc = await _context.PCs.FindAsync(id);
            if (pc == null)
            {
                return NotFound();
            }

            _context.PCs.Remove(pc);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
