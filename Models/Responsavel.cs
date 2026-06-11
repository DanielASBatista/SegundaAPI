//A questao do responsavel precisa ser tratada de uma maneira que,
//na minha cabeça, ela vai ser herdeira da superclasse usuário.
//Um usuário cria uma empresa, cadastra responsaveis que 
//não são nada além de usuários com ocupações especificas.
//Isso claramente não tem uso prático, mas é uma maneira de demonstrar
//o conceito de herança e poliformismo.
using System.ComponentModel.DataAnnotations;

public class Responsavel 
{
    [Key] // Define a chave primária da tabela
    public int IdResponsavel { get; set; }

    [Required, MaxLength(20)] // Nome do responsável. Campo obrigatório, máximo 20 caracteres.
    public string nomeResponsavel { get; set; } = string.Empty;
    
    [Required, MaxLength(80)] // Sobrenome do responsável. Campo obrigatório, máximo 80 caracteres.
    public string sobrenomeResponsavel { get; set; } = string.Empty;
    
    /*[MaxLength(14)] // CPF do responsável. Campo obrigatório, máximo 14 caracteres.
    public string? cpfResponsavel { get; set; } = string.Empty; Preciso ver se vale a pena colocar isso no projeto e no banco de dados. Parece fazer sentido.*/
    public string? telefoneResponsavel { get; set; } // Telefone do responsável
    
    [Required, MaxLength(100)] // Email do responsável. Campo obrigatório, máximo 100 caracteres.
    public string emailResponsavel { get; set; } = string.Empty;    
}