using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

public class Emprestimo
{
    [Key]
    public int IdSimEmprestimo { get; set; }
    
    [JsonIgnore]
    public Usuario? Usuario { get; set; }

    public int IdUsuario { get; set; }              
    
    [Required, MaxLength(50)]
    public string nomeEmprestimo { get; set; } = string.Empty;
    
    [MaxLength(200)]
    public string? descricaoEmprestimo { get; set; }
    
    public string? provedorEmprestimo { get; set; }
    
    [Required] 
    [Column(TypeName = "decimal(18,2)")]
    public decimal valorEmprestimo { get; set; }
    
    [Required] 
    public int parcelasEmprestimo { get; set;}
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal valorParcelas { get; set; }
    
    [Required, Column(TypeName = "decimal(5,4)")]
    public decimal IOFemprestimo { get; set; }   // taxa base, ex: 0.0038
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal despesasEmprestimo { get; set;}
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal tarifasEmprestimo { get; set;}
    
    public DateTime Data { get; set; } = DateTime.UtcNow;
    public DateTime DataCriacaoSE { get; set; } = DateTime.UtcNow;
    public int? UsuarioResponsavel { get; set; }

    // Propriedade calculada para ValorTotal
    [NotMapped]
    public decimal ValorTotal 
    { 
        get
        {
            var valorIofCalculado = CalcularIOF();
            return decimal.Round(
                valorEmprestimo + valorIofCalculado + despesasEmprestimo + tarifasEmprestimo,
                2,
                MidpointRounding.AwayFromZero
            );
        }
    }

    // Método que calcula IOF considerando dias do empréstimo (padrão 30 dias)
    public decimal CalcularIOF(int diasEmprestimo = 30)
    {
        const decimal IOF_DIARIO = 0.000082m; // 0,0082% ao dia
        var valorIof = valorEmprestimo * (IOFemprestimo + IOF_DIARIO * diasEmprestimo);
        return decimal.Round(valorIof, 2, MidpointRounding.AwayFromZero);
    }

    // Método para calcular valor da parcela e arredondar
    public void CalcularValores(int diasEmprestimo = 30)
    {
        if (parcelasEmprestimo <= 0)
            throw new InvalidOperationException("O número de parcelas deve ser maior que zero.");

        var valorTotal = ValorTotal;
        valorParcelas = decimal.Round(
            valorTotal / parcelasEmprestimo,
            2,
            MidpointRounding.AwayFromZero
        );
    }
}