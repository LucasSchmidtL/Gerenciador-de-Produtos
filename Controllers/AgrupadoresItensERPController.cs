using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Gerenciador_de_Produtos.Data;
using Gerenciador_de_Produtos.Models;

namespace Gerenciador_de_Produtos.Controllers
{
    public class AgrupadoresItensERPController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AgrupadoresItensERPController(ApplicationDbContext context)
        {
            _context = context;
        }

        // INDEX
        public async Task<IActionResult> Index()
        {
            var dados = await _context.AgrupadorItemERPs
                .Include(a => a.Agrupador)
                .Include(a => a.ItemERP)
                .ToListAsync();

            return View(dados);
        }

        // DETAILS
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var agrupadorItemERP = await _context.AgrupadorItemERPs
                .Include(a => a.Agrupador)
                .Include(a => a.ItemERP)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (agrupadorItemERP == null) return NotFound();

            return View(agrupadorItemERP);
        }

        // CREATE - GET
        public IActionResult Create()
        {
            ViewData["AgrupadorId"] = new SelectList(_context.Agrupadores.OrderBy(a => a.Nome), "Id", "Nome");
            ViewData["ItemERPId"] = new SelectList(_context.ItensERP.OrderBy(i => i.ERP), "Id", "ERP");
            return View();
        }

        // CREATE - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AgrupadorItemERP agrupadorItemERP)
        {
            if (ModelState.IsValid)
            {
                _context.Add(agrupadorItemERP);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["AgrupadorId"] = new SelectList(_context.Agrupadores.OrderBy(a => a.Nome), "Id", "Nome", agrupadorItemERP.AgrupadorId);
            ViewData["ItemERPId"] = new SelectList(_context.ItensERP.OrderBy(i => i.ERP), "Id", "ERP", agrupadorItemERP.ItemERPId);
            return View(agrupadorItemERP);
        }

        // EDIT - GET
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var agrupadorItemERP = await _context.AgrupadorItemERPs.FindAsync(id);
            if (agrupadorItemERP == null) return NotFound();

            ViewData["AgrupadorId"] = new SelectList(_context.Agrupadores.OrderBy(a => a.Nome), "Id", "Nome", agrupadorItemERP.AgrupadorId);
            ViewData["ItemERPId"] = new SelectList(_context.ItensERP.OrderBy(i => i.ERP), "Id", "ERP", agrupadorItemERP.ItemERPId);
            return View(agrupadorItemERP);
        }

        // EDIT - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AgrupadorItemERP agrupadorItemERP)
        {
            if (id != agrupadorItemERP.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(agrupadorItemERP);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AgrupadorItemERPExists(agrupadorItemERP.Id))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["AgrupadorId"] = new SelectList(_context.Agrupadores.OrderBy(a => a.Nome), "Id", "Nome", agrupadorItemERP.AgrupadorId);
            ViewData["ItemERPId"] = new SelectList(_context.ItensERP.OrderBy(i => i.ERP), "Id", "ERP", agrupadorItemERP.ItemERPId);
            return View(agrupadorItemERP);
        }

        // DELETE - GET
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var agrupadorItemERP = await _context.AgrupadorItemERPs
                .Include(a => a.Agrupador)
                .Include(a => a.ItemERP)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (agrupadorItemERP == null) return NotFound();

            return View(agrupadorItemERP);
        }

        // DELETE - POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var agrupadorItemERP = await _context.AgrupadorItemERPs.FindAsync(id);
            if (agrupadorItemERP != null)
            {
                _context.AgrupadorItemERPs.Remove(agrupadorItemERP);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // Exists
        private bool AgrupadorItemERPExists(int id)
        {
            return _context.AgrupadorItemERPs.Any(e => e.Id == id);
        }
    }
}
