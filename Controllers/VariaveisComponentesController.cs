using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Gerenciador_de_Produtos.Data;
using Gerenciador_de_Produtos.Models;

namespace Gerenciador_de_Produtos.Controllers
{
    public class VariaveisComponentesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VariaveisComponentesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: VariaveisComponentes
        public async Task<IActionResult> Index()
        {
            var lista = await _context.VariaveisComponentes
                                      // inclui o Componente
                                      .Include(v => v.Componente)
                                          // opcional: já carregar os agrupadores do componente
                                          .ThenInclude(c => c.AgrupadorComponentes)
                                              .ThenInclude(ac => ac.Agrupador)
                                      .ToListAsync();
            return View(lista);
        }

        // GET: VariaveisComponentes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var vm = await _context.VariaveisComponentes
                                   .Include(v => v.Componente)
                                   .FirstOrDefaultAsync(v => v.Id == id);
            if (vm == null) return NotFound();

            return View(vm);
        }

        // GET: VariaveisComponentes/Create
        public IActionResult Create()
        {
            ViewBag.ComponenteId = new SelectList(
                _context.Componentes.OrderBy(c => c.Nome),
                "Id",
                "Nome"
            );
            return View();
        }

        // POST: VariaveisComponentes/Create
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VariavelComponente vm)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vm);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Em caso de erro, recarrega o dropdown
            ViewBag.ComponenteId = new SelectList(
                _context.Componentes.OrderBy(c => c.Nome),
                "Id",
                "Nome",
                vm.ComponenteId
            );
            return View(vm);
        }

        // GET: VariaveisComponentes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var vm = await _context.VariaveisComponentes.FindAsync(id);
            if (vm == null) return NotFound();

            ViewBag.ComponenteId = new SelectList(
                _context.Componentes.OrderBy(c => c.Nome),
                "Id",
                "Nome",
                vm.ComponenteId
            );
            return View(vm);
        }

        // POST: VariaveisComponentes/Edit/5
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, VariavelComponente vm)
        {
            if (id != vm.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vm);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.VariaveisComponentes.Any(e => e.Id == vm.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.ComponenteId = new SelectList(
                _context.Componentes.OrderBy(c => c.Nome),
                "Id",
                "Nome",
                vm.ComponenteId
            );
            return View(vm);
        }

        // GET: VariaveisComponentes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var vm = await _context.VariaveisComponentes
                                   .Include(v => v.Componente)
                                   .FirstOrDefaultAsync(v => v.Id == id);
            if (vm == null) return NotFound();

            return View(vm);
        }

        // POST: VariaveisComponentes/Delete/5
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vm = await _context.VariaveisComponentes.FindAsync(id);
            if (vm != null)
            {
                _context.VariaveisComponentes.Remove(vm);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool VariavelComponenteExists(int id)
        {
            return _context.VariaveisComponentes.Any(e => e.Id == id);
        }
    }
}
