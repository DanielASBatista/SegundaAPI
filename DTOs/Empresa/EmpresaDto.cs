namespace ProjetoMidasAPI.Dtos.Empresa
{
    public class EmpresaDto
    {
        public string RazaoSocial { get; set; } = string.Empty;
        public string NomeFantasia { get; set; } = string.Empty;
        public string? TelefoneEmp { get; set; }
        public string CnpjEmpresa { get; set; } = string.Empty;
        public string? EmailEmpresa { get; set; }
    }
}
