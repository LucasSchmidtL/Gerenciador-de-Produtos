using System.ComponentModel.DataAnnotations;

namespace Gerenciador_de_Produtos.Models
{
    public class Produto
    {
        public Produto()
        {
            ProdutoAgrupadores = new List<AgrupadorProduto>();
            VariaveisProdutos = new List<VariavelProduto>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Familia { get; set; }

        [Required]
        public string NomeComercial { get; set; }

        public string? Precificacao { get; set; }
        public int? DesenvolvimentoId { get; set; }
        public int Nivel { get; set; }

        // Relacionamentos
        public ICollection<AgrupadorProduto> ProdutoAgrupadores { get; set; }
        public ICollection<VariavelProduto> VariaveisProdutos { get; set; }
        // public ICollection<Desenvolvimento> DesenvolvimentoIDs{ get; set; }
    }
}
