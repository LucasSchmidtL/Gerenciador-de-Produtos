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
    public class AgrupadoresProdutoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AgrupadoresProdutoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AgrupadoresProduto
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ProdutoAgrupadores.Include(a => a.Agrupador).Include(a => a.Produto);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: AgrupadoresProduto/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var agrupadorProduto = await _context.ProdutoAgrupadores
                .Include(a => a.Agrupador)
                .Include(a => a.Produto)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (agrupadorProduto == null)
            {
                return NotFound();
            }

            return View(agrupadorProduto);
        }

        // GET: AgrupadoresProduto/Create
        public IActionResult Create()
        {
            ViewData["AgrupadorId"] = new SelectList(_context.Agrupadores, "Id", "Id");
            ViewData["ProdutoId"] = new SelectList(_context.Produtos, "Id", "Familia");
            return View();
        }

        // POST: AgrupadoresProduto/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProdutoId,AgrupadorId,Variavel,Status")] AgrupadorProduto agrupadorProduto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(agrupadorProduto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AgrupadorId"] = new SelectList(_context.Agrupadores, "Id", "Id", agrupadorProduto.AgrupadorId);
            ViewData["ProdutoId"] = new SelectList(_context.Produtos, "Id", "Familia", agrupadorProduto.ProdutoId);
            return View(agrupadorProduto);
        }

        // GET: AgrupadoresProduto/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var agrupadorProduto = await _context.ProdutoAgrupadores.FindAsync(id);
            if (agrupadorProduto == null)
            {
                return NotFound();
            }
            ViewData["AgrupadorId"] = new SelectList(_context.Agrupadores, "Id", "Id", agrupadorProduto.AgrupadorId);
            ViewData["ProdutoId"] = new SelectList(_context.Produtos, "Id", "Familia", agrupadorProduto.ProdutoId);
            return View(agrupadorProduto);
        }

        // POST: AgrupadoresProduto/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProdutoId,AgrupadorId,Variavel,Status")] AgrupadorProduto agrupadorProduto)
        {
            if (id != agrupadorProduto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(agrupadorProduto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AgrupadorProdutoExists(agrupadorProduto.Id))
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
            ViewData["AgrupadorId"] = new SelectList(_context.Agrupadores, "Id", "Id", agrupadorProduto.AgrupadorId);
            ViewData["ProdutoId"] = new SelectList(_context.Produtos, "Id", "Familia", agrupadorProduto.ProdutoId);
            return View(agrupadorProduto);
        }

        // GET: AgrupadoresProduto/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var agrupadorProduto = await _context.ProdutoAgrupadores
                .Include(a => a.Agrupador)
                .Include(a => a.Produto)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (agrupadorProduto == null)
            {
                return NotFound();
            }

            return View(agrupadorProduto);
        }

        // POST: AgrupadoresProduto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var agrupadorProduto = await _context.ProdutoAgrupadores.FindAsync(id);
            if (agrupadorProduto != null)
            {
                _context.ProdutoAgrupadores.Remove(agrupadorProduto);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AgrupadorProdutoExists(int id)
        {
            return _context.ProdutoAgrupadores.Any(e => e.Id == id);
        }
    }
}
