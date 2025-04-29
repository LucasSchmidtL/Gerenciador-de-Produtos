using System.ComponentModel.DataAnnotations;
using Gerenciador_de_Produtos.Models;

namespace Gerenciador_de_Produtos.Models
{
    public class AgrupadorItemERP
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int AgrupadorId { get; set; }
        public Agrupador Agrupador { get; set; }

        [Required]
        public int ItemERPId { get; set; }
        public ItemERP ItemERP { get; set; }

        public float? Comprimento { get; set; }
        public float? Altura { get; set; }
        public float? Profundidade { get; set; }
        public float? Quantidade { get; set; }
        public bool Status { get; set; }
    }
}
