using Gerenciador_de_Produtos.Models;
using System.ComponentModel.DataAnnotations;

public class ItemERPVinculado
{
    [Key]
    public int Id { get; set; }

    public int? ItemERPId { get; set; }
    public ItemERP? ItemERP { get; set; } = null!;

    public int VinculadoId { get; set; }
    public ItemERP? Vinculado { get; set; } = null!;

    public int? DesenhoId { get; set; }
    public Desenho? Desenho { get; set; } // Se quiser navegar pelo objeto opcionalmente


    public string? ItemERPDescricao { get; set; }

    public TipoVinculoERP Tipo { get; set; }
}
