// Models/AgrupadorComponente.cs
using System.ComponentModel.DataAnnotations;

namespace Gerenciador_de_Produtos.Models
{
    public class AgrupadorComponente
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int AgrupadorId { get; set; }
        public Agrupador? Agrupador { get; set; }

        [Required]
        public int ComponenteId { get; set; }
        public Componente? Componente { get; set; }

        public float? Quantidade { get; set; }
        public float? Comprimento { get; set; }
        public float? Profundidade { get; set; }
        public float? Altura { get; set; }
        public bool Status { get; set; }
    }
}
