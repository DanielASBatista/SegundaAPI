using System.ComponentModel.DataAnnotations;
using ProjetoMidasAPI.Models.Enuns;

namespace ProjetoMidasAPI.DTOs.Recorrencia
{
    public class RecorrenciaDto
    {
        public FrequenciaRecorrencia FrequenciaRecorrencia { get; set; }

        [Range(1,240)]
        public int QtdeRecorrencia { get; set; }

        public ModoRecorrenciaMensal? ModoRecorrenciaMensal { get; set; }

        [Range(1,31)]
        public int? DataRecorrencia { get; set; }

        [Range(1, 365)]
        public int? DiasIntervalo { get; set; }
    }
}   