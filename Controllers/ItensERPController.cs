using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Gerenciador_de_Produtos.Data;
using Gerenciador_de_Produtos.Models;

namespace Gerenciador_de_Produtos.Controllers
{
    // ViewModel para Create/Edit de ItemERP com Tags e Agrupadores
    public class ItemERPCreateEditViewModel
    {
        public int Id { get; set; }
        public string? ERP { get; set; }
        public string? TipoItem { get; set; }
        public string? ItemERPIdOriginal { get; set; }
        public long? DesenhoId { get; set; }
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

        // Seleções múltiplas
        public List<int> SelectedTagIds { get; set; } = new();
        public List<SelectListItem> AllTags { get; set; } = new();
        public List<int> SelectedAgrupadorIds { get; set; } = new();
        public List<SelectListItem> AllAgrupadores { get; set; } = new();
    }

    public class ItensERPController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ItensERPController(ApplicationDbContext context) => _context = context;

        // GET: ItensERP
        public async Task<IActionResult> Index()
        {
            var itens = await _context.ItensERP
                .Include(i => i.Tags)
                .Include(i => i.AgrupadorItensERP).ThenInclude(v => v.Agrupador)
                .ToListAsync();
            return View(itens);
        }

        // GET: ItensERP/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var item = await _context.ItensERP
                .Include(i => i.Tags)
                .Include(i => i.AgrupadorItensERP).ThenInclude(v => v.Agrupador)
                .FirstOrDefaultAsync(i => i.Id == id);
            if (item == null) return NotFound();
            return View(item);
        }

        // GET: ItensERP/Create
        public async Task<IActionResult> Create()
        {
            var vm = new ItemERPCreateEditViewModel();
            vm.AllTags = await _context.Tags
                .Select(t => new SelectListItem(t.Nome, t.Id.ToString()))
                .ToListAsync();
            vm.AllAgrupadores = await _context.Agrupadores
                .Select(a => new SelectListItem(a.Nome, a.Id.ToString()))
                .ToListAsync();
            return View(vm);
        }

        // POST: ItensERP/Create
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ItemERPCreateEditViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.AllTags = await _context.Tags
                    .Select(t => new SelectListItem(t.Nome, t.Id.ToString(), vm.SelectedTagIds.Contains(t.Id)))
                    .ToListAsync();
                vm.AllAgrupadores = await _context.Agrupadores
                    .Select(a => new SelectListItem(a.Nome, a.Id.ToString(), vm.SelectedAgrupadorIds.Contains(a.Id)))
                    .ToListAsync();
                return View(vm);
            }

            var item = new ItemERP
            {
                ERP = vm.ERP,
                TipoItem = vm.TipoItem,
                ItemERPIdOriginal = vm.ItemERPIdOriginal,
                DesenhoId = vm.DesenhoId,
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

            var tags = await _context.Tags.Where(t => vm.SelectedTagIds.Contains(t.Id)).ToListAsync();
            foreach (var t in tags) item.Tags.Add(t);

            var agregs = await _context.Agrupadores.Where(a => vm.SelectedAgrupadorIds.Contains(a.Id)).ToListAsync();
            foreach (var a in agregs)
                item.AgrupadorItensERP.Add(new AgrupadorItemERP { AgrupadorId = a.Id, Status = true });

            _context.ItensERP.Add(item);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: ItensERP/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var item = await _context.ItensERP
                .Include(i => i.Tags)
                .Include(i => i.AgrupadorItensERP)
                .FirstOrDefaultAsync(i => i.Id == id);
            if (item == null) return NotFound();

            var vm = new ItemERPCreateEditViewModel
            {
                Id = item.Id,
                ERP = item.ERP,
                TipoItem = item.TipoItem,
                ItemERPIdOriginal = item.ItemERPIdOriginal,
                DesenhoId = item.DesenhoId,
                Descricao = item.Descricao,
                Revisao = item.Revisao,
                DataCriacao = item.DataCriacao,
                Status = item.Status,
                Acabamento = item.Acabamento,
                ChapaAberta = item.ChapaAberta,
                AreaSuperficial = item.AreaSuperficial,
                PesoLiquidoMetro = item.PesoLiquidoMetro,
                PesoBrutoMetro = item.PesoBrutoMetro,
                PerimetroSolda = item.PerimetroSolda,
                SRId = item.SRId,
                DesenvolvimentoId = item.DesenvolvimentoId,
                QuantidadeDobras = item.QuantidadeDobras,
                MateriaPrimaId = item.MateriaPrimaId,
                Altura = item.Altura,
                Comprimento = item.Comprimento,
                Profundidade = item.Profundidade,
                ComprimentoMaximo = item.ComprimentoMaximo,
                Passo = item.Passo,
                Classificacao = item.Classificacao,
                SelectedTagIds = item.Tags.Select(t => t.Id).ToList(),
                SelectedAgrupadorIds = item.AgrupadorItensERP.Select(a => a.AgrupadorId).ToList()
            };
            vm.AllTags = await _context.Tags
                .Select(t => new SelectListItem(t.Nome, t.Id.ToString(), vm.SelectedTagIds.Contains(t.Id)))
                .ToListAsync();
            vm.AllAgrupadores = await _context.Agrupadores
                .Select(a => new SelectListItem(a.Nome, a.Id.ToString(), vm.SelectedAgrupadorIds.Contains(a.Id)))
                .ToListAsync();

            return View(vm);
        }

        // POST: ItensERP/Edit/5
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ItemERPCreateEditViewModel vm)
        {
            if (id != vm.Id) return NotFound();
            if (!ModelState.IsValid)
            {
                vm.AllTags = await _context.Tags
                    .Select(t => new SelectListItem(t.Nome, t.Id.ToString(), vm.SelectedTagIds.Contains(t.Id)))
                    .ToListAsync();
                vm.AllAgrupadores = await _context.Agrupadores
                    .Select(a => new SelectListItem(a.Nome, a.Id.ToString(), vm.SelectedAgrupadorIds.Contains(a.Id)))
                    .ToListAsync();
                return View(vm);
            }

            var item = await _context.ItensERP
                .Include(i => i.Tags)
                .Include(i => i.AgrupadorItensERP)
                .FirstOrDefaultAsync(i => i.Id == id);
            if (item == null) return NotFound();

            // Atualiza campos básicos
            item.ERP = vm.ERP;
            item.TipoItem = vm.TipoItem;
            item.ItemERPIdOriginal = vm.ItemERPIdOriginal;
            item.DesenhoId = vm.DesenhoId;
            item.Descricao = vm.Descricao;
            item.Revisao = vm.Revisao;
            item.DataCriacao = vm.DataCriacao;
            item.Status = vm.Status;
            item.Acabamento = vm.Acabamento;
            item.ChapaAberta = vm.ChapaAberta;
            item.AreaSuperficial = vm.AreaSuperficial;
            item.PesoLiquidoMetro = vm.PesoLiquidoMetro;
            item.PesoBrutoMetro = vm.PesoBrutoMetro;
            item.PerimetroSolda = vm.PerimetroSolda;
            item.SRId = vm.SRId;
            item.DesenvolvimentoId = vm.DesenvolvimentoId;
            item.QuantidadeDobras = vm.QuantidadeDobras;
            item.MateriaPrimaId = vm.MateriaPrimaId;
            item.Altura = vm.Altura;
            item.Comprimento = vm.Comprimento;
            item.Profundidade = vm.Profundidade;
            item.ComprimentoMaximo = vm.ComprimentoMaximo;
            item.Passo = vm.Passo;
            item.Classificacao = vm.Classificacao;

            // Sincroniza Tags
            item.Tags.Clear();
            var tagsEd = await _context.Tags.Where(t => vm.SelectedTagIds.Contains(t.Id)).ToListAsync();
            foreach (var t in tagsEd) item.Tags.Add(t);

            // Sincroniza Agrupadores
            item.AgrupadorItensERP.Clear();
            var agregsEd = await _context.Agrupadores.Where(a => vm.SelectedAgrupadorIds.Contains(a.Id)).ToListAsync();
            foreach (var a in agregsEd)
                item.AgrupadorItensERP.Add(new AgrupadorItemERP { AgrupadorId = a.Id, Status = true });

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: ItensERP/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var item = await _context.ItensERP
                .Include(i => i.Tags)
                .Include(i => i.AgrupadorItensERP).ThenInclude(v => v.Agrupador)
                .FirstOrDefaultAsync(i => i.Id == id);
            if (item == null) return NotFound();
            return View(item);
        }

        // POST: ItensERP/Delete/5
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.ItensERP.FindAsync(id);
            if (item != null)
            {
                _context.ItensERP.Remove(item);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ItemERPExists(int id) => _context.ItensERP.Any(e => e.Id == id);
    }
}