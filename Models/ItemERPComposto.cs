using Gerenciador_de_Produtos.Models;
using System.ComponentModel.DataAnnotations;

public class ItemERPComposto
{
    [Key]
    public int Id { get; set; }

    public int ItemPaiId { get; set; }
    public ItemERP ItemPai { get; set; } = null!;

    public int ItemFilhoId { get; set; }
    public ItemERP ItemFilho { get; set; } = null!;

    public float? Comprimento { get; set; }
    public float? Profundidade { get; set; }
    public float? Altura { get; set; }
    public int? Quantidade { get; set; }

    public string? Unidade { get; set; }

    public ICollection<VariaveisItemERPComposto> Variaveis { get; set; } = new List<VariaveisItemERPComposto>();
}
