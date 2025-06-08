namespace Gerenciador_de_Produtos.Models
{
    public class DesenhoItemERP
    {
        public long DesenhoId { get; set; }
        public Desenho? Desenho { get; set; }

        public int ItemERPId { get; set; }
        public ItemERP? ItemERP { get; set; }
    }

}
