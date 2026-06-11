using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoMidasAPI.Data;
using ProjetoMidasAPI.Dtos.Empresa;
using ProjetoMidasAPI.Models.Enuns;

namespace ProjetoMidasAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class EmpresaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EmpresaController(AppDbContext context) => _context = context;

        private int UserId =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        private async Task<Usuario?> GetUsuarioLogado()
        {
            return await _context.Usuarios.FirstOrDefaultAsync(u => u.IdUsuario == UserId);
        }

        private static bool IsAdminEmpresa(Usuario usuario)
        {
            return usuario.Perfil == "Administrador" ||
                usuario.TipoUsuario == TipoUsuarioEnum.Administrador;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<Empresa>>> GetAll()
        {
            var usuario = await GetUsuarioLogado();

            if (usuario == null || usuario.IdEmpresa <= 0)
                return Ok(Array.Empty<Empresa>());

            return await _context.Empresas
                .Where(e => e.IdEmpresa == usuario.IdEmpresa)
                .ToListAsync();
        }

        [HttpGet("Minha")]
        public async Task<ActionResult<Empresa>> GetMinhaEmpresa()
        {
            var usuario = await GetUsuarioLogado();

            if (usuario == null)
                return Unauthorized();

            if (usuario.IdEmpresa <= 0)
                return NoContent();

            var empresa = await _context.Empresas.FindAsync(usuario.IdEmpresa);

            if (empresa == null)
                return NotFound();

            return empresa;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Empresa>> GetById(int id)
        {
            var usuario = await GetUsuarioLogado();

            if (usuario == null)
                return Unauthorized();

            var empresa = await _context.Empresas
                .FirstOrDefaultAsync(e => e.IdEmpresa == id && e.IdEmpresa == usuario.IdEmpresa);

            if (empresa == null)
                return NotFound();

            return empresa;
        }

        [AllowAnonymous]
        [HttpPost("New")]
        public async Task<ActionResult<Empresa>> Post(Empresa empresa)
        {
            _context.Empresas.Add(empresa);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = empresa.IdEmpresa }, empresa);
        }

        [HttpPost("Minha")]
        public async Task<ActionResult<Empresa>> CriarMinhaEmpresa(EmpresaDto dto)
        {
            var usuario = await GetUsuarioLogado();

            if (usuario == null)
                return Unauthorized();

            if (usuario.IdEmpresa > 0)
                return BadRequest("Usuario ja vinculado a uma empresa.");

            var empresa = new Empresa
            {
                idResponsavel = usuario.IdUsuario,
                razaoSocial = dto.RazaoSocial,
                nomeFantasia = dto.NomeFantasia,
                telefoneEmp = dto.TelefoneEmp,
                cnpjEmpresa = dto.CnpjEmpresa,
                emailEmpresa = dto.EmailEmpresa
            };

            _context.Empresas.Add(empresa);
            await _context.SaveChangesAsync();

            usuario.IdEmpresa = empresa.IdEmpresa;
            usuario.Perfil = "Administrador";
            usuario.TipoUsuario = TipoUsuarioEnum.Administrador;
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMinhaEmpresa), empresa);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, EmpresaDto empresaAtualizada)
        {
            var usuario = await GetUsuarioLogado();

            if (usuario == null)
                return Unauthorized();

            if (!IsAdminEmpresa(usuario) || usuario.IdEmpresa != id)
                return Forbid();

            var empresa = await _context.Empresas.FindAsync(id);

            if (empresa == null)
                return NotFound();

            empresa.razaoSocial = empresaAtualizada.RazaoSocial;
            empresa.nomeFantasia = empresaAtualizada.NomeFantasia;
            empresa.telefoneEmp = empresaAtualizada.TelefoneEmp;
            empresa.cnpjEmpresa = empresaAtualizada.CnpjEmpresa;
            empresa.emailEmpresa = empresaAtualizada.EmailEmpresa;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var usuario = await GetUsuarioLogado();

            if (usuario == null)
                return Unauthorized();

            if (!IsAdminEmpresa(usuario) || usuario.IdEmpresa != id)
                return Forbid();

            var empresa = await _context.Empresas.FirstOrDefaultAsync(e => e.IdEmpresa == id);

            if (empresa == null)
                return NotFound();

            _context.Empresas.Remove(empresa);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
