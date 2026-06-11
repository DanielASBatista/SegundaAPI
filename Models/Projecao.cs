using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;


namespace ProjetoMidasAPI.Models
{
    public class Projecao
    {
        [Key] // Define a chave primária da tabela
        public int IdProjecao { get; set; }

        [JsonIgnore]
        public Usuario? Usuario { get; set; }

        public int IdUsuario { get; set; } // Chave estrangeira para o usuário responsável pela projeção

        [Required, MaxLength(200)] // Campo obrigatório, máximo 200 caracteres
        public string Titulo { get; set; } = string.Empty;

        /* Ok, no esquema do banco de dados o nome da projeção tá como dsProjecao. 
        Caso a mudança se perdure na api, o modelo pra ele ta fica aqui entre essas aspas.
        Alias vários atributos tem nome diferente no esquema do banco de dados, preciso decidir se mudo no esquema o use mudo na API.
        As datas de inicio e de final da projeção precisam ser analisadas por causa dessa mesma questão.
        [Required, MaxLength(200)] // Campo obrigatório, máximo 200 caracteres
        public string dsProjecao { get; set; } = string.Empty;*/
       
        [Column(TypeName = "decimal(18,2)")] // Define o tipo decimal no banco (18 é o número total de digitos e 2 são as casas decimais)
        public decimal ValorPrevisto { get; set; }

        public DateTime DataReferencia { get; set; } = DateTime.UtcNow; // Data de referência, campo de busca

        public DateTime DataCriacao { get; set; } = DateTime.UtcNow; // Data/hora de criação, campo de busca e será inserido automaticamente a do sistema
    
        public int? UsuarioResponsavel {get; set;}
    }
}
