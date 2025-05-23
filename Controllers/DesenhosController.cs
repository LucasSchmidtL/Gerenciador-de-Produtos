﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Gerenciador_de_Produtos.Data;
using Gerenciador_de_Produtos.Models;

namespace Gerenciador_de_Produtos.Controllers
{
    public class DesenhosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DesenhosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Desenhos
        public async Task<IActionResult> Index()
        {
            var desenhos = await _context.Desenhos
                .Include(d => d.ItemERP)
                .ToListAsync();
            return View(desenhos);
        }

        // GET: Desenhos/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null) return NotFound();

            var desenho = await _context.Desenhos
                .Include(d => d.ItemERP)
                .FirstOrDefaultAsync(m => m.DesenhoId == id);

            if (desenho == null) return NotFound();

            return View(desenho);
        }

        // GET: Desenhos/Create
        public IActionResult Create()
        {
            ViewData["ItemERPId"] = new SelectList(_context.ItensERP, "Id", "ERP");
            return View();
        }

        // POST: Desenhos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nome,Descricao,Revisao,Status,Classificacao,SolicitacaoAlteracaoId,ItemERPId")] Desenho desenho)
        {
            if (ModelState.IsValid)
            {
                _context.Add(desenho);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["ItemERPId"] = new SelectList(_context.ItensERP, "Id", "ERP", desenho.ItemERPId);
            return View(desenho);
        }

        // GET: Desenhos/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null) return NotFound();

            var desenho = await _context.Desenhos.FindAsync(id);
            if (desenho == null) return NotFound();

            ViewData["ItemERPId"] = new SelectList(_context.ItensERP, "Id", "ERP", desenho.ItemERPId);
            return View(desenho);
        }

        // POST: Desenhos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("DesenhoId,Nome,Descricao,Revisao,Status,Classificacao,SolicitacaoAlteracaoId,ItemERPId")] Desenho desenho)
        {
            if (id != desenho.DesenhoId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(desenho);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DesenhoExists(desenho.DesenhoId)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["ItemERPId"] = new SelectList(_context.ItensERP, "Id", "ERP", desenho.ItemERPId);
            return View(desenho);
        }

        // GET: Desenhos/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null) return NotFound();

            var desenho = await _context.Desenhos
                .Include(d => d.ItemERP)
                .FirstOrDefaultAsync(m => m.DesenhoId == id);

            if (desenho == null) return NotFound();

            return View(desenho);
        }

        // POST: Desenhos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var desenho = await _context.Desenhos.FindAsync(id);
            if (desenho != null)
                _context.Desenhos.Remove(desenho);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DesenhoExists(long id)
        {
            return _context.Desenhos.Any(e => e.DesenhoId == id);
        }
    }
}
