using System.ComponentModel.DataAnnotations;

namespace Gerenciador_de_Produtos.Models
{
    public class Norma
    {
        [Key]
        public long Id { get; set; }

        public long? Equacoes { get; set; }
        public long? Revisao { get; set; }
    }
}
