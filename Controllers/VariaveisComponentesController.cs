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
            var applicationDbContext = _context.VariaveisComponentes.Include(v => v.Componente);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: VariaveisComponentes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var variavelComponente = await _context.VariaveisComponentes
                .Include(v => v.Componente)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (variavelComponente == null)
            {
                return NotFound();
            }

            return View(variavelComponente);
        }

        // GET: VariaveisComponentes/Create
        public IActionResult Create()
        {
            ViewData["ComponenteId"] = new SelectList(_context.Componentes, "Id", "Id");
            return View();
        }

        // POST: VariaveisComponentes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Descricao,Tipo,ComponenteId,Status,Valor")] VariavelComponente variavelComponente)
        {
            if (ModelState.IsValid)
            {
                _context.Add(variavelComponente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ComponenteId"] = new SelectList(_context.Componentes, "Id", "Id", variavelComponente.ComponenteId);
            return View(variavelComponente);
        }

        // GET: VariaveisComponentes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var variavelComponente = await _context.VariaveisComponentes.FindAsync(id);
            if (variavelComponente == null)
            {
                return NotFound();
            }
            ViewData["ComponenteId"] = new SelectList(_context.Componentes, "Id", "Id", variavelComponente.ComponenteId);
            return View(variavelComponente);
        }

        // POST: VariaveisComponentes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Descricao,Tipo,ComponenteId,Status,Valor")] VariavelComponente variavelComponente)
        {
            if (id != variavelComponente.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(variavelComponente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VariavelComponenteExists(variavelComponente.Id))
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
            ViewData["ComponenteId"] = new SelectList(_context.Componentes, "Id", "Id", variavelComponente.ComponenteId);
            return View(variavelComponente);
        }

        // GET: VariaveisComponentes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var variavelComponente = await _context.VariaveisComponentes
                .Include(v => v.Componente)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (variavelComponente == null)
            {
                return NotFound();
            }

            return View(variavelComponente);
        }

        // POST: VariaveisComponentes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var variavelComponente = await _context.VariaveisComponentes.FindAsync(id);
            if (variavelComponente != null)
            {
                _context.VariaveisComponentes.Remove(variavelComponente);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VariavelComponenteExists(int id)
        {
            return _context.VariaveisComponentes.Any(e => e.Id == id);
        }
    }
}
