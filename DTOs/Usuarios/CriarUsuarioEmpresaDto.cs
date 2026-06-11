namespace ProjetoMidasAPI.Dtos.Usuarios
{
    public class CriarUsuarioEmpresaDto
    {
        public string NomeUsuario { get; set; } = string.Empty;
        public string PasswordString { get; set; } = string.Empty;
        public string Perfil { get; set; } = string.Empty;
        public string Sobrenome { get; set; } = string.Empty;
        public string EmailUsuario { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
    }

    public class AtualizarPerfilUsuarioDto
    {
        public string Perfil { get; set; } = string.Empty;
    }
}
