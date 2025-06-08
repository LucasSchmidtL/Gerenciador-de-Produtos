using Microsoft.AspNetCore.Mvc.Rendering;

namespace Gerenciador_de_Produtos.Models.ViewModels
{
    public class DesenhoViewModel
    {
        public long? DesenhoId { get; set; }
        public string? Nome { get; set; }
        public string? Descricao { get; set; }
        public long? Revisao { get; set; }
        public StatusDesenho Status { get; set; }
        public string? Classificacao { get; set; }
        public long? SolicitacaoAlteracaoId { get; set; }

        public List<int> SelectedItemERPIds { get; set; } = new();
        public List<SelectListItem> AllItemERPs { get; set; } = new();
    }


}
