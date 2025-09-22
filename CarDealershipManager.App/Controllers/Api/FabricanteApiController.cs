using CarDealershipManager.Core.Models;
using CarDealershipManager.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarDealershipManager.App.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class FabricanteApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FabricanteApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Fabricante
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Fabricante>>> GetFabricantes()
        {
            return await _context.Fabricantes.ToListAsync();
        }

        // GET: api/Fabricante/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Fabricante>> GetFabricante(int id)
        {
            var fabricante = await _context.Fabricantes.FindAsync(id);

            if (fabricante == null)
                return NotFound();

            return fabricante;
        }

        // POST: api/Fabricante
        [HttpPost]
        public async Task<ActionResult<Fabricante>> PostFabricante(Fabricante fabricante)
        {
            // Validação de unicidade (igual ao MVC)
            if (_context.Fabricantes.Any(f => f.Nome == fabricante.Nome && !f.IsDeleted))
            {
                return BadRequest(new { message = "Já existe um fabricante ativo com este nome." });
            }

            _context.Fabricantes.Add(fabricante);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFabricante), new { id = fabricante.Id }, fabricante);
        }

        // PUT: api/Fabricante/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFabricante(int id, Fabricante fabricante)
        {
            if (id != fabricante.Id)
                return BadRequest();

            // Validação de unicidade (igual ao MVC)
            if (_context.Fabricantes.Any(f => f.Nome == fabricante.Nome && !f.IsDeleted && f.Id != fabricante.Id))
            {
                return BadRequest(new { message = "Já existe um fabricante ativo com este nome." });
            }

            try
            {
                _context.Entry(fabricante).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Fabricantes.Any(e => e.Id == id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // DELETE: api/Fabricante/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFabricante(int id)
        {
            var fabricante = await _context.Fabricantes.FindAsync(id);
            if (fabricante == null)
                return NotFound();

            fabricante.IsDeleted = true;
            _context.Update(fabricante);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}