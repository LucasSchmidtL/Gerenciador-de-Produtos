using Microsoft.AspNetCore.Mvc.Rendering;
using Gerenciador_de_Produtos.Models.ViewModels;


namespace Gerenciador_de_Produtos.Models.ViewModels
{
    public class TagItensERPViewModel
    {
        public int TagId { get; set; }
        public string Nome { get; set; }

        public List<long> SelectedItemERPIds { get; set; } = new();
        public List<SelectListItem> AllItensERP { get; set; } = new();
    }


    public class TagEditViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; } = null!;
        public List<int> SelectedItemERPIds { get; set; } = new();
        public List<SelectListItem> AllItemERPs { get; set; } = new();
    }

}
