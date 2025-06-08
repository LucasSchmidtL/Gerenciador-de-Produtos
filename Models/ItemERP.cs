// Models/ItemERP.cs
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Gerenciador_de_Produtos.Models.ViewModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gerenciador_de_Produtos.Models
{
    public class ItemERP
    {
        [Key]
        public int Id { get; set; }

        public string? ERP { get; set; }
        public string? TipoItem { get; set; }
        public string? ItemERPIdOriginal { get; set; }
        public string? Descricao { get; set; }
        public long? Revisao { get; set; }
        public DateTime? DataCriacao { get; set; }

        [Required]
        [EnumDataType(typeof(StatusItemERP))]
        [Column(TypeName = "nvarchar(20)")]
        public StatusItemERP Status { get; set; }


        // radio “Com Acabamento” / “Sem Acabamento”
        public string? Acabamento { get; set; }

        // **texto livre** para o aço
        public string? Aco { get; set; }        // <<--- aqui

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

        // relacionamentos…
        public ICollection<AgrupadorItemERP> AgrupadorItensERP { get; set; } = new List<AgrupadorItemERP>();
        public ICollection<Tag> Tags { get; set; } = new List<Tag>();
        public ICollection<ComponenteItemERP> ComponenteItemERPs { get; set; } = new List<ComponenteItemERP>();
        public ICollection<RevisaoItemERP> Revisoes { get; set; } = new List<RevisaoItemERP>();
        public ICollection<Desenho> Desenhos { get; set; } = new List<Desenho>();
        public ICollection<Perfil> Perfis { get; set; } = new List<Perfil>();
        public ICollection<PerfilItemERP> PerfilItemERPs { get; set; } = new List<PerfilItemERP>();
        public List<ItemERPRelacionado> ItensRelacionados { get; set; } = new();
    }

}
