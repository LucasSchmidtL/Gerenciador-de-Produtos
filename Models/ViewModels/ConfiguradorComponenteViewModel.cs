using Gerenciador_de_Produtos.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Gerenciador_de_Produtos.Models.ViewModels
{
    public class ConfiguradorComponenteViewModel
    {
        public Componente Componente { get; set; } = new();
        public List<VariavelComponente> Variables { get; set; } = new();
        public List<ComponenteItemERP> ComponentItems { get; set; } = new();
        public List<SelectListItem> AllItemERPs { get; set; } = new();
    }
}
