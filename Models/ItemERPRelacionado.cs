namespace Gerenciador_de_Produtos.Models
{
    public enum RelacionamentoTipo
    {
        Pintado = 1,
        Galvanizado = 2,
        Integrante = 3
    }

    public class ItemERPRelacionado
    {
        public int Id { get; set; }

        // FK de volta para o Item principal
        public int ItemERPId { get; set; }
        public ItemERP ItemERP { get; set; } = null!;

        // FK para o ItemERP relacionado
        public int RelacionadoId { get; set; }
        public ItemERP Relacionado { get; set; } = null!;

        // Desenho escolhido
        public long DesenhoId { get; set; }
        public Desenho Desenho { get; set; } = null!;

        // Tipo de relacionamento
        public RelacionamentoTipo Tipo { get; set; }
    }
}
