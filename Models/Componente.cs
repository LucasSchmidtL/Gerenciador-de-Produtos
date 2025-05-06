using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Gerenciador_de_Produtos.Models
{
    [Index(nameof(Nome), IsUnique = true)]
    public class Componente
    {
        public Componente()
        {
            VariaveisComponentes = new List<VariavelComponente>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nome { get; set; }  // Nome do componente

        [Required]
        [MaxLength(200)]
        public string Descricao { get; set; }  // Novo campo de descrição

        public int? Nivel { get; set; }

        // Relacionamentos
        public ICollection<VariavelComponente> VariaveisComponentes { get; set; }
    }
}
