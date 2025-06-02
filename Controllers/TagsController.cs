using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Gerenciador_de_Produtos.Data;
using Gerenciador_de_Produtos.Models;

namespace Gerenciador_de_Produtos.Controllers
{
    public class TagsController : Controller
    {
        private readonly ApplicationDbContext _context;
        public TagsController(ApplicationDbContext context) => _context = context;

        // GET: Tags
        public async Task<IActionResult> Index()
        {
            var tags = await _context.Tags
                .Include(t => t.ItemERPs)
                .ToListAsync();

            return View(tags);
        }

        // GET: Tags/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var tag = await _context.Tags
                .Include(t => t.ItemERPs)
                .FirstOrDefaultAsync(t => t.Id == id);
            if (tag == null) return NotFound();

            return View(tag);
        }

        // GET: Tags/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.AllItemERPs = await _context.ItensERP
            .Select(i => new SelectListItem
            {
                Value = i.Id.ToString(),
                Text = $"{i.ERP} - {i.TipoItem} ({i.Descricao})"
            })
            .ToListAsync();

            return View();
        }

        // POST: Tags/Create
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TagEditViewModel vm)
        {
            if (_context.Tags.Any(t => t.Nome == vm.Nome))
            {
                ModelState.AddModelError("Nome", "Já existe uma tag com este nome.");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.AllItemERPs = new SelectList(
                    await _context.ItensERP.ToListAsync(),
                    "Id", "ERP", vm.SelectedItemERPIds);
                return View(vm);
            }

            var tag = new Tag { Nome = vm.Nome };
            var itens = await _context.ItensERP
                .Where(i => vm.SelectedItemERPIds.Contains(i.Id))
                .ToListAsync();
            foreach (var item in itens)
                tag.ItemERPs.Add(item);

            _context.Tags.Add(tag);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Tags/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var tag = await _context.Tags
                .Include(t => t.ItemERPs)
                .FirstOrDefaultAsync(t => t.Id == id);
            if (tag == null) return NotFound();

            var vm = new TagEditViewModel
            {
                Id = tag.Id,
                Nome = tag.Nome,
                SelectedItemERPIds = tag.ItemERPs.Select(i => i.Id).ToList(),
                AllItemERPs = await _context.ItensERP
                    .Select(i => new SelectListItem(i.ERP, i.Id.ToString()))
                    .ToListAsync()
            };
            return View(vm);
        }

        // POST: Tags/Edit/5
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TagEditViewModel vm)
        {
            if (_context.Tags.Any(t => t.Nome == vm.Nome && t.Id != vm.Id))
            {
                ModelState.AddModelError("Nome", "Já existe uma tag com este nome.");
            }

            if (!ModelState.IsValid)
            {
                vm.AllItemERPs = await _context.ItensERP
                    .Select(i => new SelectListItem(
                        i.ERP,
                        i.Id.ToString(),
                        vm.SelectedItemERPIds.Contains(i.Id)))
                    .ToListAsync();
                return View(vm);
            }


            var tag = await _context.Tags
                .Include(t => t.ItemERPs)
                .FirstOrDefaultAsync(t => t.Id == vm.Id);
            if (tag == null) return NotFound();

            tag.Nome = vm.Nome;
            tag.ItemERPs.Clear();
            var selected = await _context.ItensERP
                .Where(i => vm.SelectedItemERPIds.Contains(i.Id))
                .ToListAsync();
            foreach (var item in selected)
                tag.ItemERPs.Add(item);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Tags/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var tag = await _context.Tags
                .Include(t => t.ItemERPs)
                .FirstOrDefaultAsync(t => t.Id == id);
            if (tag == null) return NotFound();

            return View(tag);
        }

        // POST: Tags/Delete/5
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tag = await _context.Tags.FindAsync(id);
            if (tag != null)
            {
                _context.Tags.Remove(tag);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool TagExists(int id)
            => _context.Tags.Any(e => e.Id == id);
    }

    public class TagEditViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; } = null!;
        public List<int> SelectedItemERPIds { get; set; } = new();
        public List<SelectListItem> AllItemERPs { get; set; } = new();
    }
}