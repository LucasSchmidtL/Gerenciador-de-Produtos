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
    public class EquacoesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EquacoesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Equacoes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Equacao.ToListAsync());
        }

        // GET: Equacoes/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var equacao = await _context.Equacao
                .FirstOrDefaultAsync(m => m.Id == id);
            if (equacao == null)
            {
                return NotFound();
            }

            return View(equacao);
        }

        // GET: Equacoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Equacoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Funcao,NormaId,Entrada,Saida,Descricao,Consideracoes")] Equacao equacao)
        {
            if (ModelState.IsValid)
            {
                _context.Add(equacao);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(equacao);
        }

        // GET: Equacoes/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var equacao = await _context.Equacao.FindAsync(id);
            if (equacao == null)
            {
                return NotFound();
            }
            return View(equacao);
        }

        // POST: Equacoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Funcao,NormaId,Entrada,Saida,Descricao,Consideracoes")] Equacao equacao)
        {
            if (id != equacao.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(equacao);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EquacaoExists(equacao.Id))
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
            return View(equacao);
        }

        // GET: Equacoes/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var equacao = await _context.Equacao
                .FirstOrDefaultAsync(m => m.Id == id);
            if (equacao == null)
            {
                return NotFound();
            }

            return View(equacao);
        }

        // POST: Equacoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var equacao = await _context.Equacao.FindAsync(id);
            if (equacao != null)
            {
                _context.Equacao.Remove(equacao);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EquacaoExists(long id)
        {
            return _context.Equacao.Any(e => e.Id == id);
        }
    }
}
