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
    public class AgrupadoresController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AgrupadoresController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Agrupadores
        public async Task<IActionResult> Index()
        {
            return View(await _context.Agrupadores.ToListAsync());
        }

        // GET: Agrupadores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var agrupador = await _context.Agrupadores
                .FirstOrDefaultAsync(m => m.Id == id);
            if (agrupador == null)
            {
                return NotFound();
            }

            return View(agrupador);
        }

        // GET: Agrupadores/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Agrupadores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Grupo,DesenvolvimentoId,ItemERPId,AgrupadorPaiId,Nivel")] Agrupador agrupador)
        {
            if (ModelState.IsValid)
            {
                _context.Add(agrupador);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(agrupador);
        }

        // GET: Agrupadores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var agrupador = await _context.Agrupadores.FindAsync(id);
            if (agrupador == null)
            {
                return NotFound();
            }
            return View(agrupador);
        }

        // POST: Agrupadores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Grupo,DesenvolvimentoId,ItemERPId,AgrupadorPaiId,Nivel")] Agrupador agrupador)
        {
            if (id != agrupador.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(agrupador);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AgrupadorExists(agrupador.Id))
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
            return View(agrupador);
        }

        // GET: Agrupadores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var agrupador = await _context.Agrupadores
                .FirstOrDefaultAsync(m => m.Id == id);
            if (agrupador == null)
            {
                return NotFound();
            }

            return View(agrupador);
        }

        // POST: Agrupadores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var agrupador = await _context.Agrupadores.FindAsync(id);
            if (agrupador != null)
            {
                _context.Agrupadores.Remove(agrupador);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AgrupadorExists(int id)
        {
            return _context.Agrupadores.Any(e => e.Id == id);
        }
    }
}
