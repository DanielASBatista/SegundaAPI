using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MidasApi.Models.Enuns;

namespace MidasApi.DTOs.Transacoes
{
    public class CriarLancamentoDTO
    {
        public StatusTransacao Status {get; set; } = StatusTransacao.Pendente;

        [Required]
        [Range(0.01, 999999999999d)]
        public decimal Valor {get; set;}

        [Required]
        public DateTime DataCriacao {get; set;}

        [MaxLength(120)]
        public string Descricao {get; set;} = string.Empty;

        [MaxLength(60)]
        public string Categoria {get; set;} = string.Empty;

        public RecorrenciaDTO? Recorrencia {get; set;}
    }
}