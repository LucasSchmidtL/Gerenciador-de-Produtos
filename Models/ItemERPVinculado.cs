using Gerenciador_de_Produtos.Models;
using System.ComponentModel.DataAnnotations;

public class ItemERPVinculado
{
    [Key]
    public int Id { get; set; }

    public int ItemERPId { get; set; }
    public ItemERP ItemERP { get; set; } = null!;

    public int VinculadoId { get; set; }
    public ItemERP Vinculado { get; set; } = null!;

    public string? ItemERPDescricao { get; set; }
    public ItemERP Descricao { get; set; } = null!;

    public TipoVinculoERP Tipo { get; set; }
}
