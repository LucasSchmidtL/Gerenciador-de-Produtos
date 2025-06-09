using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using Gerenciador_de_Produtos.Models.ViewModels;


namespace Gerenciador_de_Produtos.Models.ViewModels
{
    public class PerfilItemERPViewModel
    {
        public int? Id { get; set; }

        // Informações Básicas
        public string? Descricao { get; set; }
        public int? SecaoId { get; set; }
        public List<SelectListItem> TodasSecoes { get; set; } = new();

        public string? ERP { get; set; } = "";

        // Propriedades Físicas
        public float? Peso { get; set; }
        public float? AreaBruta { get; set; }
        public float? AreaLiq { get; set; }
        public float? AreaEq { get; set; }

        // Geométricas (X)
        public float? Ix { get; set; }
        public float? Sxt { get; set; }
        public float? Sxb { get; set; }
        public float? Zx { get; set; }
        public float? Rx { get; set; }
        public float? yt { get; set; }
        public float? yb { get; set; }

        // Geométricas (Y)
        public float? Ixy { get; set; }
        public float? Iy { get; set; }
        public float? Syl { get; set; }
        public float? Syr { get; set; }
        public float? Zy { get; set; }
        public float? ry { get; set; }

        // Coordenadas
        public float? xl { get; set; }
        public float? xr { get; set; }
        public float? xo { get; set; }
        public float? yo { get; set; }

        // Torção e Enrijecimento
        public float? jx { get; set; }
        public float? jy { get; set; }
        public float? Cw { get; set; }
        public float? J { get; set; }

        // Eixos Equivalentes
        public float? Ixe { get; set; }
        public float? Sxet { get; set; }
        public float? Sxeb { get; set; }
        public float? lye { get; set; }
        public float? Syel { get; set; }
        public float? Syer { get; set; }

        // Parâmetros Auxiliares
        public float? p1 { get; set; }
        public float? p2 { get; set; }
        public float? p3 { get; set; }

        public bool SimetricoX { get; set; }
        public bool SimetricoY { get; set; }

        // Vínculo com Itens ERP
        public List<int> ItensERPSelecionados { get; set; } = new();
        public List<SelectListItem> TodosItensERP { get; set; } = new();
        public List<int> SelectedItemERPIds { get; set; } = new();


        public int? DesenhoId { get; set; }
        public string? DesenhoNome { get; set; } 
        public List<SelectListItem> TodosDesenhos { get; set; } = new();
        public List<long> DesenhosSelecionados { get; set; } = new();


    }
}