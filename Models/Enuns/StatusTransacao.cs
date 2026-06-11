using System.ComponentModel.DataAnnotations;

namespace MidasApi.Models.Enuns
{
    public enum StatusTransacao
    {
        [Display(Name ="Pendente")]
        Pendente = 1,

        [Display(Name ="Confirmada")]
        Confirmada = 2,

        [Display(Name ="Paga")]
        Paga = 3,

        [Display(Name ="Paga com atraso")]
        Paga_Atrasada = 4,

        [Display(Name ="Cancelada")]
        Cancelada = 5
    }
}