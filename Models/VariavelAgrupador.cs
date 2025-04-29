using System.ComponentModel.DataAnnotations;
using Gerenciador_de_Produtos.Models;

namespace Gerenciador_de_Produtos.Models
{
    public class VariavelAgrupador
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; }
        public string? Descricao { get; set; }
        [Required]
        public string Tipo { get; set; }

        [Required]
        public int AgrupadorId { get; set; }
        public Agrupador Agrupador { get; set; }

        public bool Status { get; set; }
        public string? Valor { get; set; }
    }
}
