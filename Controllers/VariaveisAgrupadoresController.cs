using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Gerenciador_de_Produtos.Data;
using Gerenciador_de_Produtos.Models;

namespace Gerenciador_de_Produtos.Controllers
{
    public class VariaveisAgrupadoresController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VariaveisAgrupadoresController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: VariaveisAgrupadores
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.VariaveisAgrupadores
                                             .Include(v => v.Agrupador);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: VariaveisAgrupadores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var variavelAgrupador = await _context.VariaveisAgrupadores
                .Include(v => v.Agrupador)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (variavelAgrupador == null) return NotFound();

            return View(variavelAgrupador);
        }

        // GET: VariaveisAgrupadores/Create
        // GET: VariaveisAgrupadores/Create
        public IActionResult Create()
        {
            ViewData["AgrupadorId"] = new SelectList(
                _context.Agrupadores.OrderBy(a => a.Nome),
                "Id",
                "Nome"
            );
            // retorna um model vazio para não dar NullReference na View
            return View(new VariavelAgrupador());
        }


        // POST: VariaveisAgrupadores/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Descricao,Tipo,AgrupadorId,Status,Valor")] VariavelAgrupador variavelAgrupador)
        {
            if (ModelState.IsValid)
            {
                _context.Add(variavelAgrupador);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["AgrupadorId"] = new SelectList(
                _context.Agrupadores.OrderBy(a => a.Nome),
                "Id",
                "Nome",
                variavelAgrupador.AgrupadorId
            );
            return View(variavelAgrupador);
        }

        // GET: VariaveisAgrupadores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var variavelAgrupador = await _context.VariaveisAgrupadores.FindAsync(id);
            if (variavelAgrupador == null) return NotFound();

            ViewData["AgrupadorId"] = new SelectList(
                _context.Agrupadores.OrderBy(a => a.Nome),
                "Id",
                "Nome",
                variavelAgrupador.AgrupadorId
            );
            return View(variavelAgrupador);
        }

        // POST: VariaveisAgrupadores/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Descricao,Tipo,AgrupadorId,Status,Valor")] VariavelAgrupador variavelAgrupador)
        {
            if (id != variavelAgrupador.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(variavelAgrupador);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VariavelAgrupadorExists(variavelAgrupador.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["AgrupadorId"] = new SelectList(
                _context.Agrupadores.OrderBy(a => a.Nome),
                "Id",
                "Nome",
                variavelAgrupador.AgrupadorId
            );
            return View(variavelAgrupador);
        }

        // GET: VariaveisAgrupadores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var variavelAgrupador = await _context.VariaveisAgrupadores
                .Include(v => v.Agrupador)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (variavelAgrupador == null) return NotFound();

            return View(variavelAgrupador);
        }

        // POST: VariaveisAgrupadores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var variavelAgrupador = await _context.VariaveisAgrupadores.FindAsync(id);
            if (variavelAgrupador != null)
            {
                _context.VariaveisAgrupadores.Remove(variavelAgrupador);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool VariavelAgrupadorExists(int id)
        {
            return _context.VariaveisAgrupadores.Any(e => e.Id == id);
        }
    }
}
