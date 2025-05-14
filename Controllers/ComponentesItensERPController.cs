using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Gerenciador_de_Produtos.Data;
using Gerenciador_de_Produtos.Models;

namespace Gerenciador_de_Produtos.Controllers
{
    public class ComponentesItensERPController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ComponentesItensERPController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ComponentesItensERP
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ComponenteItemERPs
                .Include(c => c.Componente)
                .Include(c => c.ItemERP);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ComponentesItensERP/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var componenteItemERP = await _context.ComponenteItemERPs
                .Include(c => c.Componente)
                .Include(c => c.ItemERP)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (componenteItemERP == null) return NotFound();

            return View(componenteItemERP);
        }

        // GET: ComponentesItensERP/Create
        public IActionResult Create(int? componenteId)
        {
            // Dropdown de Componentes
            ViewData["ComponenteId"] = new SelectList(
                _context.Componentes.OrderBy(c => c.Nome),
                "Id", "Nome",
                componenteId
            );

            // Dropdown de Itens ERP
            ViewData["ItemERPId"] = new SelectList(
                _context.ItensERP.OrderBy(i => i.ERP),
                "Id", "ERP"
            );

            var model = new ComponenteItemERP();
            if (componenteId.HasValue)
                model.ComponenteId = componenteId.Value;

            return View(model);
        }

        // POST: ComponentesItensERP/Create
        [HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Create([Bind("ComponenteId,ItemERPId,Comprimento,Profundidade,Altura,Quantidade,Status")] ComponenteItemERP componenteItemERP)
{
    // Se entrou aqui, logamos no console todos os erros de validação
    if (!ModelState.IsValid)
    {
        var erros = ModelState
            .Where(m => m.Value.Errors.Any())
            .Select(m => $"{m.Key}: {string.Join(", ", m.Value.Errors.Select(e => e.ErrorMessage))}");
        var msg = string.Join(" | ", erros);
        // 1) Log no Debug do servidor
        System.Diagnostics.Debug.WriteLine($"[Create POST] ModelState inválido: {msg}");
        // 2) Disponibiliza para a View exibir
        ViewBag.DebugErrors = msg;
    }

    if (ModelState.IsValid)
    {
        try
        {
            _context.Add(componenteItemERP);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            // Loga qualquer exceção não esperada
            System.Diagnostics.Debug.WriteLine($"[Create POST] Exceção: {ex}");
            ViewBag.DebugErrors = ex.ToString();
        }
    }

    // Recarrega dropdowns em caso de erro
    ViewData["ComponenteId"] = new SelectList(
        _context.Componentes.OrderBy(c => c.Nome), "Id", "Nome", componenteItemERP.ComponenteId);
    ViewData["ItemERPId"] = new SelectList(
        _context.ItensERP.OrderBy(i => i.ERP), "Id", "ERP", componenteItemERP.ItemERPId);

    return View(componenteItemERP);
}


        // GET: ComponentesItensERP/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var componenteItemERP = await _context.ComponenteItemERPs.FindAsync(id);
            if (componenteItemERP == null) return NotFound();

            ViewData["ComponenteId"] = new SelectList(
                _context.Componentes.OrderBy(c => c.Nome),
                "Id", "Nome",
                componenteItemERP.ComponenteId
            );
            ViewData["ItemERPId"] = new SelectList(
                _context.ItensERP.OrderBy(i => i.ERP),
                "Id", "ERP",
                componenteItemERP.ItemERPId
            );
            return View(componenteItemERP);
        }

        // POST: ComponentesItensERP/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ComponenteId,ItemERPId,Comprimento,Profundidade,Altura,Quantidade,Status")] ComponenteItemERP componenteItemERP)
        {
            if (id != componenteItemERP.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(componenteItemERP);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.ComponenteItemERPs.Any(e => e.Id == componenteItemERP.Id))
                        return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["ComponenteId"] = new SelectList(
                _context.Componentes.OrderBy(c => c.Nome),
                "Id", "Nome",
                componenteItemERP.ComponenteId
            );
            ViewData["ItemERPId"] = new SelectList(
                _context.ItensERP.OrderBy(i => i.ERP),
                "Id", "ERP",
                componenteItemERP.ItemERPId
            );
            return View(componenteItemERP);
        }

        // GET: ComponentesItensERP/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var componenteItemERP = await _context.ComponenteItemERPs
                .Include(c => c.Componente)
                .Include(c => c.ItemERP)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (componenteItemERP == null) return NotFound();

            return View(componenteItemERP);
        }

        // POST: ComponentesItensERP/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var componenteItemERP = await _context.ComponenteItemERPs.FindAsync(id);
            if (componenteItemERP != null)
            {
                _context.ComponenteItemERPs.Remove(componenteItemERP);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ComponenteItemERPExists(int id)
        {
            return _context.ComponenteItemERPs.Any(e => e.Id == id);
        }
    }
}
