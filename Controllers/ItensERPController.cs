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
    public class ItensERPController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ItensERPController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ItensERP
        public async Task<IActionResult> Index()
        {
            return View(await _context.ItensERP.ToListAsync());
        }

        // GET: ItensERP/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemERP = await _context.ItensERP
                .FirstOrDefaultAsync(m => m.Id == id);
            if (itemERP == null)
            {
                return NotFound();
            }

            return View(itemERP);
        }

        // GET: ItensERP/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ItensERP/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ERP,TipoItem,ItemERPIdOriginal,DesenhoId,Descricao,Revisao,DataCriacao,Status,Acabamento,ChapaAberta,AreaSuperficial,PesoLiquidoMetro,PesoBrutoMetro,PerimetroSolda,SRId,DesenvolvimentoId,QuantidadeDobras,MateriaPrimaId,TagId,Altura,Comprimento,Profundidade,ComprimentoMaximo,Passo,Classificacao")] ItemERP itemERP)
        {
            if (ModelState.IsValid)
            {
                _context.Add(itemERP);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(itemERP);
        }

        // GET: ItensERP/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemERP = await _context.ItensERP.FindAsync(id);
            if (itemERP == null)
            {
                return NotFound();
            }
            return View(itemERP);
        }

        // POST: ItensERP/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ERP,TipoItem,ItemERPIdOriginal,DesenhoId,Descricao,Revisao,DataCriacao,Status,Acabamento,ChapaAberta,AreaSuperficial,PesoLiquidoMetro,PesoBrutoMetro,PerimetroSolda,SRId,DesenvolvimentoId,QuantidadeDobras,MateriaPrimaId,TagId,Altura,Comprimento,Profundidade,ComprimentoMaximo,Passo,Classificacao")] ItemERP itemERP)
        {
            if (id != itemERP.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(itemERP);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemERPExists(itemERP.Id))
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
            return View(itemERP);
        }

        // GET: ItensERP/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemERP = await _context.ItensERP
                .FirstOrDefaultAsync(m => m.Id == id);
            if (itemERP == null)
            {
                return NotFound();
            }

            return View(itemERP);
        }

        // POST: ItensERP/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var itemERP = await _context.ItensERP.FindAsync(id);
            if (itemERP != null)
            {
                _context.ItensERP.Remove(itemERP);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ItemERPExists(int id)
        {
            return _context.ItensERP.Any(e => e.Id == id);
        }
    }
}
