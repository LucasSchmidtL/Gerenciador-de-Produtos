using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Gerenciador_de_Produtos.Models
{
    public class AgrupadorProduto
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ProdutoId { get; set; }

        [ValidateNever]
        public Produto? Produto { get; set; }

        [Required]
        public int AgrupadorId { get; set; }

        [ValidateNever]
        public Agrupador? Agrupador { get; set; }

        public string? Variavel { get; set; }
        public bool Status { get; set; }
    }
}
