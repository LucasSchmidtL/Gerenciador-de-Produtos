using System.ComponentModel.DataAnnotations;

namespace Gerenciador_de_Produtos.Models
{
    public class Secao
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Tipo { get; set; }

        public bool SimetricoX { get; set; }
        public bool SimetricoY { get; set; }
        public bool PontoSimetrico { get; set; }
    }
}
