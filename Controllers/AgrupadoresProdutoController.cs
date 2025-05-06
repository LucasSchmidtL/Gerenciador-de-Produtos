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
    // ViewModel para exibir listas de produtos ↔ agrupadores
    public class AgrupadorProdutoViewModel
    {
        public int VinculoId { get; set; }
        public int ProdutoId { get; set; }
        public string NomeComercial { get; set; } = null!;
        public int AgrupadorId { get; set; }
        public string AgrupadorNome { get; set; } = null!;
        public string? Variavel { get; set; }
        public bool Status { get; set; }
    }

    public class AgrupadoresProdutoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AgrupadoresProdutoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AgrupadoresProduto
        // Exibe todas as relações em um ViewModel customizado
        public async Task<IActionResult> Index()
        {
            var itens = await _context.ProdutoAgrupadores
                .Include(pa => pa.Produto)
                .Include(pa => pa.Agrupador)
                .Select(pa => new AgrupadorProdutoViewModel
                {
                    VinculoId = pa.Id,
                    ProdutoId = pa.ProdutoId,
                    NomeComercial = pa.Produto.NomeComercial,
                    AgrupadorId = pa.AgrupadorId,
                    AgrupadorNome = pa.Agrupador.Nome,
                    Variavel = pa.Variavel,
                    Status = pa.Status
                })
                .ToListAsync();

            return View(itens);
        }

        // GET: AgrupadoresProduto/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var pa = await _context.ProdutoAgrupadores
                .Include(a => a.Produto)
                .Include(a => a.Agrupador)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pa == null) return NotFound();

            var vm = new AgrupadorProdutoViewModel
            {
                VinculoId = pa.Id,
                ProdutoId = pa.ProdutoId,
                NomeComercial = pa.Produto.NomeComercial,
                AgrupadorId = pa.AgrupadorId,
                AgrupadorNome = pa.Agrupador.Nome,
                Variavel = pa.Variavel,
                Status = pa.Status
            };
            return View(vm);
        }

        // GET: AgrupadoresProduto/Create
        public IActionResult Create(int? produtoId)
        {
            var model = new AgrupadorProduto();
            if (produtoId.HasValue)
                model.ProdutoId = produtoId.Value;

            PopulateDropdowns(model.ProdutoId, model.AgrupadorId);
            return View(model);
        }

        // POST: AgrupadoresProduto/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProdutoId,AgrupadorId,Variavel,Status")] AgrupadorProduto agrupadorProduto)
        {
            if (!ModelState.IsValid)
            {
                var erros = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .Select(x => new { Campo = x.Key, Mensagens = x.Value.Errors.Select(e => e.ErrorMessage) })
                    .ToList();
                ViewData["ModelErrors"] = erros;
                PopulateDropdowns(agrupadorProduto.ProdutoId, agrupadorProduto.AgrupadorId);
                return View(agrupadorProduto);
            }

            _context.Add(agrupadorProduto);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Produtos", new { id = agrupadorProduto.ProdutoId });
        }

        // GET: AgrupadoresProduto/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var agrupadorProduto = await _context.ProdutoAgrupadores.FindAsync(id);
            if (agrupadorProduto == null) return NotFound();

            PopulateDropdowns(agrupadorProduto.ProdutoId, agrupadorProduto.AgrupadorId);
            return View(agrupadorProduto);
        }

        // POST: AgrupadoresProduto/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProdutoId,AgrupadorId,Variavel,Status")] AgrupadorProduto agrupadorProduto)
        {
            if (id != agrupadorProduto.Id) return NotFound();

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
                if (!AgrupadorProdutoExists(agrupadorProduto.Id)) return NotFound(); else throw;
            }

            return RedirectToAction("Details", "Produtos", new { id = agrupadorProduto.ProdutoId });
        }

        // GET: AgrupadoresProduto/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var pa = await _context.ProdutoAgrupadores
                .Include(a => a.Produto)
                .Include(a => a.Agrupador)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pa == null) return NotFound();

            var vm = new AgrupadorProdutoViewModel
            {
                VinculoId = pa.Id,
                ProdutoId = pa.ProdutoId,
                NomeComercial = pa.Produto.NomeComercial,
                AgrupadorId = pa.AgrupadorId,
                AgrupadorNome = pa.Agrupador.Nome,
                Variavel = pa.Variavel,
                Status = pa.Status
            };
            return View(vm);
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
            return RedirectToAction("Details", "Produtos", new { id = item!.ProdutoId });
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