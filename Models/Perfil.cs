using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gerenciador_de_Produtos.Models
{
    public class Perfil
    {
        [Key]
        public int Id { get; set; }

        #region Informações Básicas

        [Display(Name = "Desenho")]
        public string? Desenho { get; set; }

        [Display(Name = "Descrição")]
        public string? Descricao { get; set; }

        [Display(Name = "Tipo de Seção")]
        public string? TipoSecao { get; set; }

        #endregion

        #region Propriedades Físicas

        [Display(Name = "Peso (kg/m)")]
        public float? Peso { get; set; }

        [Display(Name = "Área Bruta")]
        public float? AreaBruta { get; set; }

        [Display(Name = "Área Líquida")]
        public float? AreaLiq { get; set; }

        [Display(Name = "Área Equivalente")]
        public float? AreaEq { get; set; }

        #endregion

        #region Propriedades Geométricas (X)

        public float? Ix { get; set; }
        public float? Sxt { get; set; }
        public float? Sxb { get; set; }
        public float? Zx { get; set; }
        public float? Rx { get; set; }
        public float? yt { get; set; }
        public float? yb { get; set; }

        #endregion

        #region Propriedades Geométricas (Y)

        public float? Ixy { get; set; }
        public float? Iy { get; set; }
        public float? Syl { get; set; }
        public float? Syr { get; set; }
        public float? Zy { get; set; }
        public float? ry { get; set; }

        #endregion

        #region Coordenadas

        public float? xl { get; set; }
        public float? xr { get; set; }
        public float? xo { get; set; }
        public float? yo { get; set; }

        #endregion

        #region Torção e Enrijecimento

        public float? jx { get; set; }
        public float? jy { get; set; }
        public float? Cw { get; set; }
        public float? J { get; set; }

        #endregion

        #region Eixos Equivalentes

        public float? Ixe { get; set; }
        public float? Sxet { get; set; }
        public float? Sxeb { get; set; }
        public float? lye { get; set; }
        public float? Syel { get; set; }
        public float? Syer { get; set; }

        #endregion

        #region Parâmetros Auxiliares

        public float? p1 { get; set; }
        public float? p2 { get; set; }
        public float? p3 { get; set; }

        [Display(Name = "Simétrico em X")]
        public bool? SimetricoX { get; set; }

        [Display(Name = "Simétrico em Y")]
        public bool? SimetricoY { get; set; }

        #endregion

        #region Relacionamento

        [Display(Name = "Item ERP")]
        public int? ItemERPId { get; set; }

        public ItemERP? ItemERP { get; set; } = null!;

        public ICollection<PerfilItemERP> PerfilItemERPs { get; set; } = new List<PerfilItemERP>();
        #endregion 
    }
}
