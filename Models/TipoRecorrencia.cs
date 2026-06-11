//A criação dessa classe serve para definir os tipos de recorrência que podem ser associados a uma Recorrência específica (diária, semanal, mensal, anual, etc)
//OU recorrências personalizadas criadas pelo usuário.
using System.ComponentModel.DataAnnotations;

public class TipoRecorrencia
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Nome { get; set; } = null!;

    // true = criado pelo sistema (Mensal, Semanal, etc)
    // false = criado pelo usuário
    public bool PadraoSistema { get; set; } = false;
}
