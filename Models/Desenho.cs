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
        public string Nome { get; set; }  // Novo campo de nome para Desenho

        [Required]
        public string Descricao { get; set; }

        public long? Revisao { get; set; }
        public string? Status { get; set; }
        public string? Classificacao { get; set; }
        public long? SolicitacaoAlteracaoId { get; set; }
    }
}
