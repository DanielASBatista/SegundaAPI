using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MidasApi.Models.Enuns;
using ProjetoMidasAPI.Models.Enuns;
using System.Text.Json.Serialization;

public class Lancamento
{
    [Key] // Define a chave primária da tabela
    public int IdLancamento { get; set; }

    [JsonIgnore]
    public Usuario? Usuario { get; set; }
    
    public int IdUsuario { get; set; }
    
    public int? IdProjecao { get; set; } // Identificação de projeção. Se for preenchida significa que o lançamento é uma projeção confirmada no sistema

    public int? IdSimEmprestimo { get; set; } // Identificação de simulação de empréstimo. Se for preenchida significa que o lançamento é uma simulação de empréstimo confirmada no sistema

    public int? IdRecorrencia { get; set; } // Identificação de recorrência. Se for preenchida significa que o lançamento é oriundo de recorrência programada pelo usuário

    [JsonIgnore]
    public Recorrencia? Recorrencia { get; set; } // Identificação de recorrência. Se for preenchida significa que o lançamento é oriundo de recorrência programada pelo usuário    
    
    public TipoLancamentoEnum? TipoLancamento { get; set; } // Identifica o tipo de lançamento dentro das categorias pré estabelecidas

    public OrigemLancamento? OrigemLancamento { get; set; } // Identifica a origem do lançamento dentro das categorias pré estabelecidas

    public FrequenciaRecorrencia? FrequenciaRecorrencia { get; set; } // Identifica a frequência de recorrência do lançamento dentro das categorias pré estabelecidas. Se for preenchida significa que o lançamento é oriundo de recorrência programada pelo usuário

    public ModoRecorrenciaMensal? ModoRecorrenciaMensal { get; set; } // Identifica o modo de recorrência mensal do lançamento dentro das categorias pré estabelecidas. Se for preenchida significa que o lançamento é oriundo de recorrência programada pelo usuário com frequência mensal

    public CategoriaGastoEnum? CategoriaGasto { get; set; } // Categoria de gasto associada ao lançamento (Alimentação, Moradia, etc.)

    public StatusTransacao? StatusTransacao { get; set; } // Identifica o status da transação do lançamento dentro das categorias pré estabelecidas. Se for preenchida significa que o lançamento é oriundo de simulação de empréstimo ou projeção confirmada no sistema
    
    public int? QtdeRecorrencia { get; set; } // Quantidade de recorrências do lançamento. Se for preenchida significa que o lançamento é oriundo de recorrência programada pelo usuário
    
    [Required, MaxLength(500)] // Descrição do lançamento. Campo obrigatório, máximo 50 caracteres. Talvez eu mude o nome pra Nome ao invés de descrição
    public string DescricaoLancamento { get; set; } = string.Empty;

    [MaxLength(200)] // Campo opcional para observações a respeito do lançamento, máximo 200 caracteres
    public string? ObservacaoLancamento { get; set;}

    [Column(TypeName = "decimal(18,2)")] // // Define o tipo decimal no banco (18 é o número total de digitos e 2 são as casas decimais)
    public decimal Valor { get; set; }

    public DateTime Data { get; set; } = DateTime.UtcNow; // Data do lançamento, campo de busca e será inserido manual

    public DateTime DataCriacao { get; set; } = DateTime.UtcNow; // Data/hora de criação, campo de busca e será inserido automaticamente a do sistema 

    public int? NumeroDaOcorrencia { get; set; } // parcela atual (1, 2, 3...)
    public int? TotalOcorrencia { get; set; } // total (3)

}
