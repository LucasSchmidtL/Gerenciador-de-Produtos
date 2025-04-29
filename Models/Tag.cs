using System.ComponentModel.DataAnnotations;
using System.Threading;

namespace Gerenciador_de_Produtos.Models
{
    public class Tag
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        public int ItemERPId { get; set; }
        public ItemERP ItemERP { get; set; }
    }
}
