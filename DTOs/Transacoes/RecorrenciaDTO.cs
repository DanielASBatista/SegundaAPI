using System.ComponentModel.DataAnnotations;
using ProjetoMidasAPI.Models.Enuns;

namespace MidasApi.DTOs.Transacoes
{
    public class RecorrenciaDTO
    {
        public FrequenciaRecorrencia Frequencia {get; set;}

        [Range(1, 240)]
        public int Ocorrencias {get; set;}

        public ModoRecorrenciaMensal? ModoRecorrencia {get; set;}

        [Range(1, 365)]
        public int? IntervaloDias { get; set; }
    }
}