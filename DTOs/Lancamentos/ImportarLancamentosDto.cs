using MidasApi.Models.Enuns;
using ProjetoMidasAPI.Models.Enuns;
using System.ComponentModel.DataAnnotations;

namespace ProjetoMidasAPI.Dtos.Lancamentos
{
    public class ImportarLancamentoRequestDto
    {
        [Required]
        [MinLength(1)]
        public List<ImportarLancamentoItemDto> Lancamentos { get; set; } = new();
    }

    public class ImportarLancamentoItemDto
    {
        [Required]
        [MaxLength(120)]
        public string DescricaoLancamento { get; set; } = string.Empty;

        [Range(0.01, 999999999999d)]
        public decimal Valor { get; set; }

        /// <summary>0 = Receita, 1 = Despesa</summary>
        [Required]
        public int TipoLancamento { get; set; }

        [Required]
        public DateTime Data { get; set; }

        public int? CategoriaGasto { get; set; }
    }
}
