using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Gerenciador_de_Produtos.Data;
using Gerenciador_de_Produtos.Models;
using Gerenciador_de_Produtos.Models.ViewModels;

namespace Gerenciador_de_Produtos.Controllers
{
    public class ItensERPController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ItensERPController> _logger;

        public ItensERPController(ApplicationDbContext context, ILogger<ItensERPController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: ItensERP
        public async Task<IActionResult> Index()
        {
            var itens = await _context.ItensERP
                .Include(i => i.Tags)
                .Include(i => i.AgrupadorItensERP).ThenInclude(ai => ai.Agrupador)
                .Include(i => i.Desenhos)
                .Include(i => i.Revisoes)
                .Include(i => i.PerfilItemERPs).ThenInclude(pi => pi.Revisoes)
                .Include(i => i.PerfilItemERPs).ThenInclude(pi => pi.Perfil)
                .ToListAsync();
            return View(itens);
        }

        // GET: ItensERP/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var item = await _context.ItensERP
                .Include(i => i.Tags)
                .Include(i => i.AgrupadorItensERP).ThenInclude(ai => ai.Agrupador)
                .Include(i => i.Desenhos)
                .Include(i => i.Revisoes)
                .Include(i => i.PerfilItemERPs).ThenInclude(pi => pi.Revisoes)
                .Include(i => i.PerfilItemERPs).ThenInclude(pi => pi.Perfil)
                .FirstOrDefaultAsync(i => i.Id == id);
            if (item == null) return NotFound();
            return View(item);
        }

        // GET: ItensERP/Create
        public IActionResult Create()
        {
            var vm = new ItemERPCreateEditViewModel();
            PopulateSelections(vm);
            return View(vm);
        }

