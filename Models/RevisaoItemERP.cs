using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gerenciador_de_Produtos.Models
{
    public class RevisaoItemERP
    {
        [Key]
        public int Id { get; set; }

        // número da revisão
        public long Numero { get; set; }

        [MaxLength(500)]
        public string? Motivo { get; set; }

        public DateTime Data { get; set; }

        // FK para o ItemERP
        [ForeignKey(nameof(ItemERP))]
        public int ItemERPId { get; set; }
        public ItemERP ItemERP { get; set; } = null!;
    }
}
