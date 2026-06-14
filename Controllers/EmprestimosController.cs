using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoMidasAPI.Data;
using System.Security.Claims;

namespace ProjetoMidasAPI.Controllers
{

    [Authorize]
    [ApiController]
    [Route("[controller]")] // Define a rota Emprestimos
    public class EmprestimosController : ControllerBase
    {
        private readonly AppDbContext _context;

        //Construtor
        public EmprestimosController(AppDbContext context) => _context = context;

        private int UserId =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        private async Task<int?> GetUserEmpresaId()
        {
            var user = await _context.Usuarios.FindAsync(UserId);
            return user?.IdEmpresa > 0 ? user.IdEmpresa : null;
        }

        private IQueryable<Emprestimo> QueryUsuario()
        {
            return _context.Emprestimos
                .Where(e => e.IdUsuario == UserId);
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<Emprestimo>>> GetAll() =>
            await QueryUsuario().ToListAsync();

        // Retorna uma simulação específica
        [HttpGet("{id}")]
        public async Task<ActionResult<Emprestimo>> GetById(int id)
        {
            var emprestimo = await QueryUsuario().FirstOrDefaultAsync(e => e.IdSimEmprestimo == id);

            if (emprestimo == null)
                return NotFound();

            return emprestimo;
        }
        // Cria uma nova simulação
        [HttpPost("New")]
        public async Task<ActionResult<Emprestimo>> Post(Emprestimo emprestimo)
        {
            
            emprestimo.IdUsuario = UserId;
            emprestimo.DataCriacaoSE = DateTime.UtcNow;

            // Calcula valores
            emprestimo.CalcularValores();

            _context.Emprestimos.Add(emprestimo);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = emprestimo.IdSimEmprestimo }, emprestimo);
        }

        // Edita uma simulação existente
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Emprestimo emprestimoAtualizado)
        {
            if (emprestimoAtualizado == null)
                return BadRequest();

            var emprestimo = await QueryUsuario().FirstOrDefaultAsync(e => e.IdSimEmprestimo == id);
            
            if (emprestimo == null)
                return NotFound();

            emprestimo.nomeEmprestimo = emprestimoAtualizado.nomeEmprestimo;
            emprestimo.descricaoEmprestimo = emprestimoAtualizado.descricaoEmprestimo;
            emprestimo.provedorEmprestimo = emprestimoAtualizado.provedorEmprestimo;
            emprestimo.valorEmprestimo = emprestimoAtualizado.valorEmprestimo;
            emprestimo.parcelasEmprestimo = emprestimoAtualizado.parcelasEmprestimo;
            emprestimo.IOFemprestimo = emprestimoAtualizado.IOFemprestimo;
            emprestimo.despesasEmprestimo = emprestimoAtualizado.despesasEmprestimo;
            emprestimo.tarifasEmprestimo = emprestimoAtualizado.tarifasEmprestimo;
            emprestimo.Data = emprestimoAtualizado.Data;
            emprestimo.CategoriaGasto = emprestimoAtualizado.CategoriaGasto;

            emprestimo.CalcularValores();

            await _context.SaveChangesAsync();
            return NoContent();
        }
        // Remove uma simulação
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var emprestimo = await QueryUsuario().FirstOrDefaultAsync(e => e.IdSimEmprestimo == id);
            if (emprestimo == null) return NotFound();

            _context.Emprestimos.Remove(emprestimo);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        // READ - Lista todos os empréstimos da empresa (exclui Admins)
        [HttpGet("Empresa")]
        public async Task<ActionResult<IEnumerable<Emprestimo>>> GetAllEmpresa()
        {
            var empresaId = await GetUserEmpresaId();
            if (empresaId == null)
                return Ok(Array.Empty<Emprestimo>());

            var idsUsuarios = await _context.Usuarios
                .Where(u => u.IdEmpresa == empresaId.Value
                         && u.Perfil != null
                         && u.Perfil != "Administrador")
                .Select(u => u.IdUsuario)
                .ToListAsync();

            return await _context.Emprestimos
                .Where(e => idsUsuarios.Contains(e.IdUsuario))
                .ToListAsync();
        }

        //É interessante considerar a adição de filtros para os empréstimos.
    }
}