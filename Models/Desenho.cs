using System.ComponentModel.DataAnnotations;

namespace Gerenciador_de_Produtos.Models
{
    public class Desenho
    {
        [Key]
        public long DesenhoId { get; set; }

        [Required]
        public string Descricao { get; set; }

        public long? Revisao { get; set; }
        public string? Status { get; set; }
        public string? Classificacao { get; set; }
        public long? SolicitacaoAlteracaoId { get; set; }
    }
}
