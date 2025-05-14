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

        public ICollection<ItemERP> ItemERPs { get; set; } = new List<ItemERP>();
    }
}
