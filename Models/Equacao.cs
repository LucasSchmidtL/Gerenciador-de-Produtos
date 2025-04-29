using System.ComponentModel.DataAnnotations;

namespace Gerenciador_de_Produtos.Models
{
    public class Equacao
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public string Funcao { get; set; }

        public long? NormaId { get; set; }
        public long? Entrada { get; set; }
        public long? Saida { get; set; }

        public string? Descricao { get; set; }
        public string? Consideracoes { get; set; }
    }
}
