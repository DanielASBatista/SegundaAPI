using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Empresa
{
    [Key] // Define a chave primária da tabela
    public int IdEmpresa { get; set; }

    [Required] // id do responsável pela empresa. Campo obrigatório, máximo 80 caracteres.
    public int idResponsavel { get; set; }

    [Required, MaxLength(80)] // Razão social da empresa. Campo obrigatório, máximo 80 caracteres.
    public string razaoSocial { get; set; } = string.Empty;
    
    [MaxLength(50)] // Nome fantasia da empresa. Campo obrigatório, máximo 50 caracteres.
    public string nomeFantasia { get; set; } = string.Empty;

    public string? telefoneEmp { get; set; } // Telefone da empresa
    
    [Required, MaxLength(14)]
    public string cnpjEmpresa { get; set; } = string.Empty; // CNPJ da empresa. Campo obrigatório, máximo 14 caracteres.
    
    [MaxLength(100)] // Email da empresa. Campo obrigatório, máximo 100 caracteres.
    public string? emailEmpresa { get; set; } = string.Empty;
    
}