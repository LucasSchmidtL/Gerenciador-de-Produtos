using System.ComponentModel.DataAnnotations;

namespace Gerenciador_de_Produtos.Models
{
    public class AgrupadorProduto
    {

        [Key]
        public int Id { get; set; }

        [Required]
        public int ProdutoId { get; set; }
        public Produto Produto { get; set; }

        [Required]
        public int AgrupadorId { get; set; }
        public Agrupador Agrupador { get; set; }

        public string? Variavel { get; set; }
        public bool Status { get; set; }
    }
}
