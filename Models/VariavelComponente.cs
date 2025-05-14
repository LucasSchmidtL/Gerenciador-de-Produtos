using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Gerenciador_de_Produtos.Models
{
    public class VariavelComponente
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; }

        public string? Descricao { get; set; }

        [Required]
        public string Tipo { get; set; }

        [Required]
        public int ComponenteId { get; set; }

        [ValidateNever]  
        public Componente Componente { get; set; }

        public bool Status { get; set; }
        public string? Valor { get; set; }
    }
}
