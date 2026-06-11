using System.ComponentModel.DataAnnotations;

namespace ProjetoMidasAPI.Models.Enuns
{
    public enum TipoUsuarioEnum
    {
        [Display(Name ="Autonomo")]
        Autonomo=1,

        [Display(Name ="MEI")]
        MEI=2,

        [Display(Name ="Pequena empresa")]
        PequenaEmpresa=3,   

        [Display(Name ="Média empresa")]
        MediaEmpresa=4,

        [Display(Name ="Grande empresa")]
        GrandeEmpresa=5,

        [Display(Name ="CLT")]
        CLT=6,

        [Display(Name ="Admin")]
        Administrador=7,

        [Display(Name="Visitante")]
        Visitante=8
    }
}