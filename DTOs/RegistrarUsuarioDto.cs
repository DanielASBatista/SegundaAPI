using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoMidasAPI.Dtos
{
    public class RegistrarUsuarioDto
    {
        public string NomeUsuario { get; set; } = string.Empty;
        public string PasswordString { get; set; } = string.Empty;
    }
}