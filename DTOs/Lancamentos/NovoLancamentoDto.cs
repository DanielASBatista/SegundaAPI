using System.ComponentModel.DataAnnotations;
using MidasApi.Models.Enuns;
using ProjetoMidasAPI.DTOs.Recorrencia;
using ProjetoMidasAPI.Models.Enuns;

namespace ProjetoMidasAPI.Dtos.Lancamentos
{
    public class NovoLancamentoDto
    {
        
        public int IdUsuario { get; set; }
        public StatusTransacao StatusTransacao {get; set;} = StatusTransacao.Confirmada;

        [Range(0.01, 999999999999d)]
        public decimal Valor { get; set; }

        [Required]
        public TipoLancamentoEnum TipoLancamento { get; set; }        
        
        public DateTime DataCriacao { get; set; }

        [Required]
        [MaxLength(120)]
        public string DescricaoLancamento { get; set; } = string.Empty;

        public DateTime Data { get; set; }
        
        [MaxLength(60)]
        public string Category { get; set; } = string.Empty;

        public RecorrenciaDto? Recorrencia { get; set; }
    }
}