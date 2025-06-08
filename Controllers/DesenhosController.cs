using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Gerenciador_de_Produtos.Data;
using Gerenciador_de_Produtos.Models;
using Gerenciador_de_Produtos.Models.ViewModels;

namespace Gerenciador_de_Produtos.Controllers
{
    public class DesenhosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DesenhosController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var desenhos = await _context.Desenhos
                .Include(d => d.DesenhoItemERPs)
                    .ThenInclude(di => di.ItemERP)
                .ToListAsync();

            return View(desenhos);
        }

        public async Task<IActionResult> Details(long? id)
        {
            if (id == null) return NotFound();

            var desenho = await _context.Desenhos
                .Include(d => d.DesenhoItemERPs)
                    .ThenInclude(di => di.ItemERP)
                .FirstOrDefaultAsync(d => d.DesenhoId == id);

            if (desenho == null) return NotFound();

            return View(desenho);
        }


        public IActionResult Create()
        {
            var vm = new DesenhoViewModel
            {
                AllItemERPs = _context.ItensERP
                    .Select(i => new SelectListItem(i.ERP, i.Id.ToString()))
                    .ToList()
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DesenhoViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.AllItemERPs = _context.ItensERP
                    .Select(i => new SelectListItem(i.ERP, i.Id.ToString()))
                    .ToList();
                return View(vm);
            }

            var desenho = new Desenho
            {
                Nome = vm.Nome,
                Descricao = vm.Descricao,
                Revisao = vm.Revisao,
                Status = vm.Status,
                Classificacao = vm.Classificacao,
                SolicitacaoAlteracaoId = vm.SolicitacaoAlteracaoId,
                DataCriacao = DateTime.Now,
                DesenhoItemERPs = vm.SelectedItemERPIds.Select(id => new DesenhoItemERP
                {
                    ItemERPId = id
                }).ToList()
            };

            _context.Desenhos.Add(desenho);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null) return NotFound();

            var desenho = await _context.Desenhos
                .Include(d => d.DesenhoItemERPs)
                .FirstOrDefaultAsync(d => d.DesenhoId == id);

            if (desenho == null) return NotFound();

            var vm = new DesenhoViewModel
            {
                DesenhoId = desenho.DesenhoId,
                Nome = desenho.Nome,
                Descricao = desenho.Descricao,
                Revisao = desenho.Revisao,
                Status = desenho.Status,
                Classificacao = desenho.Classificacao,
                SolicitacaoAlteracaoId = desenho.SolicitacaoAlteracaoId,
                SelectedItemERPIds = desenho.DesenhoItemERPs.Select(di => di.ItemERPId).ToList(),
                AllItemERPs = await _context.ItensERP
                    .Select(i => new SelectListItem(i.ERP, i.Id.ToString()))
                    .ToListAsync()
            };

            ViewBag.ItensSelecionados = await _context.ItensERP
                .Where(i => vm.SelectedItemERPIds.Contains(i.Id))
                .Select(i => new SelectListItem
                {
                    Value = i.Id.ToString(),
                    Text = $"{i.ERP} | {i.Descricao} | {i.TipoItem}"
                })
                .ToListAsync();

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, DesenhoViewModel vm)
        {
            if (id != vm.DesenhoId) return NotFound();

            if (!ModelState.IsValid)
            {
                vm.AllItemERPs = _context.ItensERP
                    .Select(i => new SelectListItem(i.ERP, i.Id.ToString()))
                    .ToList();
                return View(vm);
            }

            var desenho = await _context.Desenhos
                .Include(d => d.DesenhoItemERPs)
                .FirstOrDefaultAsync(d => d.DesenhoId == id);

            if (desenho == null) return NotFound();

            desenho.Nome = vm.Nome;
            desenho.Descricao = vm.Descricao;
            desenho.Revisao = vm.Revisao;
            desenho.Status = vm.Status;
            desenho.Classificacao = vm.Classificacao;
            desenho.SolicitacaoAlteracaoId = vm.SolicitacaoAlteracaoId;

            // Atualiza os vínculos
            desenho.DesenhoItemERPs.Clear();
            foreach (var idErp in vm.SelectedItemERPIds)
            {
                desenho.DesenhoItemERPs.Add(new DesenhoItemERP
                {
                    DesenhoId = desenho.DesenhoId,
                    ItemERPId = idErp
                });
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null) return NotFound();

            var desenho = await _context.Desenhos
                .Include(d => d.DesenhoItemERPs)
                .ThenInclude(di => di.ItemERP)
                .FirstOrDefaultAsync(d => d.DesenhoId == id);

            if (desenho == null) return NotFound();

            return View(desenho);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var desenho = await _context.Desenhos
                .Include(d => d.DesenhoItemERPs)
                .FirstOrDefaultAsync(d => d.DesenhoId == id);

            if (desenho != null)
            {
                _context.Desenhos.Remove(desenho);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
