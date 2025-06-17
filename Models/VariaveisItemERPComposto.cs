using System.ComponentModel.DataAnnotations;

namespace Gerenciador_de_Produtos.Models
{
    public class VariaveisItemERPComposto
    {
        [Key]
        public int Id { get; set; }

        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public string Tipo { get; set; } = "texto";

        public int ItemERPCompostoId { get; set; }
        public ItemERPComposto ItemERPComposto { get; set; } = null!;

        public bool Status { get; set; }
        public string? Valor { get; set; }
    }
}
