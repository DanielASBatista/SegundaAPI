using System.ComponentModel.DataAnnotations;

namespace ProjetoMidasAPI.Dtos.Lancamentos
{
    public class PatchLancamentoDto
    {
        [MaxLength(120)]
        public string? DescricaoLancamento { get; set; }

        [Range(0.01, 999999999999d)]
        public decimal? Valor { get; set; }

        /// <summary>0 = Receita, 1 = Despesa</summary>
        public int? TipoLancamento { get; set; }

        public DateTime? Data { get; set; }

        public int? CategoriaGasto { get; set; }

        public string? ObservacaoLancamento { get; set; }
    }
}
