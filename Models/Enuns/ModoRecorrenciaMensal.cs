using System.ComponentModel.DataAnnotations;
//Lista os modos de recorrência mensal
namespace ProjetoMidasAPI.Models.Enuns
{
    public enum ModoRecorrenciaMensal
    {
        [Display(Name ="Dia fixo")]
        Dia_Fixo = 1,
        
        [Display(Name ="Intervalo de dias")]
        Intervalo_Dias = 2
    }
}