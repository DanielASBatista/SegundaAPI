using System.ComponentModel.DataAnnotations;

namespace ProjetoMidasAPI.Models.Enuns
{
    public enum FrequenciaRecorrencia
    {
        [Display(Name ="Diaria")]
        Diaria = 1,
        
        [Display(Name ="Semanal")]
        Semanal = 2,
        
        [Display(Name ="Mensal")]
        Mensal = 3,
        
        [Display(Name ="Quinzenal")]
        Quinzenal = 4,
        
        [Display(Name ="Anual")]
        Anual = 5
    }
}