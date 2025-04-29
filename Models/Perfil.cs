using System.ComponentModel.DataAnnotations;

namespace Gerenciador_de_Produtos.Models
{
    public class Perfil
    {
        [Key]
        public int Id { get; set; }

        public string? ERP { get; set; }
        public string? Desenho { get; set; }
        public string? Descricao { get; set; }
        public string? TipoSecao { get; set; }
        public float? Peso { get; set; }
        public float? AreaBruta { get; set; }
        public float? AreaLiq { get; set; }
        public float? AreaEq { get; set; }
        public float? Ix { get; set; }
        public float? Sxt { get; set; }
        public float? Sxb { get; set; }
        public float? Zx { get; set; }
        public float? Rx { get; set; }
        public float? yt { get; set; }
        public float? yb { get; set; }
        public float? Ixy { get; set; }
        public float? Iy { get; set; }
        public float? Syl { get; set; }
        public float? Syr { get; set; }
        public float? Zy { get; set; }
        public float? ry { get; set; }
        public float? xl { get; set; }
        public float? xr { get; set; }
        public float? xo { get; set; }
        public float? yo { get; set; }
        public float? jx { get; set; }
        public float? jy { get; set; }
        public float? Cw { get; set; }
        public float? J { get; set; }
        public float? Ixe { get; set; }
        public float? Sxet { get; set; }
        public float? Sxeb { get; set; }
        public float? lye { get; set; }
        public float? Syel { get; set; }
        public float? Syer { get; set; }
        public float? p1 { get; set; }
        public float? p2 { get; set; }
        public float? p3 { get; set; }
        public bool? SimetricoX { get; set; }
        public bool? SimetricoY { get; set; }
    }
}
