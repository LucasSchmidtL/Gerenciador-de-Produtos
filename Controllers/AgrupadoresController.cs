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
            // Inclui vínculos e detalhes dos itens ERP
            var agrupadores = await _context.Agrupadores
                .Include(a => a.AgrupadorItensERP)
                    .ThenInclude(ai => ai.ItemERP)
                .ToListAsync();
            return View(agrupadores);
        }

        // GET: Agrupadores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var agrupador = await _context.Agrupadores
                .Include(a => a.AgrupadorItensERP)
                    .ThenInclude(ai => ai.ItemERP)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (agrupador == null) return NotFound();

            return View(agrupador);
        }

        // GET: Agrupadores/Create
        public IActionResult Create()
        {
            // Carrega lista de ItemERP para multiselect
            ViewData["ItemERPs"] = new MultiSelectList(
                _context.Set<ItemERP>().OrderBy(i => i.ERP).ToList(),
                "Id",
                "ERP"
            );
            return View();
        }

        // POST: Agrupadores/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Agrupador model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["ItemERPs"] = new MultiSelectList(
                    _context.Set<ItemERP>().OrderBy(i => i.ERP).ToList(),
                    "Id",
                    "ERP",
                    model.ItemErpIds
                );
                return View(model);
            }

            // 1) persiste o agrupador básico
            _context.Add(model);
            await _context.SaveChangesAsync();

            // 2) vincula cada ItemERP selecionado
            if (model.ItemErpIds?.Any() == true)
            {
                foreach (var itemId in model.ItemErpIds)
                {
                    _context.Set<AgrupadorItemERP>().Add(new AgrupadorItemERP
                    {
                        AgrupadorId = model.Id,
                        ItemERPId = itemId
                    });
                }
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Agrupadores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var agrupador = await _context.Agrupadores
                .Include(a => a.AgrupadorItensERP)
                .FirstOrDefaultAsync(a => a.Id == id);
            if (agrupador == null) return NotFound();

            // pré-seleciona no multiselect
            agrupador.ItemErpIds = agrupador.AgrupadorItensERP
                .Select(x => x.ItemERPId)
                .ToList();

            ViewData["ItemERPs"] = new MultiSelectList(
                _context.Set<ItemERP>().OrderBy(i => i.ERP).ToList(),
                "Id",
                "ERP",
                agrupador.ItemErpIds
            );
            return View(agrupador);
        }

        // POST: Agrupadores/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Agrupador model)
        {
            if (id != model.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                ViewData["ItemERPs"] = new MultiSelectList(
                    _context.Set<ItemERP>().OrderBy(i => i.ERP).ToList(),
                    "Id",
                    "ERP",
                    model.ItemErpIds
                );
                return View(model);
            }

            // atualiza agrupador básico
            _context.Update(model);
            await _context.SaveChangesAsync();

            // sincroniza vínculos: remove antigos e adiciona novos
            var existentes = _context.Set<AgrupadorItemERP>()
                .Where(x => x.AgrupadorId == id);
            _context.Set<AgrupadorItemERP>().RemoveRange(existentes);

            if (model.ItemErpIds?.Any() == true)
            {
                foreach (var itemId in model.ItemErpIds)
                {
                    _context.Set<AgrupadorItemERP>().Add(new AgrupadorItemERP
                    {
                        AgrupadorId = id,
                        ItemERPId = itemId
                    });
                }
            }
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Agrupadores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var agrupador = await _context.Agrupadores.FindAsync(id);
            if (agrupador == null) return NotFound();

            return View(agrupador);
        }

        // POST: Agrupadores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var agrupador = await _context.Agrupadores.FindAsync(id);
            if (agrupador != null)
                _context.Agrupadores.Remove(agrupador);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AgrupadorExists(int id)
        {
            return _context.Agrupadores.Any(e => e.Id == id);
        }
    }
}