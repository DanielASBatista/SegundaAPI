//AQUI TEM UM MÉTODO QUE PRECISA SER ARRUMADO!
//O filtro por tipo de recorrencia, mensal semanal e etc, ta quebrado, precisa arrumar depois.
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;    
using ProjetoMidasAPI.Data;
using ProjetoMidasAPI.Models.Enuns;

namespace ProjetoMidasAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")] // Define a rota sem precisar colocar API: /Recorrencia
    public class RecorrenciaController : ControllerBase
    {
        private readonly AppDbContext _context;

        // Construtor
        public RecorrenciaController(AppDbContext context) => _context = context;

        private int UserId =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        private async Task<int?> GetUserEmpresaId()
        {
            var user = await _context.Usuarios.FindAsync(UserId);
            return user?.IdEmpresa > 0 ? user.IdEmpresa : null;
        }

        private IQueryable<Recorrencia> QueryUsuario()
        {
            return _context.Recorrencias
                .Where(r => r.IdUsuario == UserId);
        }
        
        // READ - Lista todas as recorrências
        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<Recorrencia>>> GetAll() =>
            await QueryUsuario()
            .Include(r => r.Lancamentos)
            .Include(r => r.TipoRecorrencia)
            .ToListAsync();

        // READ - Busca por ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Recorrencia>> GetById(int id)
        {
            var recorrencia = await QueryUsuario()
            .Include(r => r.TipoRecorrencia)
            .FirstOrDefaultAsync(r => r.IdRecorrencia == id);
        
            if (recorrencia == null)
                return NotFound();
            
            return recorrencia;
        }

        // CREATE - Adiciona nova recorrência
        [HttpPost("New")]
        public async Task<ActionResult<Recorrencia>> Post(Recorrencia recorrencia)
        {
            recorrencia.IdUsuario = UserId; 
            recorrencia.momentoCriacao = DateTime.UtcNow; 
            
            _context.Recorrencias.Add(recorrencia); 
            
            await _context.SaveChangesAsync(); 
            
            return CreatedAtAction(nameof(GetById), new { id = recorrencia.IdRecorrencia }, recorrencia); 
        }

        // UPDATE - Atualiza recorrência existente
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Recorrencia recorrencia)
        {
            var existente = await QueryUsuario().FirstOrDefaultAsync(r => r.IdRecorrencia == id);
            
            if (existente == null) return NotFound();

            existente.Valor = recorrencia.Valor;
            existente.TipoRecorrencia = recorrencia.TipoRecorrencia;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE - Remove recorrência
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existente = await QueryUsuario().FirstOrDefaultAsync(r => r.IdRecorrencia == id);
            
            if (existente == null) return NotFound();

            _context.Recorrencias.Remove(existente);
            await _context.SaveChangesAsync();
            return NoContent();   
        }
        [HttpGet("Empresa")]
        public async Task<ActionResult<IEnumerable<Recorrencia>>> GetAllEmpresa()
        {
            var empresaId = await GetUserEmpresaId();
            if (empresaId == null)
                return Ok(Array.Empty<Recorrencia>());

            var idsUsuarios = await _context.Usuarios
                .Where(u => u.IdEmpresa == empresaId.Value
                         && u.Perfil != null
                         && u.Perfil != "Administrador")
                .Select(u => u.IdUsuario)
                .ToListAsync();

            return await _context.Recorrencias
                .Include(r => r.Lancamentos)
                .Include(r => r.TipoRecorrencia)
                .Where(r => idsUsuarios.Contains(r.IdUsuario))
                .ToListAsync();
        }

        [HttpGet("Data/{data}")]
        public async Task<ActionResult<IEnumerable<Recorrencia>>> GetByData(DateTime data)
        {
            return await QueryUsuario()
                .Where(r => r.momentoCriacao.Date == data.Date)
                .ToListAsync();
        }
        [HttpGet("Tipo/{tipoId}")]
        public async Task<ActionResult<IEnumerable<Recorrencia>>> GetByTipo(int tipoId)
        {
            return await QueryUsuario()
                .Where(r => r.TipoLancamento == (TipoLancamentoEnum)tipoId)
                .ToListAsync();
        }
        [HttpGet("Valor/{valor}")]
        public async Task<ActionResult<IEnumerable<Recorrencia>>> GetByValor(decimal valor)
        {
            return await QueryUsuario()
                .Where(r => r.Valor == valor)
                .ToListAsync();
        }
    }
}