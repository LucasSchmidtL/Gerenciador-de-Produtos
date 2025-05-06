using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Gerenciador_de_Produtos.Models
{
    [Index(nameof(Nome), IsUnique = true)]
    public class Agrupador
    {
        public Agrupador()
        {
            ProdutoAgrupadores = new List<AgrupadorProduto>();
            VariaveisAgrupadores = new List<VariavelAgrupador>();
            AgrupadorItensERP = new List<AgrupadorItemERP>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nome { get; set; }

        public string? Grupo { get; set; }
        public int? DesenvolvimentoId { get; set; }
        public int? ItemERPId { get; set; }
        public int? AgrupadorPaiId { get; set; }
        public int Nivel { get; set; }

        // Relacionamentos
        public ICollection<AgrupadorProduto> ProdutoAgrupadores { get; set; }
        public ICollection<VariavelAgrupador> VariaveisAgrupadores { get; set; }
        public ICollection<AgrupadorItemERP> AgrupadorItensERP { get; set; }
    }
}
