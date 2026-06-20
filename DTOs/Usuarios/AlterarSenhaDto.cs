namespace ProjetoMidasAPI.Dtos.Usuarios
{
    public class AlterarSenhaDto
    {
        public string SenhaAtual { get; set; } = string.Empty;
        public string NovaSenha { get; set; } = string.Empty;
    }

    public class AtualizarPerfilDto
    {
        public string? NomeUsuario { get; set; }
        public string? Sobrenome { get; set; }
        public string? EmailUsuario { get; set; }
        public string? Telefone { get; set; }
    }
}
