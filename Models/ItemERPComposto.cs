using System.ComponentModel.DataAnnotations;

namespace Gerenciador_de_Produtos.Models
{
    public class ItemERPComposto
    {
        [Key]
        public int Id { get; set; }

        public int ItemERPId_Pai { get; set; }
        public ItemERP ItemERP_Pai { get; set; } = null!;

        public int ItemERPId_Filho { get; set; }
        public ItemERP ItemERP_Filho { get; set; } = null!;

        public float? Comprimento { get; set; }
        public float? Profundidade { get; set; }
        public float? Altura { get; set; }
        public int? Quantidade { get; set; }

        public ICollection<VariaveisItemERPComposto> Variaveis { get; set; } = new List<VariaveisItemERPComposto>();
    }
}
