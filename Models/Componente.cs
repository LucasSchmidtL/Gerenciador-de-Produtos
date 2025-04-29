using System.ComponentModel.DataAnnotations;

namespace Gerenciador_de_Produtos.Models
{
    public class Componente
    {
        [Key]
        public int Id { get; set; }

        public int? Nivel { get; set; }
        public long? NrId { get; set; }
        public long? MrId { get; set; }

        // Relacionamentos
        public ICollection<VariavelComponente> VariaveisComponentes { get; set; }
    }
}