        // POST: ItensERP/Create
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ItemERPCreateEditViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                PopulateSelections(vm);
                return View(vm);
            }

            var item = new ItemERP
            {
                ERP = vm.ERP,
                Descricao = vm.Descricao,
                TipoItem = vm.TipoItem,
                Acabamento = vm.Acabamento,
                Classificacao = vm.Classificacao,
                ChapaAberta = vm.ChapaAberta,
                Altura = vm.Espessura,
                AreaSuperficial = vm.AreaSuperficial,
                PesoLiquidoMetro = vm.PesoLiquidoMetro,
                PesoBrutoMetro = vm.PesoBrutoMetro,
                QuantidadeDobras = vm.QuantidadeDobras
            };

            _context.ItensERP.Add(item);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ConfiguradorItemERP), new { id = item.Id });
        }

        // GET: ItensERP/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var item = await _context.ItensERP
                .Include(i => i.Tags)
                .Include(i => i.AgrupadorItensERP)
                .Include(i => i.Desenhos)
                .Include(i => i.PerfilItemERPs)
                .FirstOrDefaultAsync(i => i.Id == id);
            if (item == null) return NotFound();

            var vm = new ItemERPCreateEditViewModel
            {
                Id = item.Id,
                ERP = item.ERP,
                Descricao = item.Descricao,
                TipoItem = item.TipoItem,
                Acabamento = item.Acabamento,
                Classificacao = item.Classificacao,
                ChapaAberta = item.ChapaAberta,
                Espessura = item.Altura,
                AreaSuperficial = item.AreaSuperficial,
                PesoLiquidoMetro = item.PesoLiquidoMetro,
                PesoBrutoMetro = item.PesoBrutoMetro,
                QuantidadeDobras = item.QuantidadeDobras,
                SelectedTagIds = item.Tags.Select(t => t.Id).ToList(),
                SelectedAgrupadorIds = item.AgrupadorItensERP.Select(ai => ai.AgrupadorId).ToList(),
                SelectedDesenhoIds = item.Desenhos.Select(d => (int)d.DesenhoId).ToList(),
                SelectedPerfilIds = item.PerfilItemERPs.Select(pi => pi.PerfilId).ToList()
            };
            PopulateSelections(vm);
            return View(vm);
        }

        // POST: ItensERP/Edit/5
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ItemERPCreateEditViewModel vm)
        {
            if (id != vm.Id) return BadRequest();
            if (!ModelState.IsValid)
            {
                PopulateSelections(vm);
                return View(vm);
            }

            var item = await _context.ItensERP
                .Include(i => i.Tags)
                .Include(i => i.AgrupadorItensERP)
                .Include(i => i.Desenhos)
                .Include(i => i.PerfilItemERPs)
                .FirstOrDefaultAsync(i => i.Id == id);
            if (item == null) return NotFound();

            // Atualiza campos básicos
            item.ERP = vm.ERP;
            item.Descricao = vm.Descricao;
            item.TipoItem = vm.TipoItem;
            item.Acabamento = vm.Acabamento;
            item.Classificacao = vm.Classificacao;
            item.ChapaAberta = vm.ChapaAberta;
            item.Altura = vm.Espessura;
            item.AreaSuperficial = vm.AreaSuperficial;
            item.PesoLiquidoMetro = vm.PesoLiquidoMetro;
            item.PesoBrutoMetro = vm.PesoBrutoMetro;
            item.QuantidadeDobras = vm.QuantidadeDobras;

            // Tags
            item.Tags.Clear();
            var tags = await _context.Tags.Where(t => vm.SelectedTagIds.Contains(t.Id)).ToListAsync();
            tags.ForEach(t => item.Tags.Add(t));

            // Agrupadores
            _context.AgrupadorItemERPs.RemoveRange(item.AgrupadorItensERP);
            foreach (var agrId in vm.SelectedAgrupadorIds)
                item.AgrupadorItensERP.Add(new AgrupadorItemERP { ItemERPId = item.Id, AgrupadorId = agrId, Status = true });

            // Desenhos
            item.Desenhos.Clear();
            var desenhos = await _context.Desenhos
                .Where(d => vm.SelectedDesenhoIds.Contains((int)d.DesenhoId))
                .ToListAsync();
            desenhos.ForEach(d => item.Desenhos.Add(d));

            // Perfis
            item.PerfilItemERPs.Clear();
            var perfis = await _context.Perfis
                .Where(p => vm.SelectedPerfilIds.Contains(p.Id))
                .ToListAsync();
            perfis.ForEach(p => item.PerfilItemERPs.Add(new PerfilItemERP { ItemERPId = item.Id, PerfilId = p.Id }));

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: ItensERP/ConfiguradorItemERP/5
        public async Task<IActionResult> ConfiguradorItemERP(int? id)
        {
            if (id == null) return NotFound();

            var item = await _context.ItensERP
                .Include(i => i.AgrupadorItensERP)
                .Include(i => i.ComponenteItemERPs)           // necessário para Seção 06
                .Include(i => i.Desenhos)
                .Include(i => i.Revisoes)
                .Include(i => i.PerfilItemERPs).ThenInclude(pi => pi.Revisoes)
                .Include(i => i.ItensRelacionados)            // Seção 05
                .FirstOrDefaultAsync(i => i.Id == id);

            if (item == null) return NotFound();

            var vm = new ConfiguradorItemERPViewModel
            {
                Id = item.Id,
                ERP = item.ERP,
                Descricao = item.Descricao,
                TipoItem = item.TipoItem,
                Acabamento = item.Acabamento,
                Classificacao = item.Classificacao,
                ChapaAberta = item.ChapaAberta,
                Espessura = item.Altura,
                Aco = item.Aco,
                PesoLiquidoMetro = item.PesoLiquidoMetro,
                PesoBrutoMetro = item.PesoBrutoMetro,
                QuantidadeDobras = item.QuantidadeDobras,

                // Seção 04
                SelectedAgrupadorIds = item.AgrupadorItensERP.Select(ai => ai.AgrupadorId).ToList(),

                // Seção 01
                Desenhos = item.Desenhos.Select(d => new DesenhoLinhaViewModel
                {
                    Id = (int)d.DesenhoId,
                    Nome = d.Nome,
                    Descricao = d.Descricao,
                    Revisao = d.Revisao,
                    DataCriacao = d.DataCriacao
                }).ToList(),

                // Seção 02
                Revisoes = item.Revisoes.Select(r => new RevisaoLinhaViewModel
                {
                    Id = r.Id,
                    Numero = r.Numero,
                    MotivoRevisao = r.Motivo,
                    DataRevisao = r.Data
                }).ToList(),

                // Seção 04
                PerfisSection = item.PerfilItemERPs.Select(pi => new PerfilLinhaViewModel
                {
                    Id = pi.Id,
                    PerfilId = pi.PerfilId,
                    Aco = pi.Aco,
                    Revisoes = pi.Revisoes.Select(rr => new RevisaoLinhaViewModel
                    {
                        Id = rr.Id,
                        Numero = rr.Numero,
                        MotivoRevisao = rr.Motivo,
                        DataRevisao = rr.Data
                    }).ToList()
                }).ToList(),

                // Seção 05
                ItensPintados = item.ItensRelacionados
                    .Where(r => r.Tipo == RelacionamentoTipo.Pintado)
                    .Select(r => new RelatedItemViewModel
                    {
                        Id = r.Id,
                        ItemERPId = r.RelacionadoId,
                        DesenhoId = r.DesenhoId
                    }).ToList(),

                ItensGalvanizados = item.ItensRelacionados
                    .Where(r => r.Tipo == RelacionamentoTipo.Galvanizado)
                    .Select(r => new RelatedItemViewModel
                    {
                        Id = r.Id,
                        ItemERPId = r.RelacionadoId,
                        DesenhoId = r.DesenhoId
                    }).ToList(),

                // Seção 06
                ComponentesFamily = item.ComponenteItemERPs.Select(ci => new FamilyComponenteViewModel
                {
                    Id = ci.Id,
                    ComponenteId = ci.ComponenteId
                }).ToList(),

                AgrupadoresFamily = item.AgrupadorItensERP.Select(ai => new FamilyAgrupadorViewModel
                {
                    Id = ai.Id,
                    AgrupadorId = ai.AgrupadorId
                }).ToList()
            };

            PopulateAuxLists(vm);
            vm.AllComponentes = _context.Componentes
                .Select(c => new SelectListItem(c.Nome, c.Id.ToString()))
                .ToList();

            return View(vm);
        }

        // POST: ItensERP/ConfiguradorItemERP
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfiguradorItemERP(ConfiguradorItemERPViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                PopulateAuxLists(vm);
                return View(vm);
            }

            var item = await _context.ItensERP
                .Include(i => i.AgrupadorItensERP)
                .Include(i => i.ComponenteItemERPs)
                .Include(i => i.Desenhos)
                .Include(i => i.Revisoes)
                .Include(i => i.PerfilItemERPs).ThenInclude(pi => pi.Revisoes)
                .Include(i => i.ItensRelacionados)
                .FirstOrDefaultAsync(i => i.Id == vm.Id);

            if (item == null) return NotFound();

            // campos básicos
            item.ERP = vm.ERP;
            item.Descricao = vm.Descricao;
            item.TipoItem = vm.TipoItem;
            item.Acabamento = vm.Acabamento;
            item.Classificacao = vm.Classificacao;
            item.ChapaAberta = vm.ChapaAberta;
            item.Altura = vm.Espessura;
            item.Aco = vm.Aco;
            item.PesoLiquidoMetro = vm.PesoLiquidoMetro;
            item.PesoBrutoMetro = vm.PesoBrutoMetro;
            item.QuantidadeDobras = vm.QuantidadeDobras;

            // Seção 04 (agrupadores originais, se ainda usar)
            _context.AgrupadorItemERPs.RemoveRange(item.AgrupadorItensERP);
            foreach (var agrId in vm.SelectedAgrupadorIds)
                item.AgrupadorItensERP.Add(new AgrupadorItemERP { ItemERPId = item.Id, AgrupadorId = agrId, Status = true });

            // Seção 05 (itens relacionados)
            _context.ItemERPRelacionados.RemoveRange(item.ItensRelacionados);
            foreach (var r in vm.ItensPintados)
                item.ItensRelacionados.Add(new ItemERPRelacionado { ItemERPId = item.Id, RelacionadoId = r.ItemERPId, DesenhoId = r.DesenhoId, Tipo = RelacionamentoTipo.Pintado });
            foreach (var r in vm.ItensGalvanizados)
                item.ItensRelacionados.Add(new ItemERPRelacionado { ItemERPId = item.Id, RelacionadoId = r.ItemERPId, DesenhoId = r.DesenhoId, Tipo = RelacionamentoTipo.Galvanizado });

            // Seção 06 (famílias)
            _context.ComponenteItemERPs.RemoveRange(item.ComponenteItemERPs);
            _context.AgrupadorItemERPs.RemoveRange(item.AgrupadorItensERP);
            foreach (var c in vm.ComponentesFamily)
                item.ComponenteItemERPs.Add(new ComponenteItemERP { ItemERPId = item.Id, ComponenteId = c.ComponenteId });
            foreach (var a in vm.AgrupadoresFamily)
                item.AgrupadorItensERP.Add(new AgrupadorItemERP { ItemERPId = item.Id, AgrupadorId = a.AgrupadorId, Status = true });

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // ---------------------------
        // Helpers para popular dropdowns
        // ---------------------------

        private void PopulateSelections(ItemERPCreateEditViewModel vm)
        {
            vm.AllTags = _context.Tags
                .Select(t => new SelectListItem(t.Nome, t.Id.ToString(), vm.SelectedTagIds.Contains(t.Id)))
                .ToList();

            vm.AllAgrupadores = _context.Agrupadores
                .Select(a => new SelectListItem(a.Nome, a.Id.ToString(), vm.SelectedAgrupadorIds.Contains(a.Id)))
                .ToList();

            vm.AllDesenhos = _context.Desenhos
                .Select(d => new SelectListItem(d.Nome, d.DesenhoId.ToString(), vm.SelectedDesenhoIds.Contains((int)d.DesenhoId)))
                .ToList();

            vm.AllPerfis = _context.Perfis
                .Select(p => new SelectListItem(p.ERP ?? p.Id.ToString(), p.Id.ToString(), vm.SelectedPerfilIds.Contains(p.Id)))
                .ToList();
        }

        private void PopulateAuxLists(ConfiguradorItemERPViewModel vm)
        {
            vm.AllAgrupadores = _context.Agrupadores
                .Select(a => new SelectListItem(a.Nome, a.Id.ToString(), vm.SelectedAgrupadorIds.Contains(a.Id)))
                .ToList();

            vm.AllDesenhos = _context.Desenhos
                .Select(d => new SelectListItem(d.Nome, d.DesenhoId.ToString()))
                .ToList();

            vm.AllPerfisSection = _context.Perfis
                .Select(p => new SelectListItem(p.Descricao ?? p.ERP, p.Id.ToString()))
                .ToList();

            vm.AllItensERP = _context.ItensERP
                .Select(i => new SelectListItem(i.ERP, i.Id.ToString()))
                .ToList();

            vm.AllComponentes = _context.Componentes
                .Select(c => new SelectListItem(c.Nome, c.Id.ToString()))
                .ToList();
        }
    }
}
