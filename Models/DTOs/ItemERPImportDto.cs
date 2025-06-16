namespace Gerenciador_de_Produtos.Models.DTOs
{
    public class ItemERPImportDto
    {
        public string ERP { get; set; }
        public string Descricao { get; set; }
        public float PesoLiquidoMetro { get; set; }
        public float PesoBrutoMetro { get; set; }
        public DateTime? DataCriacao { get; set; } // 
    }
}
