using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Gerenciador_de_Produtos.Data;
using Gerenciador_de_Produtos.Models;

namespace Gerenciador_de_Produtos.Controllers
{
    public class ItemERPCreateEditViewModel
    {
        public int Id { get; set; }
        public string? ERP { get; set; }
        public string? TipoItem { get; set; }
        public string? ItemERPIdOriginal { get; set; }
        public string? Descricao { get; set; }
        public long? Revisao { get; set; }
        public DateTime? DataCriacao { get; set; }
        public string? Status { get; set; }
        public string? Acabamento { get; set; }
        public long? ChapaAberta { get; set; }
        public float? AreaSuperficial { get; set; }
        public float? PesoLiquidoMetro { get; set; }
        public float? PesoBrutoMetro { get; set; }
        public float? PerimetroSolda { get; set; }
        public long? SRId { get; set; }
        public int? DesenvolvimentoId { get; set; }
        public int? QuantidadeDobras { get; set; }
        public int? MateriaPrimaId { get; set; }
        public float? Altura { get; set; }
        public float? Comprimento { get; set; }
        public float? Profundidade { get; set; }
        public float? ComprimentoMaximo { get; set; }
        public int? Passo { get; set; }
        public int? Classificacao { get; set; }

        public List<int> SelectedTagIds { get; set; } = new();
        public List<SelectListItem> AllTags { get; set; } = new();

        public List<int> SelectedAgrupadorIds { get; set; } = new();
        public List<SelectListItem> AllAgrupadores { get; set; } = new();

        public List<int> SelectedDesenhoIds { get; set; } = new();
        public List<SelectListItem> AllDesenhos { get; set; } = new();

        public List<int> SelectedPerfilIds { get; set; } = new();
        public List<SelectListItem> AllPerfis { get; set; } = new();
    }

    public class ItensERPController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ItensERPController(ApplicationDbContext context) => _context = context;

        public async Task<IActionResult> Index()
        {
            var itens = await _context.ItensERP
                .Include(i => i.Tags)
                .Include(i => i.AgrupadorItensERP).ThenInclude(v => v.Agrupador)
                .Include(i => i.Desenhos)
                .Include(i => i.Perfis)
                .Include(i => i.ComponenteItemERPs).ThenInclude(ci => ci.Componente)
                .ToListAsync();

            return View(itens);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var item = await _context.ItensERP
                .Include(i => i.Tags)
                .Include(i => i.AgrupadorItensERP).ThenInclude(v => v.Agrupador)
                .Include(i => i.Desenhos)
                .Include(i => i.Perfis)
                .Include(i => i.ComponenteItemERPs).ThenInclude(ci => ci.Componente)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (item == null) return NotFound();
            return View(item);
        }

        public async Task<IActionResult> Create()
        {
            var vm = new ItemERPCreateEditViewModel();
            await PopulateSelections(vm);
            return View(vm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ItemERPCreateEditViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                await PopulateSelections(vm);
                return View(vm);
            }

            var item = new ItemERP
            {
                ERP = vm.ERP,
                TipoItem = vm.TipoItem,
                ItemERPIdOriginal = vm.ItemERPIdOriginal,
                Descricao = vm.Descricao,
                Revisao = vm.Revisao,
                DataCriacao = vm.DataCriacao,
                Status = vm.Status,
                Acabamento = vm.Acabamento,
                ChapaAberta = vm.ChapaAberta,
                AreaSuperficial = vm.AreaSuperficial,
                PesoLiquidoMetro = vm.PesoLiquidoMetro,
                PesoBrutoMetro = vm.PesoBrutoMetro,
                PerimetroSolda = vm.PerimetroSolda,
                SRId = vm.SRId,
                DesenvolvimentoId = vm.DesenvolvimentoId,
                QuantidadeDobras = vm.QuantidadeDobras,
                MateriaPrimaId = vm.MateriaPrimaId,
                Altura = vm.Altura,
                Comprimento = vm.Comprimento,
                Profundidade = vm.Profundidade,
                ComprimentoMaximo = vm.ComprimentoMaximo,
                Passo = vm.Passo,
                Classificacao = vm.Classificacao
            };

            _context.ItensERP.Add(item);
            await _context.SaveChangesAsync();

            await AssociateRelations(item, vm, item.Id);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private async Task PopulateSelections(ItemERPCreateEditViewModel vm)
        {
            vm.AllTags = await _context.Tags
                .Select(t => new SelectListItem(t.Nome, t.Id.ToString(), vm.SelectedTagIds.Contains(t.Id)))
                .ToListAsync();

            vm.AllAgrupadores = await _context.Agrupadores
                .Select(a => new SelectListItem(a.Nome, a.Id.ToString(), vm.SelectedAgrupadorIds.Contains(a.Id)))
                .ToListAsync();

            vm.AllDesenhos = await _context.Desenhos
                .Select(d => new SelectListItem(d.Nome, d.DesenhoId.ToString(), vm.SelectedDesenhoIds.Contains((int)d.DesenhoId)))
                .ToListAsync();

            vm.AllPerfis = await _context.Perfis
                .Select(p => new SelectListItem(p.ERP ?? p.Id.ToString(), p.Id.ToString(), vm.SelectedPerfilIds.Contains(p.Id)))
                .ToListAsync();
        }

        private async Task AssociateRelations(ItemERP item, ItemERPCreateEditViewModel vm, int? id = null)
        {
            var tags = await _context.Tags.Where(t => vm.SelectedTagIds.Contains(t.Id)).ToListAsync();
            foreach (var t in tags) item.Tags.Add(t);

            var agrs = await _context.Agrupadores.Where(a => vm.SelectedAgrupadorIds.Contains(a.Id)).ToListAsync();
            foreach (var a in agrs)
                item.AgrupadorItensERP.Add(new AgrupadorItemERP { AgrupadorId = a.Id, Status = true });

            var desenhos = await _context.Desenhos.Where(d => vm.SelectedDesenhoIds.Contains((int)d.DesenhoId)).ToListAsync();
            foreach (var d in desenhos)
                d.ItemERPId = id ?? item.Id;

            var perfis = await _context.Perfis.Where(p => vm.SelectedPerfilIds.Contains(p.Id)).ToListAsync();
            foreach (var p in perfis)
                p.ItemERPId = id ?? item.Id;
        }
    }
}