using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Gerenciador_de_Produtos.Data;
using Gerenciador_de_Produtos.Models;
using Gerenciador_de_Produtos.Models.ViewModels;


namespace Gerenciador_de_Produtos.Controllers
{
    public class TagsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TagsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var tags = await _context.Tags
                .Include(t => t.ItemERPs)
                .ToListAsync();

            return View(tags);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var tag = await _context.Tags
                .Include(t => t.ItemERPs)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (tag == null) return NotFound();

            return View(tag);
        }

        public async Task<IActionResult> Create()
        {
            var vm = new TagEditViewModel
            {
                AllItemERPs = await _context.ItensERP
                    .Select(i => new SelectListItem(i.ERP, i.Id.ToString()))
                    .ToListAsync()
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TagEditViewModel vm)
        {
            if (await _context.Tags.AnyAsync(t => t.Nome == vm.Nome))
            {
                ModelState.AddModelError("Nome", "Já existe uma tag com esse nome.");
            }

            if (!ModelState.IsValid)
            {
                vm.AllItemERPs = await _context.ItensERP
                    .Select(i => new SelectListItem(i.ERP, i.Id.ToString()))
                    .ToListAsync();
                return View(vm);
            }

            var novaTag = new Tag
            {
                Nome = vm.Nome
            };

            _context.Tags.Add(novaTag);
            await _context.SaveChangesAsync();

            if (vm.SelectedItemERPIds != null)
            {
                var itens = await _context.ItensERP
                    .Where(i => vm.SelectedItemERPIds.Contains(i.Id))
                    .ToListAsync();

                foreach (var item in itens)
                    novaTag.ItemERPs.Add(item);

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }


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

            ViewBag.ItensSelecionados = await _context.ItensERP
                .Where(i => vm.SelectedItemERPIds.Contains(i.Id))
                .Select(i => new SelectListItem
                {
                    Value = i.Id.ToString(),
                    Text = $"{i.ERP} | {i.Descricao} | {i.TipoItem}"
                })
                .ToListAsync();

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TagEditViewModel vm)
        {
            if (id != vm.Id) return NotFound();

            if (await _context.Tags.AnyAsync(t => t.Nome == vm.Nome && t.Id != vm.Id))
            {
                ModelState.AddModelError("Nome", "Já existe uma tag com esse nome.");
            }

            if (!ModelState.IsValid)
            {
                vm.AllItemERPs = await _context.ItensERP
                    .Select(i => new SelectListItem(i.ERP, i.Id.ToString()))
                    .ToListAsync();
                return View(vm);
            }

            var tag = await _context.Tags
                .Include(t => t.ItemERPs)
                .FirstOrDefaultAsync(t => t.Id == id);

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


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var tag = await _context.Tags
                .Include(t => t.ItemERPs)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (tag == null) return NotFound();

            return View(tag);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
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

        private bool TagExists(int id) => _context.Tags.Any(t => t.Id == id);
    }
}