using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Gerenciador_de_Produtos.Models
{
    [Index(nameof(Nome), IsUnique = true)]
    public class Desenho
    {
        [Key]
        public long DesenhoId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nome { get; set; } = null!;

        [Required]
        public string Descricao { get; set; } = null!;

        public long? Revisao { get; set; }
        public string? Status { get; set; }
        public string? Classificacao { get; set; }
        public long? SolicitacaoAlteracaoId { get; set; }

        // 🔗 Relacionamento com ItemERP
        public int? ItemERPId { get; set; }
        public ItemERP? ItemERP { get; set; }

    }
}
