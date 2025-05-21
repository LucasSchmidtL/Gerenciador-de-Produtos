// Models/RevisaoPerfilItemERP.cs
using System.ComponentModel.DataAnnotations;

namespace Gerenciador_de_Produtos.Models
{
    public class RevisaoPerfilItemERP
    {
        [Key]
        public int Id { get; set; }

        public int PerfilItemERPId { get; set; }
        public PerfilItemERP PerfilItemERP { get; set; } = null!;

        public long Numero { get; set; }
        public string? Motivo { get; set; }
        public DateTime? Data { get; set; }
    }
}
