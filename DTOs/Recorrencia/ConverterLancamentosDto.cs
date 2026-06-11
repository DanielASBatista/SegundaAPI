using System.ComponentModel.DataAnnotations;
using ProjetoMidasAPI.DTOs.Recorrencia;
using ProjetoMidasAPI.Models;   

namespace ProjetoMidasAPI.DTOs.Recorrencia
{
    public class ConverterLancamentosRecorrenciaDto
    {
        public List<int>? IdsLancamentos { get; set; } 
        public TipoRecorrencia? TipoRecorrencia { get; set; }

        public string? Descricao { get; set; }
    }
}