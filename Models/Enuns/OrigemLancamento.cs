using System.ComponentModel.DataAnnotations;

namespace MidasApi.Models.Enuns
{
    public enum OrigemLancamento
    {
        
        [Display(Name ="Manual")]
        Manual = 1,

        [Display(Name ="Recorrencia")]
        Recorrencia = 2,

        [Display(Name ="Projecao")]
        Projecao = 3,

        [Display(Name ="Emprestimo")]
        Emprestimo = 4
    }
}