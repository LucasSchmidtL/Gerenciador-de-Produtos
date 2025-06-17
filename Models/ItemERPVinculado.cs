using System.ComponentModel.DataAnnotations;

namespace Gerenciador_de_Produtos.Models
{
    public class ItemERPVinculado
    {
        [Key]
        public int Id { get; set; }

        public int? ItemERP_SemAcabamentoId { get; set; }
        public ItemERP? ItemERP_SemAcabamento { get; set; }

        public int? ItemERP_PintadoId { get; set; }
        public ItemERP? ItemERP_Pintado { get; set; }

        public int? ItemERP_GalvanizadoId { get; set; }
        public ItemERP? ItemERP_Galvanizado { get; set; }

        public int? ItemERP_ZincadoId { get; set; }
        public ItemERP? ItemERP_Zincado { get; set; }
    }
}
