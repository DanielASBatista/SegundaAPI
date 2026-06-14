namespace MidasApi.Models.Enuns
{
    public enum CategoriaGastoEnum
    {
        [System.ComponentModel.DataAnnotations.Display(Name = "Alimentação")]
        Alimentacao = 1,

        [System.ComponentModel.DataAnnotations.Display(Name = "Moradia")]
        Moradia = 2,

        [System.ComponentModel.DataAnnotations.Display(Name = "Transporte")]
        Transporte = 3,

        [System.ComponentModel.DataAnnotations.Display(Name = "Saúde")]
        Saude = 4,

        [System.ComponentModel.DataAnnotations.Display(Name = "Educação")]
        Educacao = 5,

        [System.ComponentModel.DataAnnotations.Display(Name = "Lazer")]
        Lazer = 6,

        [System.ComponentModel.DataAnnotations.Display(Name = "Vestuário")]
        Vestuario = 7,

        [System.ComponentModel.DataAnnotations.Display(Name = "Utilidades")]
        Utilidades = 8,

        [System.ComponentModel.DataAnnotations.Display(Name = "Assinaturas")]
        Assinaturas = 9,

        [System.ComponentModel.DataAnnotations.Display(Name = "Impostos")]
        Impostos = 10,

        [System.ComponentModel.DataAnnotations.Display(Name = "Investimentos")]
        Investimentos = 11,

        [System.ComponentModel.DataAnnotations.Display(Name = "Salário")]
        Salario = 12,

        [System.ComponentModel.DataAnnotations.Display(Name = "Freelance")]
        Freelance = 13,

        [System.ComponentModel.DataAnnotations.Display(Name = "Outros")]
        Outros = 99
    }
}
