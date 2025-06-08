using Gerenciador_de_Produtos.Models.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Gerenciador_de_Produtos.Models.ViewModels
{
    public class ItemERPCreateEditViewModel
    {
            public int Id { get; set; }
            public string? ERP { get; set; }
            public TipoItem TipoItem { get; set; }
            public string? ItemERPIdOriginal { get; set; }
            public string? Descricao { get; set; }
            public long? Revisao { get; set; }
            public DateTime? DataCriacao { get; set; }
            public StatusItemERP Status { get; set; }
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
            public float? Altura { get; set; }
            public float? Espessura { get; set; }
            public float? Comprimento { get; set; }
            public float? Profundidade { get; set; }
            public float? ComprimentoMaximo { get; set; }
            public int? Passo { get; set; }
            public int? Classificacao { get; set; }

            public List<int> SelectedTagIds { get; set; } = new();
            public List<SelectListItem> AllTags { get; set; } = new();

            public List<int> SelectedAgrupadorIds { get; set; } = new();
            public List<SelectListItem> AllAgrupadores { get; set; } = new();

            public List<int> SelectedDesenhoIds { get; set; } = new();
            public List<SelectListItem> AllDesenhos { get; set; } = new();

            public List<int> SelectedPerfilIds { get; set; } = new();
            public List<SelectListItem> AllPerfis { get; set; } = new();
 
    }
}
