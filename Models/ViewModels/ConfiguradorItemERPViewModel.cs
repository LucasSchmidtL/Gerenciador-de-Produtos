using Gerenciador_de_Produtos.Models.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Gerenciador_de_Produtos.Models.ViewModels
{

    public enum StatusItemERP
    {
        [Display(Name = "Ativo")]
        Ativo,

        [Display(Name = "Não Ativo")]
        NaoAtivo,

        [Display(Name = "Obsoleto")]
        Obsoleto
    }



    public class DesenhoLinhaViewModel
    {
        public long Id { get; set; }          // antes era int, agora long
        public string? Nome { get; set; }
        public string? Descricao { get; set; }
        public long? Revisao { get; set; }
        public DateTime? DataCriacao { get; set; }
    }


    public class RevisaoLinhaViewModel
    {
        public int Id { get; set; }
        public long Numero { get; set; }
        public string? MotivoRevisao { get; set; }
        public DateTime? DataRevisao { get; set; }
    }

    public class PerfilLinhaViewModel
    {
        public int Id { get; set; }           // identifica o PerfilItemERP
        public int PerfilId { get; set; }
        public string? Aco { get; set; }
        public List<RevisaoLinhaViewModel> Revisoes { get; set; } = new();
    }

    public class RelatedItemViewModel
    {
        public int Id { get; set; }
        public int ItemERPId { get; set; }
        public long DesenhoId { get; set; }

        public long ItemPaiId { get; set; }
        public string? ItemPaiERP { get; set; }
        public string? DesenhoNome { get; set; }

    }

    public class FamilyComponenteViewModel
    {
        public int Id { get; set; }
        public int ComponenteId { get; set; }
    }

    public class FamilyAgrupadorViewModel
    {
        public int Id { get; set; }
        public int AgrupadorId { get; set; }
    }


    public class ConfiguradorItemERPViewModel
    {
        public int Id { get; set; }
        public string? ERP { get; set; }
        public string? Descricao { get; set; }
        public TipoItem TipoItem { get; set; }

        [Display(Name = "Status")]
        public StatusItemERP? Status { get; set; }



        public string? Acabamento { get; set; }
        public int? Classificacao { get; set; }

        public List<DesenhoLinhaViewModel> Desenhos { get; set; } = new();
        public List<SelectListItem> AllDesenhos { get; set; } = new();
        public List<RevisaoLinhaViewModel> Revisoes { get; set; } = new();

        public long? ChapaAberta { get; set; }
        public float? Espessura { get; set; }
        public string? Aco { get; set; }
        public float? PesoLiquidoMetro { get; set; }
        public float? PesoBrutoMetro { get; set; }
        public float? PesoLinear { get; set; }
        public float? AreaSuperficial { get; set; }
        public int? QuantidadeDobras { get; set; }

        public List<int> SelectedAgrupadorIds { get; set; } = new();
        public List<SelectListItem> AllAgrupadores { get; set; } = new();

        public List<PerfilLinhaViewModel> PerfisSection { get; set; } = new();
        public List<SelectListItem> AllPerfisSection { get; set; } = new();

        public List<RelatedItemViewModel> ItensPintados { get; set; } = new();
        public List<RelatedItemViewModel> ItensGalvanizados { get; set; } = new();
        public List<RelatedItemViewModel> ItensIntegrantes { get; set; } = new();

        public List<RelatedItemViewModel> ApareceComoIntegrante { get; set; } = new();
        public List<RelatedItemViewModel> ApareceComoPintado { get; set; } = new();
        public List<RelatedItemViewModel> ApareceComoGalvanizado { get; set; } = new();




        public List<FamilyComponenteViewModel> ComponentesFamily { get; set; } = new();
        public List<FamilyAgrupadorViewModel> AgrupadoresFamily { get; set; } = new();

        public List<SelectListItem> AllComponentes { get; set; } = new();


        /// <summary>
        /// Dropdown de ItensERP (Seção 05)
        /// </summary>
        public List<SelectListItem> AllItensERP { get; set; } = new();

        /// <summary>
        /// IDs selecionados de desenhos (se você fizer bind no POST de Create/Edit)
        /// </summary>
        public List<long> SelectedDesenhoIds { get; set; } = new();

        /// <summary>
        /// IDs selecionados de perfis (se você fizer bind no POST de Create/Edit)
        /// </summary>
        public List<int> SelectedPerfilIds { get; set; } = new();          

        /// <summary>
        /// Aço de cada perfil para pré-carregar o campo no form
        /// </summary>
        public List<PerfilAcoViewModel> PerfisAco { get; set; } = new();   
    }

    // Auxiliar para mapear o aço de cada perfil
    public class PerfilAcoViewModel
    {
        public int PerfilId { get; set; }
        public string? Aco { get; set; }
    }
}
