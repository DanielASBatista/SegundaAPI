using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoMidasAPI.Data;
using ProjetoMidasAPI.Utils;
using ProjetoMidasAPI.Dtos;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using ProjetoMidasAPI.Dtos.Usuarios;
using ProjetoMidasAPI.Models;
using ProjetoMidasAPI.Models.Enuns;

namespace ProjetoMidasAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    
    public class UsuarioController : ControllerBase
    {
        private readonly AppDbContext _context;

        private readonly IConfiguration _configuration;

        public UsuarioController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        private async Task<bool> UsuarioExistente(string nomeUsuario)
        {
            if (await _context.Usuarios.AnyAsync(u => u.nomeUsuario.ToLower() == nomeUsuario.ToLower()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

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

        private string CriarToken(Usuario usuario)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
                new Claim(ClaimTypes.Name, usuario.nomeUsuario)
            };

            //Pra não virar bagunça e ficar citando a instancia a toda hora nós só definimos uma variável pra ela e chamamos como sendo uma nova instancia.
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("ConfiguracaoToken:Chave").Value!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(29),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
        [HttpGet("ValidarToken")]
        public IActionResult ValidarToken()
        {
            return Ok(new { valido = true });
        }
        
        [AllowAnonymous]
        [HttpPost("Registrar")]
        public async Task<IActionResult> RegistrarUsuario(RegistrarUsuarioDto dto)
        {
            if (await UsuarioExistente(dto.NomeUsuario))
                return BadRequest("Nome de usuário já existe!");

            Usuario usuario = new Usuario
            {
                nomeUsuario = dto.NomeUsuario,
                Perfil = "Visitante",
                TipoUsuario = TipoUsuarioEnum.Visitante
            };

            Criptografia.CriarPasswordHash(dto.PasswordString, out byte[] hash, out byte[] salt);

            usuario.PasswordHash = hash;
            usuario.PasswordSalt = salt;

            await _context.Usuarios.AddAsync(usuario);
            await _context.SaveChangesAsync();

            return Ok(new { mensagem = "Usuário registrado com sucesso!" });
        }
        [AllowAnonymous]
        [HttpPost("Autenticar")]
        public async Task<IActionResult> AutenticarUsuario(Usuario credenciais)
        {
            try
            {
                var usuario = await _context.Usuarios
                    .FirstOrDefaultAsync(u => 
                        u.nomeUsuario.ToLower() == credenciais.nomeUsuario.ToLower());

                if (usuario == null)
                    return Unauthorized("Usuário não encontrado.");

                if (!Criptografia.VerificarPasswordHash(
                    credenciais.PasswordString,
                    usuario.PasswordHash!,
                    usuario.PasswordSalt!))
                {
                    return Unauthorized("Senha incorreta.");
                }

                var token = CriarToken(usuario);

                var response = new LoginResponseDto
                {
                    Token = token,
                    Usuario = new UsuarioDto
                    {
                        Id = usuario.IdUsuario,
                        NomeUsuario = usuario.nomeUsuario,
                        IdEmpresa = usuario.IdEmpresa,
                        Perfil = usuario.Perfil ?? string.Empty
                    }
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao autenticar usuário: {ex.Message}");
            }
        }
        [HttpPut("AlterarSenha")]
        public async Task<IActionResult> AlterarSenhaUsuario(AlterarSenhaDto dto)
        {
            try
            {
                var usuario = await GetUsuarioLogado();
                if (usuario == null)
                    return Unauthorized();

                if (usuario.PasswordHash == null || usuario.PasswordSalt == null)
                    return BadRequest("Erro ao verificar senha atual.");

                if (!Criptografia.VerificarPasswordHash(dto.SenhaAtual, usuario.PasswordHash, usuario.PasswordSalt))
                    return BadRequest("Senha atual incorreta.");

                Criptografia.CriarPasswordHash(dto.NovaSenha, out byte[] hash, out byte[] salt);
                usuario.PasswordHash = hash;
                usuario.PasswordSalt = salt;

                await _context.SaveChangesAsync();
                return Ok(new { mensagem = "Senha alterada com sucesso!" });
            }
            catch (System.Exception ex)
            {
                return BadRequest($"Erro ao alterar a senha do usuário: {ex.Message}");
            }
        }

        [HttpPut("Perfil")]
        public async Task<IActionResult> AtualizarPerfil(AtualizarPerfilDto dto)
        {
            try
            {
                var usuario = await GetUsuarioLogado();
                if (usuario == null)
                    return Unauthorized();

                if (!string.IsNullOrEmpty(dto.NomeUsuario))
                    usuario.nomeUsuario = dto.NomeUsuario;

                if (!string.IsNullOrEmpty(dto.Sobrenome))
                    usuario.sobrenome = dto.Sobrenome;

                if (!string.IsNullOrEmpty(dto.EmailUsuario))
                    usuario.emailUsuario = dto.EmailUsuario;

                if (!string.IsNullOrEmpty(dto.Telefone))
                    usuario.telefone = dto.Telefone;

                await _context.SaveChangesAsync();

                return Ok(new
                {
                    nomeUsuario = usuario.nomeUsuario,
                    sobrenome = usuario.sobrenome,
                    emailUsuario = usuario.emailUsuario,
                    telefone = usuario.telefone
                });
            }
            catch (System.Exception ex)
            {
                return BadRequest($"Erro ao atualizar perfil: {ex.Message}");
            }
        }

        [HttpDelete("Excluir")]
        public async Task<IActionResult> ExcluirConta()
        {
            try
            {
                var usuario = await GetUsuarioLogado();
                if (usuario == null)
                    return Unauthorized();

                _context.Usuarios.Remove(usuario);
                await _context.SaveChangesAsync();

                return Ok(new { mensagem = "Conta excluída com sucesso." });
            }
            catch (System.Exception ex)
            {
                return BadRequest($"Erro ao excluir conta: {ex.Message}");
            }
        }
        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            return await _context.Set<Usuario>().ToListAsync();
        }
        [HttpGet("Get/{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            var usuario = await _context.Usuarios
            .Include(u => u.Lancamentos)
            .FirstOrDefaultAsync(u => u.IdUsuario == id);

            if (usuario == null)
            {
                return NotFound();
            }

            return usuario;
        }
        [HttpPost("New")]
        public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
        {
            _context.Set<Usuario>().Add(usuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUsuario", new { id = usuario.IdUsuario }, usuario);
        }

        [HttpGet("Empresa")]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuariosEmpresa()
        {
            var usuarioLogado = await GetUsuarioLogado();

            if (usuarioLogado == null)
                return Unauthorized();

            if (usuarioLogado.IdEmpresa <= 0)
                return Ok(Array.Empty<Usuario>());

            return await _context.Usuarios
                .Where(u => u.IdEmpresa == usuarioLogado.IdEmpresa)
                .ToListAsync();
        }

        [HttpPost("Empresa")]
        public async Task<ActionResult<Usuario>> CriarUsuarioEmpresa(CriarUsuarioEmpresaDto dto)
        {
            var usuarioLogado = await GetUsuarioLogado();

            if (usuarioLogado == null)
                return Unauthorized();

            if (!IsAdminEmpresa(usuarioLogado) || usuarioLogado.IdEmpresa <= 0)
                return Forbid();

            if (await UsuarioExistente(dto.NomeUsuario))
                return BadRequest("Nome de usuário já existe!");

            Criptografia.CriarPasswordHash(dto.PasswordString, out byte[] hash, out byte[] salt);

            var usuario = new Usuario
            {
                nomeUsuario = dto.NomeUsuario,
                PasswordHash = hash,
                PasswordSalt = salt,
                IdEmpresa = usuarioLogado.IdEmpresa,
                Perfil = dto.Perfil,
                TipoUsuario = TipoUsuarioEnum.Visitante,
                sobrenome = dto.Sobrenome,
                emailUsuario = dto.EmailUsuario,
                telefone = dto.Telefone
            };

            await _context.Usuarios.AddAsync(usuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUsuario", new { id = usuario.IdUsuario }, usuario);
        }

        [HttpPut("Empresa/{id}/Perfil")]
        public async Task<IActionResult> AtualizarPerfilUsuarioEmpresa(int id, AtualizarPerfilUsuarioDto dto)
        {
            var usuarioLogado = await GetUsuarioLogado();

            if (usuarioLogado == null)
                return Unauthorized();

            if (!IsAdminEmpresa(usuarioLogado) || usuarioLogado.IdEmpresa <= 0)
                return Forbid();

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.IdUsuario == id && u.IdEmpresa == usuarioLogado.IdEmpresa);

            if (usuario == null)
                return NotFound();

            usuario.Perfil = dto.Perfil;
            usuario.TipoUsuario = dto.Perfil == "Administrador"
                ? TipoUsuarioEnum.Administrador
                : TipoUsuarioEnum.Visitante;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuarioLogado = await GetUsuarioLogado();

            if (usuarioLogado == null)
                return Unauthorized();

            var usuario = await _context.Set<Usuario>().FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            if (usuarioLogado.IdUsuario != id)
            {
                if (!IsAdminEmpresa(usuarioLogado) ||
                    usuarioLogado.IdEmpresa <= 0 ||
                    usuario.IdEmpresa != usuarioLogado.IdEmpresa)
                {
                    return Forbid();
                }
            }

            _context.Set<Usuario>().Remove(usuario);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
