using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjetoMidasAPI.Data;

namespace ProjetoMidasAPI.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class ResponsavelController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ResponsavelController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Responsavel
        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<Responsavel>>> GetResponsaveis()
        {
            return await _context.Set<Responsavel>().ToListAsync();
        }

        // GET: api/Responsavel/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Responsavel>> GetResponsavel(int id)
        {
            var responsavel = await _context.Set<Responsavel>().FindAsync(id);

            if (responsavel == null)
            {
                return NotFound();
            }

            return responsavel;
        }
        [HttpPost("New")]
        public async Task<ActionResult<Responsavel>> PostResponsavel(Responsavel responsavel)
        {
            _context.Set<Responsavel>().Add(responsavel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetResponsavel", new { id = responsavel.IdResponsavel }, responsavel);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteResponsavel(int id)
        {
            var responsavel = await _context.Set<Responsavel>().FindAsync(id);
            if (responsavel == null)
            {
                return NotFound();
            }

            _context.Set<Responsavel>().Remove(responsavel);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}