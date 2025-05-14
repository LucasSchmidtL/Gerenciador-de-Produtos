using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Gerenciador_de_Produtos.Data;
using Gerenciador_de_Produtos.Models;

namespace Gerenciador_de_Produtos.Controllers
{
    public class AgrupadoresComponentesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AgrupadoresComponentesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AgrupadoresComponentes
        public async Task<IActionResult> Index()
        {
            var lista = await _context.AgrupadorComponentes
                .Include(x => x.Agrupador)
                .Include(x => x.Componente)
                .ToListAsync();
            return View(lista);
        }

        // GET: AgrupadoresComponentes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var item = await _context.AgrupadorComponentes
                .Include(x => x.Agrupador)
                .Include(x => x.Componente)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (item == null) return NotFound();

            return View(item);
        }

        // GET: AgrupadoresComponentes/Create
        public IActionResult Create()
        {
            ViewData["AgrupadorId"] = new SelectList(_context.Agrupadores.OrderBy(a => a.Nome), "Id", "Nome");
            ViewData["ComponenteId"] = new SelectList(_context.Componentes.OrderBy(c => c.Nome), "Id", "Nome");
            return View();
        }

        // POST: AgrupadoresComponentes/Create
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AgrupadorComponente vm)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vm);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AgrupadorId"] = new SelectList(_context.Agrupadores.OrderBy(a => a.Nome), "Id", "Nome", vm.AgrupadorId);
            ViewData["ComponenteId"] = new SelectList(_context.Componentes.OrderBy(c => c.Nome), "Id", "Nome", vm.ComponenteId);
            return View(vm);
        }

        // GET: AgrupadoresComponentes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var vm = await _context.AgrupadorComponentes.FindAsync(id);
            if (vm == null) return NotFound();

            ViewData["AgrupadorId"] = new SelectList(_context.Agrupadores.OrderBy(a => a.Nome), "Id", "Nome", vm.AgrupadorId);
            ViewData["ComponenteId"] = new SelectList(_context.Componentes.OrderBy(c => c.Nome), "Id", "Nome", vm.ComponenteId);
            return View(vm);
        }

        // POST: AgrupadoresComponentes/Edit/5
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AgrupadorComponente vm)
        {
            if (id != vm.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vm);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException) when (!_context.AgrupadorComponentes.Any(e => e.Id == vm.Id))
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["AgrupadorId"] = new SelectList(_context.Agrupadores.OrderBy(a => a.Nome), "Id", "Nome", vm.AgrupadorId);
            ViewData["ComponenteId"] = new SelectList(_context.Componentes.OrderBy(c => c.Nome), "Id", "Nome", vm.ComponenteId);
            return View(vm);
        }

        // GET: AgrupadoresComponentes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var item = await _context.AgrupadorComponentes
                .Include(x => x.Agrupador)
                .Include(x => x.Componente)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (item == null) return NotFound();
            return View(item);
        }

        // POST: AgrupadoresComponentes/Delete/5
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.AgrupadorComponentes.FindAsync(id);
            if (item != null)
            {
                _context.Remove(item);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
