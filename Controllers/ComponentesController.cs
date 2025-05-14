using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Gerenciador_de_Produtos.Data;
using Gerenciador_de_Produtos.Models;

namespace Gerenciador_de_Produtos.Controllers
{
    public class ComponentesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ComponentesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Componentes
        public async Task<IActionResult> Index()
        {
            var lista = await _context.Componentes
                // inclui as variáveis deste componente
                .Include(c => c.VariaveisComponentes)
                // inclui os vínculos com agrupadores e depois o próprio agrupador
                .Include(c => c.AgrupadorComponentes)
                    .ThenInclude(ac => ac.Agrupador)
                .ToListAsync();

            return View(lista);
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
                .FirstOrDefaultAsync(m => m.Id == id);

            if (componente == null)
                return NotFound();

            return View(componente);
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
