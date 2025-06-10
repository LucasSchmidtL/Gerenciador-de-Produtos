using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Gerenciador_de_Produtos.Data;
using Gerenciador_de_Produtos.Models;
using Gerenciador_de_Produtos.Models.ViewModels;
using Gerenciador_de_Produtos.Models.Enums;

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

        public async Task<IActionResult> Create()
        {
            var vm = new DesenhoViewModel
            {
                AllItemERPs = await _context.ItensERP.Select(i => new SelectListItem(i.ERP, i.Id.ToString())).ToListAsync()
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DesenhoViewModel viewModel)
        {
            if (await _context.Desenhos.AnyAsync(d => d.Nome == viewModel.Nome))
            {
                ModelState.AddModelError("Nome", "Já existe um desenho com este nome.");
            }

            if (!ModelState.IsValid)
            {
                viewModel.AllItemERPs = await _context.ItensERP
                    .Select(i => new SelectListItem(i.ERP, i.Id.ToString()))
                    .ToListAsync();

                return View(viewModel);
            }

            var novoDesenho = new Desenho
            {
                Nome = viewModel.Nome,
                Descricao = viewModel.Descricao,
                DataCriacao = viewModel.DataCriacao,
                Revisao = viewModel.Revisao,
                Status = viewModel.Status,
                Classificacao = viewModel.Classificacao,
                SolicitacaoAlteracaoId = viewModel.SolicitacaoAlteracaoId
            };

            _context.Desenhos.Add(novoDesenho);
            await _context.SaveChangesAsync();

            if (viewModel.SelectedItemERPIds != null)
            {
                var vinculos = viewModel.SelectedItemERPIds.Select(itemId => new DesenhoItemERP
                {
                    DesenhoId = novoDesenho.DesenhoId,
                    ItemERPId = itemId
                });

                _context.DesenhoItemERPs.AddRange(vinculos);
                await _context.SaveChangesAsync();
            }

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
                DataCriacao = desenho.DataCriacao,
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

            ViewBag.AllTags = await _context.Tags
                .Select(t => new SelectListItem
                {
                    Value = t.Nome,
                    Text = t.Nome
                }).ToListAsync();


            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, DesenhoViewModel vm)
        {
            if (id != vm.DesenhoId) return NotFound();

            bool nomeDuplicado = await _context.Desenhos
                .AnyAsync(d => d.Nome == vm.Nome && d.DesenhoId != vm.DesenhoId);

            if (nomeDuplicado)
            {
                ModelState.AddModelError("Nome", "Já existe outro desenho com este nome.");
            }

            if (!ModelState.IsValid)
            {
                vm.AllItemERPs = await _context.ItensERP
                    .Select(i => new SelectListItem(i.ERP, i.Id.ToString()))
                    .ToListAsync();

                ViewBag.ItensSelecionados = await _context.ItensERP
                    .Where(i => vm.SelectedItemERPIds.Contains(i.Id))
                    .Select(i => new SelectListItem
                    {
                        Value = i.Id.ToString(),
                        Text = $"{i.ERP} | {i.Descricao} | {i.TipoItem}"
                    })
                    .ToListAsync();

                ViewBag.AllTags = await _context.Tags
                    .Select(t => new SelectListItem
                    {
                        Value = t.Nome,
                        Text = t.Nome
                    }).ToListAsync();

                return View(vm);
            }


            var desenho = await _context.Desenhos
                .Include(d => d.DesenhoItemERPs)
                .FirstOrDefaultAsync(d => d.DesenhoId == id);

            if (desenho == null) return NotFound();

            desenho.Nome = vm.Nome;
            desenho.Descricao = vm.Descricao;
            desenho.DataCriacao = vm.DataCriacao;
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

            var vm = new DesenhoViewModel
            {
                DesenhoId = desenho.DesenhoId,
                Nome = desenho.Nome,
                Descricao = desenho.Descricao,
                DataCriacao = desenho.DataCriacao,
                Revisao = desenho.Revisao,
                Status = desenho.Status,
                Classificacao = desenho.Classificacao,
                SolicitacaoAlteracaoId = desenho.SolicitacaoAlteracaoId,
                SelectedItemERPIds = desenho.DesenhoItemERPs.Select(di => di.ItemERPId).ToList()
            };

            ViewBag.ItensSelecionados = desenho.DesenhoItemERPs
                .Select(di => new SelectListItem
                {
                    Value = di.ItemERPId.ToString(),
                    Text = $"{di.ItemERP.ERP}|{di.ItemERP.Descricao}|{di.ItemERP.TipoItem}"
                }).ToList();

            return View(vm);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var desenho = await _context.Desenhos
                .Include(d => d.DesenhoItemERPs)
                .FirstOrDefaultAsync(d => d.DesenhoId == id);

            if (desenho == null)
                return NotFound();

            // Remove os relacionamentos em ItemERPRelacionados que apontam para esse Desenho
            var relacionados = _context.ItemERPRelacionados
                .Where(r => r.DesenhoId == id);

            _context.ItemERPRelacionados.RemoveRange(relacionados);

            // Remove os vínculos com ItemERP
            _context.DesenhoItemERPs.RemoveRange(desenho.DesenhoItemERPs);

            // Remove o desenho em si
            _context.Desenhos.Remove(desenho);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> BuscarDesenhos(string term, string status = null, string classificacao = null, string revisao = null){
            var query = _context.Desenhos
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(term))
                query = query.Where(d => d.Nome.Contains(term));

            if (!string.IsNullOrEmpty(status))
                if (Enum.TryParse<StatusDesenho>(status, out var statusEnum))
                    query = query.Where(d => d.Status == statusEnum);
            ;

            if (!string.IsNullOrEmpty(classificacao))
                query = query.Where(d => d.Classificacao == classificacao);

            if (long.TryParse(revisao, out var revisaoLong))
                query = query.Where(d => d.Revisao == revisaoLong);

            var resultados = await query
                .Select(d => new
                {
                    id = d.DesenhoId,
                    text = $"{d.Nome} | Rev. {d.Revisao} | {d.Descricao}"
                })
                .Take(50)
                .ToListAsync();

            return Json(resultados);
        }



    }
}
