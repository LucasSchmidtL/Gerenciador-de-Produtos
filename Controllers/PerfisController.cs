using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using Gerenciador_de_Produtos.Data;
using Gerenciador_de_Produtos.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Gerenciador_de_Produtos.Controllers
{
    public class PerfisController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PerfisController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Perfis
        public async Task<IActionResult> Index()
        {
            // carrega perfis com seus itens ERP
            var perfis = await _context.Perfis
                .Include(p => p.PerfilItemERPs)
                .ToListAsync();

            // lista base de todos os ItensERP (ERP + Descrição)
            var allErps = await _context.ItensERP
                .Select(i => new SelectListItem
                {
                    Value = i.Id.ToString(),
                    Text = $"{i.ERP} – {i.Descricao}"
                })
                .ToListAsync();

            // mapeia para o ViewModel
            var vm = perfis.Select(p => new PerfilItemERPViewModel
            {
                Id = p.Id,
                Desenho = p.Desenho,
                Descricao = p.Descricao,
                TipoSecao = p.TipoSecao,
                Peso = p.Peso,
                AreaBruta = p.AreaBruta,
                AreaLiq = p.AreaLiq,
                AreaEq = p.AreaEq,
                Ix = p.Ix,
                Sxt = p.Sxt,
                Sxb = p.Sxb,
                Zx = p.Zx,
                Rx = p.Rx,
                yt = p.yt,
                yb = p.yb,
                Ixy = p.Ixy,
                Iy = p.Iy,
                Syl = p.Syl,
                Syr = p.Syr,
                Zy = p.Zy,
                ry = p.ry,
                xl = p.xl,
                xr = p.xr,
                xo = p.xo,
                yo = p.yo,
                jx = p.jx,
                jy = p.jy,
                Cw = p.Cw,
                J = p.J,
                Ixe = p.Ixe,
                Sxet = p.Sxet,
                Sxeb = p.Sxeb,
                lye = p.lye,
                Syel = p.Syel,
                Syer = p.Syer,
                p1 = p.p1,
                p2 = p.p2,
                p3 = p.p3,
                SimetricoX = p.SimetricoX,
                SimetricoY = p.SimetricoY,

                // popula dropdown + selecionados
                TodosItensERP = allErps,
                ItensERPSelecionados = p.PerfilItemERPs.Select(pi => pi.ItemERPId).ToList()
            })
            .ToList();

            return View(vm);
        }

        // POST: Perfis/AtualizaItens
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AtualizaItens(PerfilItemERPViewModel vm)
        {
            if (!vm.Id.HasValue) return BadRequest();

            var perfil = await _context.Perfis
                .Include(p => p.PerfilItemERPs)
                .FirstOrDefaultAsync(p => p.Id == vm.Id.Value);

            if (perfil == null) return NotFound();

            // limpa os antigos vínculos
            _context.PerfilItemERPs.RemoveRange(perfil.PerfilItemERPs);

            // adiciona os novos (evita duplicatas)
            foreach (var itemId in vm.ItensERPSelecionados.Distinct())
            {
                perfil.PerfilItemERPs.Add(new PerfilItemERP
                {
                    PerfilId = perfil.Id,
                    ItemERPId = itemId
                });
            }

            await _context.SaveChangesAsync();
            TempData["Success"] = "Itens ERP atualizados!";
            return RedirectToAction(nameof(Index));
        }

        // GET: Perfis/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var perfil = await _context.Perfis.FirstOrDefaultAsync(m => m.Id == id);
            if (perfil == null) return NotFound();
            return View(perfil);
        }

        // GET: Perfis/Create
        public IActionResult Create()
        {
            ViewBag.ItemERPId = new SelectList(_context.ItensERP, "Id", "ERP");
            return View();
        }

        // POST: Perfis/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ERP,Desenho,Descricao,TipoSecao,Peso,AreaBruta,AreaLiq,AreaEq,Ix,Sxt,Sxb,Zx,Rx,yt,yb,Ixy,Iy,Syl,Syr,Zy,ry,xl,xr,xo,yo,jx,jy,Cw,J,Ixe,Sxet,Sxeb,lye,Syel,Syer,p1,p2,p3,SimetricoX,SimetricoY,ItemERPId")] Perfil perfil)
        {
            if (ModelState.IsValid)
            {
                _context.Add(perfil);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.ItemERPId = new SelectList(_context.ItensERP, "Id", "ERP", perfil.ItemERPId);
            return View(perfil);
        }


        // GET: Perfis/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var perfil = await _context.Perfis.FindAsync(id);
            if (perfil == null) return NotFound();
            return View(perfil);
        }

        // POST: Perfis/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ERP,Desenho,Descricao,TipoSecao,Peso,AreaBruta,AreaLiq,AreaEq,Ix,Sxt,Sxb,Zx,Rx,yt,yb,Ixy,Iy,Syl,Syr,Zy,ry,xl,xr,xo,yo,jx,jy,Cw,J,Ixe,Sxet,Sxeb,lye,Syel,Syer,p1,p2,p3,SimetricoX,SimetricoY")] Perfil perfil)
        {
            if (id != perfil.Id) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(perfil);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PerfilExists(perfil.Id)) return NotFound(); else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(perfil);
        }

        // GET: Perfis/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var perfil = await _context.Perfis.FirstOrDefaultAsync(m => m.Id == id);
            if (perfil == null) return NotFound();
            return View(perfil);
        }

        // POST: Perfis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var perfil = await _context.Perfis.FindAsync(id);
            if (perfil != null)
            {
                _context.Perfis.Remove(perfil);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Perfis/Import
        public IActionResult Import()
        {
            // Exibe a view de upload
            return View();
        }

        // GET: Perfis/DownloadTemplate
        public FileResult DownloadTemplate()
        {
            var csvLines = new List<string>
            {
                // Cabeçalho (sem Id)
                "ERP,Desenho,Descricao,TipoSecao,Peso,AreaBruta,AreaLiq,AreaEq,Ix,Sxt,Sxb,Zx,Rx,yt,yb,Ixy,Iy,Syl,Syr,Zy,ry,xl,xr,xo,yo,jx,jy,Cw,J,Ixe,Sxet,Sxeb,lye,Syel,Syer,p1,p2,p3,SimetricoX,SimetricoY",
                // Exemplo de linha
                "ERP123,DES001,Exemplo,SecaoA,12.5,100,95,90,200,30,25,50,5,10,8,2,150,20,15,75,8.7,3.2,1.5,0.5,0.5,0.8,0.9,0.2,0.1,180,25,20,5,3,1,0.5,0.3,0.2,TRUE,FALSE"
            };
            var csvContent = string.Join("\r\n", csvLines);
            var bytes = System.Text.Encoding.UTF8.GetBytes(csvContent);
            return File(bytes, "text/csv", "template_perfis.csv");
        }

        // POST: Perfis/Import
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Import(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("file", "Selecione um arquivo CSV.");
                return View();
            }

            using var stream = file.OpenReadStream();
            using var reader = new StreamReader(stream);
            var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                MissingFieldFound = null,
                HeaderValidated = null
            };
            using var csv = new CsvReader(reader, csvConfig);

            var records = csv.GetRecords<Perfil>().ToList();
            await _context.Perfis.AddRangeAsync(records);
            await _context.SaveChangesAsync();

            TempData["Success"] = $"{records.Count} perfis importados com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        private bool PerfilExists(int id) => _context.Perfis.Any(e => e.Id == id);
    }
}
