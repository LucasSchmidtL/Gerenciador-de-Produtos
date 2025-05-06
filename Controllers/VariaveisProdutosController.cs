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
    public class VariaveisProdutosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VariaveisProdutosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: VariaveisProdutos
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.VariaveisProdutos
                                               .Include(v => v.Produto);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: VariaveisProdutos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var variavelProduto = await _context.VariaveisProdutos
                .Include(v => v.Produto)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (variavelProduto == null)
                return NotFound();

            return View(variavelProduto);
        }

        // GET: VariaveisProdutos/Create
        public IActionResult Create()
        {
            // Usar NomeComercial ao exibir produtos
            ViewData["ProdutoId"] = new SelectList(
                _context.Produtos, "Id", "NomeComercial");
            return View();
        }

        // POST: VariaveisProdutos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Nome,Descricao,Tipo,ProdutoId,Status,Valor")] VariavelProduto variavelProduto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(variavelProduto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProdutoId"] = new SelectList(
                _context.Produtos, "Id", "NomeComercial", variavelProduto.ProdutoId);
            return View(variavelProduto);
        }

        // GET: VariaveisProdutos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var variavelProduto = await _context.VariaveisProdutos.FindAsync(id);
            if (variavelProduto == null)
                return NotFound();

            ViewData["ProdutoId"] = new SelectList(
                _context.Produtos, "Id", "NomeComercial", variavelProduto.ProdutoId);
            return View(variavelProduto);
        }

        // POST: VariaveisProdutos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("Id,Nome,Descricao,Tipo,ProdutoId,Status,Valor")] VariavelProduto variavelProduto)
        {
            if (id != variavelProduto.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(variavelProduto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VariavelProdutoExists(variavelProduto.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProdutoId"] = new SelectList(
                _context.Produtos, "Id", "NomeComercial", variavelProduto.ProdutoId);
            return View(variavelProduto);
        }

        // GET: VariaveisProdutos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var variavelProduto = await _context.VariaveisProdutos
                .Include(v => v.Produto)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (variavelProduto == null)
                return NotFound();

            return View(variavelProduto);
        }

        // POST: VariaveisProdutos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var variavelProduto = await _context.VariaveisProdutos.FindAsync(id);
            if (variavelProduto != null)
            {
                _context.VariaveisProdutos.Remove(variavelProduto);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool VariavelProdutoExists(int id)
        {
            return _context.VariaveisProdutos.Any(e => e.Id == id);
        }
    }
}
