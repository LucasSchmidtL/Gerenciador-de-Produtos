using System.ComponentModel.DataAnnotations;

namespace Gerenciador_de_Produtos.Models
{
    public class ItemERP
    {
        [Key]
        public int Id { get; set; }

        public string? ERP { get; set; }
        public string? TipoItem { get; set; }
        public string? ItemERPIdOriginal { get; set; }
        public long? DesenhoId { get; set; }
        public string? Descricao { get; set; }
        public long? Revisao { get; set; }
        public DateTime? DataCriacao { get; set; }
        public string? Status { get; set; }
        public string? Acabamento { get; set; }
        public long? ChapaAberta { get; set; }
        public float? AreaSuperficial { get; set; }
        public float? PesoLiquidoMetro { get; set; }
        public float? PesoBrutoMetro { get; set; }
        public float? PerimetroSolda { get; set; }
        public long? SRId { get; set; }
        public int? DesenvolvimentoId { get; set; }
        public int? QuantidadeDobras { get; set; }
        public int? MateriaPrimaId { get; set; }
        public long? TagId { get; set; }
        public float? Altura { get; set; }
        public float? Comprimento { get; set; }
        public float? Profundidade { get; set; }
        public float? ComprimentoMaximo { get; set; }
        public int? Passo { get; set; }
        public int? Classificacao { get; set; }

        // 🔗 Relacionamento com Agrupadores
        public ICollection<AgrupadorItemERP> AgrupadorItensERP { get; set; } = new List<AgrupadorItemERP>();
    }
}
