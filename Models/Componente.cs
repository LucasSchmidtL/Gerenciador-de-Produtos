// Models/Componente.cs
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Gerenciador_de_Produtos.Models
{
    [Index(nameof(Nome), IsUnique = true)]
    public class Componente
    {
        public Componente()
        {
            VariaveisComponentes = new List<VariavelComponente>();
            AgrupadorComponentes = new List<AgrupadorComponente>();
            ComponenteItensERP = new List<ComponenteItemERP>();   // ← Adicionado
        }

        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Nome { get; set; } = null!;

        [Required, MaxLength(200)]
        public string Descricao { get; set; } = null!;

        public int? Nivel { get; set; }

        public ICollection<VariavelComponente> VariaveisComponentes { get; set; }
        public ICollection<AgrupadorComponente> AgrupadorComponentes { get; set; }
        public ICollection<ComponenteItemERP> ComponenteItensERP { get; set; }  // ← Adicionado
    }
}
