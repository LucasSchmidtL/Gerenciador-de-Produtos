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
            var itens = _context.ProdutoAgrupadores
                .Include(a => a.Agrupador)
                .Include(a => a.Produto);
            return View(await itens.ToListAsync());
        }

        // GET: AgrupadoresProduto/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var item = await _context.ProdutoAgrupadores
                .Include(a => a.Agrupador)
                .Include(a => a.Produto)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (item == null)
                return NotFound();

            return View(item);
        }

        // GET: AgrupadoresProduto/Create
        public IActionResult Create()
        {
            PopulateDropdowns();
            return View();
        }

        // POST: AgrupadoresProduto/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProdutoId,AgrupadorId,Variavel,Status")] AgrupadorProduto agrupadorProduto)
        {
            if (!ModelState.IsValid)
            {
                // coleta erros para debugger ou exibição
                var erros = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .Select(x => new { Campo = x.Key, Mensagens = x.Value.Errors.Select(e => e.ErrorMessage) })
                    .ToList();

                // opcional: passar erros para a view
                ViewData["ModelErrors"] = erros;

                PopulateDropdowns(agrupadorProduto.ProdutoId, agrupadorProduto.AgrupadorId);
                return View(agrupadorProduto);
            }

            _context.Add(agrupadorProduto);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: AgrupadoresProduto/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var agrupadorProduto = await _context.ProdutoAgrupadores.FindAsync(id);
            if (agrupadorProduto == null)
                return NotFound();

            PopulateDropdowns(agrupadorProduto.ProdutoId, agrupadorProduto.AgrupadorId);
            return View(agrupadorProduto);
        }

        // POST: AgrupadoresProduto/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProdutoId,AgrupadorId,Variavel,Status")] AgrupadorProduto agrupadorProduto)
        {
            if (id != agrupadorProduto.Id)
                return NotFound();

            if (!ModelState.IsValid)
            {
                PopulateDropdowns(agrupadorProduto.ProdutoId, agrupadorProduto.AgrupadorId);
                return View(agrupadorProduto);
            }

            try
            {
                _context.Update(agrupadorProduto);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AgrupadorProdutoExists(agrupadorProduto.Id))
                    return NotFound();
                else
                    throw;
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: AgrupadoresProduto/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var item = await _context.ProdutoAgrupadores
                .Include(a => a.Agrupador)
                .Include(a => a.Produto)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (item == null)
                return NotFound();

            return View(item);
        }

        // POST: AgrupadoresProduto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.ProdutoAgrupadores.FindAsync(id);
            if (item != null)
                _context.ProdutoAgrupadores.Remove(item);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AgrupadorProdutoExists(int id)
        {
            return _context.ProdutoAgrupadores.Any(e => e.Id == id);
        }

        private void PopulateDropdowns(int? produtoId = null, int? agrupadorId = null)
        {
            ViewData["ProdutoId"] = new SelectList(_context.Produtos, "Id", "NomeComercial", produtoId);
            ViewData["AgrupadorId"] = new SelectList(_context.Agrupadores, "Id", "Nome", agrupadorId);
        }
    }
}
