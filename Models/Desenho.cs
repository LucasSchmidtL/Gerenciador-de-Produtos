using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Gerenciador_de_Produtos.Models
{
    // Se quiser deixar Nome único por ItemERP apenas, mude IsUnique para false por enquanto
    [Index(nameof(Nome), IsUnique = false)]
    public class Desenho
    {
        [Key]
        public long DesenhoId { get; set; }

        [MaxLength(100)]
        public string? Nome { get; set; }        // agora opcional

        public string? Descricao { get; set; }   // agora opcional

        public DateTime? DataCriacao { get; set; }
        public long? Revisao { get; set; }

        [Required]
        [EnumDataType(typeof(StatusDesenho))]
        [Column(TypeName = "nvarchar(20)")]
        public StatusDesenho Status { get; set; }

        public string? Classificacao { get; set; }
        public long? SolicitacaoAlteracaoId { get; set; }

        public int? ItemERPId { get; set; }
        public ItemERP? ItemERP { get; set; }
    }

    public enum StatusDesenho
    {
        [Display(Name = "Ativo")]
        Ativo,

        [Display(Name = "Não Ativo")]
        NaoAtivo,

        [Display(Name = "Obsoleto")]
        Obsoleto
    }

}
