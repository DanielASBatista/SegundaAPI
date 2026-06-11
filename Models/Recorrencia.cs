using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using ProjetoMidasAPI.Models.Enuns;

public class Recorrencia
{
    [Key] // Define a chave primária da tabela
    public int IdRecorrencia { get; set; }

    [JsonIgnore]
    public Usuario? Usuario { get; set; }

    public int IdUsuario { get; set; } // Chave estrangeira para o usuário responsável pela recorrência

    public int? IdProjecao { get; set; } // Identificação de projeção. Se for preenchida significa que a recorrencia é uma projeção confirmada no sistema

    public TipoLancamentoEnum? TipoLancamento { get; set; } // Identifica o tipo de lançamento dentro das categorias pré estabelecidas


    public List<Lancamento>? Lancamentos { get; set; } // Lista de lançamentos associados a essa recorrência. Se for preenchida significa que a recorrencia já gerou lançamentos no sistema
    
    public TipoRecorrencia? TipoRecorrencia { get; set; } // Identifica o tipo de recorrência (diária, semanal, mensal, anual, etc)
    
    [MaxLength(50)] // Descrição da recorrencia. Daqui vai ser extraída a informação a respeito da repetição impplicita na recorrencia. Campo opcional por enquanto mas que obviamente vai ser obrigatório.
    public string? dsRecorrencia { get; set; }

    [MaxLength(200)] // Campo opcional para observações a respeito da recorrência, máximo 200 caracteres
    public string? obRecorrencia { get; set;}

    public DateTime dataInicio { get; set; } = DateTime.UtcNow; // Data do inicio da recorrencia, campo de busca e será inserido manual

    public int? qtdeRecorrencia {get; set;} // Essa é a quantidade de recorrencias. Ela pode ser definida pelo usuário ou escolhida entre valores pré existentes. Só Deus sabe como eu vou programar isso.
    [Required, Column(TypeName = "decimal(18,2)")] // // Define o tipo decimal no banco (18 é o número total de digitos e 2 são as casas decimais)
    public decimal Valor { get; set; }
    public DateTime momentoCriacao { get; set; } = DateTime.UtcNow; // Data/hora de criação, campo de busca e será inserido automaticamente a do sistema
    public int? UsuarioResponsavel { get; set; }
}
