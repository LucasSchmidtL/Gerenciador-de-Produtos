using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Gerenciador_de_Produtos.Data;
using Gerenciador_de_Produtos.Models;
using Gerenciador_de_Produtos.Models.ViewModels;

namespace Gerenciador_de_Produtos.Controllers
{
    public class ComponentesController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ComponentesController(ApplicationDbContext context) => _context = context;

        // GET: Componentes
        public async Task<IActionResult> Index()
        {
            var lista = await _context.Componentes
                .Include(c => c.VariaveisComponentes)
                .Include(c => c.AgrupadorComponentes)
                    .ThenInclude(ac => ac.Agrupador)
                .Include(c => c.ComponenteItensERP)
                    .ThenInclude(ci => ci.ItemERP)
                .ToListAsync();

            return View(lista);
        }

        // GET: Componentes/Configurador/5
        public IActionResult Configurador(int id)
        {
            var componente = _context.Componentes
                .Include(c => c.VariaveisComponentes)
                .Include(c => c.ComponenteItensERP)
                    .ThenInclude(ci => ci.ItemERP)
                        .ThenInclude(i => i.Tags)
                .Include(c => c.ComponenteItensERP)
                    .ThenInclude(ci => ci.ItemERP)
                        .ThenInclude(i => i.Perfis)
                .FirstOrDefault(c => c.Id == id);
            if (componente == null) return NotFound();

            var allItemErps = _context.ItensERP
                .Select(i => new SelectListItem { Text = i.ERP, Value = i.Id.ToString() })
                .ToList();

            var vm = new ConfiguradorComponenteViewModel
            {
                Componente = componente,
                Variables = componente.VariaveisComponentes
                    .Select(v => new VariavelComponente
                    {
                        Id = v.Id,
                        ComponenteId = v.ComponenteId,
                        Nome = v.Nome,
                        Valor = v.Valor,
                        Tipo = v.Tipo
                    })
                    .ToList(),
                ComponentItems = componente.ComponenteItensERP.ToList(),
                AllItemERPs = allItemErps
            };
            return View(vm);
        }

        // POST: Componentes/Configurador
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Configurador(ConfiguradorComponenteViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.AllItemERPs = _context.ItensERP
                    .Select(i => new SelectListItem { Text = i.ERP, Value = i.Id.ToString() })
                    .ToList();
                return View(vm);
            }

            var componente = await _context.Componentes
                .Include(c => c.VariaveisComponentes)
                .Include(c => c.ComponenteItensERP)
                .FirstOrDefaultAsync(c => c.Id == vm.Componente.Id);
            if (componente == null) return NotFound();

            // 1) Variáveis
            var toRemoveVars = componente.VariaveisComponentes
                .Where(v => !vm.Variables.Any(x => x.Id == v.Id))
                .ToList();
            _context.RemoveRange(toRemoveVars);

            foreach (var vModel in vm.Variables)
            {
                var existing = componente.VariaveisComponentes
                    .FirstOrDefault(v => v.Id == vModel.Id);
                if (existing != null)
                {
                    existing.Nome = vModel.Nome;
                    existing.Valor = vModel.Valor;
                    existing.Tipo = vModel.Tipo;
                }
                else
                {
                    componente.VariaveisComponentes.Add(new VariavelComponente
                    {
                        ComponenteId = componente.Id,
                        Nome = vModel.Nome,
                        Valor = vModel.Valor,
                        Tipo = vModel.Tipo
                    });
                }
            }

            // 2) Itens ERP
            var toRemoveItems = componente.ComponenteItensERP
                .Where(ci => !vm.ComponentItems.Any(x => x.Id == ci.Id))
                .ToList();
            _context.RemoveRange(toRemoveItems);

            foreach (var itemModel in vm.ComponentItems)
            {
                var existing = componente.ComponenteItensERP
                    .FirstOrDefault(ci => ci.Id == itemModel.Id);
                if (existing != null)
                {
                    existing.ItemERPId = itemModel.ItemERPId;
                    existing.Comprimento = itemModel.Comprimento;
                    existing.Altura = itemModel.Altura;
                    existing.Profundidade = itemModel.Profundidade;
                    existing.Quantidade = itemModel.Quantidade;
                    existing.Status = itemModel.Status;
                }
                else
                {
                    componente.ComponenteItensERP.Add(new ComponenteItemERP
                    {
                        ComponenteId = componente.Id,
                        ItemERPId = itemModel.ItemERPId,
                        Comprimento = itemModel.Comprimento,
                        Altura = itemModel.Altura,
                        Profundidade = itemModel.Profundidade,
                        Quantidade = itemModel.Quantidade,
                        Status = itemModel.Status
                    });
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



        // GET: Componentes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Componentes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nome,Descricao,Nivel")] Componente componente)
        {
            if (ModelState.IsValid)
            {
                _context.Add(componente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(componente);
        }

        // GET: Componentes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var componente = await _context.Componentes.FindAsync(id);
            if (componente == null)
                return NotFound();

            return View(componente);
        }

        // POST: Componentes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Descricao,Nivel")] Componente componente)
        {
            if (id != componente.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(componente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ComponenteExists(componente.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(componente);
        }

        // GET: Componentes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var componente = await _context.Componentes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (componente == null)
                return NotFound();

            return View(componente);
        }

        // GET: Componentes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var componente = await _context.Componentes
                .Include(c => c.VariaveisComponentes)
                .Include(c => c.AgrupadorComponentes)
                    .ThenInclude(ac => ac.Agrupador)
                .Include(c => c.ComponenteItensERP)
                    .ThenInclude(ci => ci.ItemERP)
                        .ThenInclude(i => i.Tags)
                .Include(c => c.ComponenteItensERP)
                    .ThenInclude(ci => ci.ItemERP)
                        .ThenInclude(i => i.Perfis)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (componente == null)
                return NotFound();

            return View(componente);
        }


        // POST: Componentes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var componente = await _context.Componentes.FindAsync(id);
            if (componente != null)
            {
                _context.Componentes.Remove(componente);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ComponenteExists(int id)
        {
            return _context.Componentes.Any(e => e.Id == id);
        }
    }
}
