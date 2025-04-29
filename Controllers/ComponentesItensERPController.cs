using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Gerenciador_de_Produtos.Data;
using Gerenciador_de_Produtos.Models;

namespace Gerenciador_de_Produtos.Controllers
{
    public class ComponentesItensERPController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ComponentesItensERPController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ComponentesItensERP
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ComponenteItemERPs.Include(c => c.Componente).Include(c => c.ItemERP);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ComponentesItensERP/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var componenteItemERP = await _context.ComponenteItemERPs
                .Include(c => c.Componente)
                .Include(c => c.ItemERP)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (componenteItemERP == null)
            {
                return NotFound();
            }

            return View(componenteItemERP);
        }

        // GET: ComponentesItensERP/Create
        public IActionResult Create()
        {
            ViewData["ComponenteId"] = new SelectList(_context.Componentes, "Id", "Id");
            ViewData["ItemERPId"] = new SelectList(_context.ItensERP, "Id", "Id");
            return View();
        }

        // POST: ComponentesItensERP/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ComponenteId,ItemERPId,Comprimento,Profundidade,Altura,Quantidade,Status")] ComponenteItemERP componenteItemERP)
        {
            if (ModelState.IsValid)
            {
                _context.Add(componenteItemERP);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ComponenteId"] = new SelectList(_context.Componentes, "Id", "Id", componenteItemERP.ComponenteId);
            ViewData["ItemERPId"] = new SelectList(_context.ItensERP, "Id", "Id", componenteItemERP.ItemERPId);
            return View(componenteItemERP);
        }

        // GET: ComponentesItensERP/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var componenteItemERP = await _context.ComponenteItemERPs.FindAsync(id);
            if (componenteItemERP == null)
            {
                return NotFound();
            }
            ViewData["ComponenteId"] = new SelectList(_context.Componentes, "Id", "Id", componenteItemERP.ComponenteId);
            ViewData["ItemERPId"] = new SelectList(_context.ItensERP, "Id", "Id", componenteItemERP.ItemERPId);
            return View(componenteItemERP);
        }

        // POST: ComponentesItensERP/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ComponenteId,ItemERPId,Comprimento,Profundidade,Altura,Quantidade,Status")] ComponenteItemERP componenteItemERP)
        {
            if (id != componenteItemERP.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(componenteItemERP);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ComponenteItemERPExists(componenteItemERP.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ComponenteId"] = new SelectList(_context.Componentes, "Id", "Id", componenteItemERP.ComponenteId);
            ViewData["ItemERPId"] = new SelectList(_context.ItensERP, "Id", "Id", componenteItemERP.ItemERPId);
            return View(componenteItemERP);
        }

        // GET: ComponentesItensERP/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var componenteItemERP = await _context.ComponenteItemERPs
                .Include(c => c.Componente)
                .Include(c => c.ItemERP)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (componenteItemERP == null)
            {
                return NotFound();
            }

            return View(componenteItemERP);
        }

        // POST: ComponentesItensERP/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var componenteItemERP = await _context.ComponenteItemERPs.FindAsync(id);
            if (componenteItemERP != null)
            {
                _context.ComponenteItemERPs.Remove(componenteItemERP);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ComponenteItemERPExists(int id)
        {
            return _context.ComponenteItemERPs.Any(e => e.Id == id);
        }
    }
}
