using MidasApi.Models.Enuns;
using ProjetoMidasAPI.Models.Enuns;

namespace ProjetoMidasAPI.DTOs.Projecoes
{
    public class ConfirmarProjecaoDto
    {
        public string? Descricao { get; set; }
        public decimal? Valor { get; set; }
        public DateTime? Data { get; set; }
        public TipoLancamentoEnum? TipoLancamento { get; set; }
        public CategoriaGastoEnum? CategoriaGasto { get; set; }
    }
}
