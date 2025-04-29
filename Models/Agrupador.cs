using System.ComponentModel.DataAnnotations;

namespace Gerenciador_de_Produtos.Models
{
    public class Agrupador
    {
        [Key]
        public int Id { get; set; }

        public string? Grupo { get; set; }
        public int? DesenvolvimentoId { get; set; }
        public int? ItemERPId { get; set; }
        public int? AgrupadorPaiId { get; set; }
        public int Nivel { get; set; }

        // Relacionamentos
        public ICollection<AgrupadorProduto> ProdutoAgrupadores { get; set; }
        public ICollection<VariavelAgrupador> VariaveisAgrupadores { get; set; }
    }
}
