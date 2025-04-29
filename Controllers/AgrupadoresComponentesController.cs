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
            var applicationDbContext = _context.AgrupadorComponentes.Include(a => a.Agrupador).Include(a => a.Componente);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: AgrupadoresComponentes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var agrupadorComponente = await _context.AgrupadorComponentes
                .Include(a => a.Agrupador)
                .Include(a => a.Componente)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (agrupadorComponente == null)
            {
                return NotFound();
            }

            return View(agrupadorComponente);
        }

        // GET: AgrupadoresComponentes/Create
        public IActionResult Create()
        {
            ViewData["AgrupadorId"] = new SelectList(_context.Agrupadores, "Id", "Id");
            ViewData["ComponenteId"] = new SelectList(_context.Componentes, "Id", "Id");
            return View();
        }

        // POST: AgrupadoresComponentes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AgrupadorId,ComponenteId,Quantidade,Comprimento,Profundidade,Altura,Status")] AgrupadorComponente agrupadorComponente)
        {
            if (ModelState.IsValid)
            {
                _context.Add(agrupadorComponente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AgrupadorId"] = new SelectList(_context.Agrupadores, "Id", "Id", agrupadorComponente.AgrupadorId);
            ViewData["ComponenteId"] = new SelectList(_context.Componentes, "Id", "Id", agrupadorComponente.ComponenteId);
            return View(agrupadorComponente);
        }

        // GET: AgrupadoresComponentes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var agrupadorComponente = await _context.AgrupadorComponentes.FindAsync(id);
            if (agrupadorComponente == null)
            {
                return NotFound();
            }
            ViewData["AgrupadorId"] = new SelectList(_context.Agrupadores, "Id", "Id", agrupadorComponente.AgrupadorId);
            ViewData["ComponenteId"] = new SelectList(_context.Componentes, "Id", "Id", agrupadorComponente.ComponenteId);
            return View(agrupadorComponente);
        }

        // POST: AgrupadoresComponentes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AgrupadorId,ComponenteId,Quantidade,Comprimento,Profundidade,Altura,Status")] AgrupadorComponente agrupadorComponente)
        {
            if (id != agrupadorComponente.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(agrupadorComponente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AgrupadorComponenteExists(agrupadorComponente.Id))
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
            ViewData["AgrupadorId"] = new SelectList(_context.Agrupadores, "Id", "Id", agrupadorComponente.AgrupadorId);
            ViewData["ComponenteId"] = new SelectList(_context.Componentes, "Id", "Id", agrupadorComponente.ComponenteId);
            return View(agrupadorComponente);
        }

        // GET: AgrupadoresComponentes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var agrupadorComponente = await _context.AgrupadorComponentes
                .Include(a => a.Agrupador)
                .Include(a => a.Componente)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (agrupadorComponente == null)
            {
                return NotFound();
            }

            return View(agrupadorComponente);
        }

        // POST: AgrupadoresComponentes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var agrupadorComponente = await _context.AgrupadorComponentes.FindAsync(id);
            if (agrupadorComponente != null)
            {
                _context.AgrupadorComponentes.Remove(agrupadorComponente);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AgrupadorComponenteExists(int id)
        {
            return _context.AgrupadorComponentes.Any(e => e.Id == id);
        }
    }
}
