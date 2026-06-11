using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoMidasAPI.Dtos
{
    public class LoginResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public UsuarioDto Usuario { get; set; } = null!;
    }

    public class UsuarioDto
    {
        public int Id { get; set; }
        public string NomeUsuario { get; set; } = string.Empty;
        public int IdEmpresa { get; set; }
        public string Perfil { get; set; } = string.Empty;
    }
}
