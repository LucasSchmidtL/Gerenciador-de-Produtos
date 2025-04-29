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
    public class AgrupadoresItensERPController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AgrupadoresItensERPController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AgrupadoresItensERP
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.AgrupadorItemERPs.Include(a => a.Agrupador).Include(a => a.ItemERP);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: AgrupadoresItensERP/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var agrupadorItemERP = await _context.AgrupadorItemERPs
                .Include(a => a.Agrupador)
                .Include(a => a.ItemERP)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (agrupadorItemERP == null)
            {
                return NotFound();
            }

            return View(agrupadorItemERP);
        }

        // GET: AgrupadoresItensERP/Create
        public IActionResult Create()
        {
            ViewData["AgrupadorId"] = new SelectList(_context.Agrupadores, "Id", "Id");
            ViewData["ItemERPId"] = new SelectList(_context.ItensERP, "Id", "Id");
            return View();
        }

        // POST: AgrupadoresItensERP/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AgrupadorId,ItemERPId,Comprimento,Altura,Profundidade,Quantidade,Status")] AgrupadorItemERP agrupadorItemERP)
        {
            if (ModelState.IsValid)
            {
                _context.Add(agrupadorItemERP);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AgrupadorId"] = new SelectList(_context.Agrupadores, "Id", "Id", agrupadorItemERP.AgrupadorId);
            ViewData["ItemERPId"] = new SelectList(_context.ItensERP, "Id", "Id", agrupadorItemERP.ItemERPId);
            return View(agrupadorItemERP);
        }

        // GET: AgrupadoresItensERP/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var agrupadorItemERP = await _context.AgrupadorItemERPs.FindAsync(id);
            if (agrupadorItemERP == null)
            {
                return NotFound();
            }
            ViewData["AgrupadorId"] = new SelectList(_context.Agrupadores, "Id", "Id", agrupadorItemERP.AgrupadorId);
            ViewData["ItemERPId"] = new SelectList(_context.ItensERP, "Id", "Id", agrupadorItemERP.ItemERPId);
            return View(agrupadorItemERP);
        }

        // POST: AgrupadoresItensERP/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AgrupadorId,ItemERPId,Comprimento,Altura,Profundidade,Quantidade,Status")] AgrupadorItemERP agrupadorItemERP)
        {
            if (id != agrupadorItemERP.Id)
            {
                return NotFound();
            }

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
            ViewData["AgrupadorId"] = new SelectList(_context.Agrupadores, "Id", "Id", agrupadorItemERP.AgrupadorId);
            ViewData["ItemERPId"] = new SelectList(_context.ItensERP, "Id", "Id", agrupadorItemERP.ItemERPId);
            return View(agrupadorItemERP);
        }

        // GET: AgrupadoresItensERP/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var agrupadorItemERP = await _context.AgrupadorItemERPs
                .Include(a => a.Agrupador)
                .Include(a => a.ItemERP)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (agrupadorItemERP == null)
            {
                return NotFound();
            }

            return View(agrupadorItemERP);
        }

        // POST: AgrupadoresItensERP/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var agrupadorItemERP = await _context.AgrupadorItemERPs.FindAsync(id);
            if (agrupadorItemERP != null)
            {
                _context.AgrupadorItemERPs.Remove(agrupadorItemERP);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AgrupadorItemERPExists(int id)
        {
            return _context.AgrupadorItemERPs.Any(e => e.Id == id);
        }
    }
}
