// Models/PerfilItemERP.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gerenciador_de_Produtos.Models
{
    public class PerfilItemERP
    {
        [Key]
        public int Id { get; set; }

        // FKs
        public int ItemERPId { get; set; }
        public ItemERP ItemERP { get; set; } = null!;

        public int PerfilId { get; set; }
        public Perfil Perfil { get; set; } = null!;

        // Atributos extras
        public string? Aco { get; set; }

        // Relação 1:N para revisões desse par
        public ICollection<RevisaoPerfilItemERP> Revisoes { get; set; } = new List<RevisaoPerfilItemERP>();
    }
}
