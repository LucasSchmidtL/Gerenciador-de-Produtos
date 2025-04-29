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
    public class NormasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NormasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Normas
        public async Task<IActionResult> Index()
        {
            return View(await _context.Norma.ToListAsync());
        }

        // GET: Normas/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var norma = await _context.Norma
                .FirstOrDefaultAsync(m => m.Id == id);
            if (norma == null)
            {
                return NotFound();
            }

            return View(norma);
        }

        // GET: Normas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Normas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Equacoes,Revisao")] Norma norma)
        {
            if (ModelState.IsValid)
            {
                _context.Add(norma);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(norma);
        }

        // GET: Normas/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var norma = await _context.Norma.FindAsync(id);
            if (norma == null)
            {
                return NotFound();
            }
            return View(norma);
        }

        // POST: Normas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Equacoes,Revisao")] Norma norma)
        {
            if (id != norma.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(norma);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NormaExists(norma.Id))
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
            return View(norma);
        }

        // GET: Normas/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var norma = await _context.Norma
                .FirstOrDefaultAsync(m => m.Id == id);
            if (norma == null)
            {
                return NotFound();
            }

            return View(norma);
        }

        // POST: Normas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var norma = await _context.Norma.FindAsync(id);
            if (norma != null)
            {
                _context.Norma.Remove(norma);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NormaExists(long id)
        {
            return _context.Norma.Any(e => e.Id == id);
        }
    }
}
