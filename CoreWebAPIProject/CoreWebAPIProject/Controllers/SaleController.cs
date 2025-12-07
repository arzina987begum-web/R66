using CoreWebAPIProject.DataContext;
using CoreWebAPIProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoreWebAPIProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaleController : ControllerBase
    {
        private readonly AppDBContext _context;

        public SaleController(AppDBContext context)
        {
            _context = context;
        }

        // -------------------- GET ALL --------------------
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var sale = await _context.saleMasters
                        .Include(s => s.saleDetail)
                        .ToListAsync();
            return Ok(sale);
        }

        // -------------------- GET BY ID --------------------
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var sale = await _context.saleMasters
                        .Include(s => s.saleDetail)
                        .FirstOrDefaultAsync(s => s.Id == id);

            if (sale == null)
                return NotFound();

            return Ok(sale);
        }

        // -------------------- CREATE --------------------
        [HttpPost("Create")]
        public async Task<IActionResult> Create(SaleMaster sm)
        {
            _context.Add(sm);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = sm.Id }, sm);
        }

        // -------------------- UPDATE --------------------
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, SaleMaster model)
        {
            if (id != model.Id)
                return BadRequest("ID mismatch");

            var existing = await _context.saleMasters
                                .Include(s => s.saleDetail)
                                .FirstOrDefaultAsync(s => s.Id == id);

            if (existing == null)
                return NotFound();

            // Update SaleMaster fields
            existing.CustomerName = model.CustomerName;
            existing.SaleDate = model.SaleDate;

            // Remove old SaleDetail
            _context.saleDetails.RemoveRange(existing.saleDetail);

            // Add new SaleDetail
            existing.saleDetail = model.saleDetail;

            await _context.SaveChangesAsync();

            return Ok(existing);
        }

        // -------------------- DELETE --------------------
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var sale = await _context.saleMasters
                        .Include(s => s.saleDetail)
                        .FirstOrDefaultAsync(s => s.Id == id);

            if (sale == null)
                return NotFound();

            // First delete details (if cascade not enabled)
            _context.saleDetails.RemoveRange(sale.saleDetail);

            // Then delete master
            _context.saleMasters.Remove(sale);

            await _context.SaveChangesAsync();

            return Ok("Sale deleted successfully");
        }
    }
}
