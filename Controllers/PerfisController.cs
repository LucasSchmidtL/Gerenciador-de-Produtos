using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using Gerenciador_de_Produtos.Data;
using Gerenciador_de_Produtos.Models;
using Gerenciador_de_Produtos.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Gerenciador_de_Produtos.Controllers
{
    public class PerfisController : Controller
    {
        private readonly ApplicationDbContext _context;
        public PerfisController(ApplicationDbContext context)
            => _context = context;

        // GET: Perfis
        public async Task<IActionResult> Index()
        {
            var perfis = await _context.Perfis
                .Include(p => p.PerfilItemERPs)
                .ToListAsync();

            var allErps = await _context.ItensERP
                .Select(i => new SelectListItem
                {
                    Value = i.Id.ToString(),
                    Text = $"{i.ERP} – {i.Descricao}"
                })
                .ToListAsync();

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
                SimetricoX = p.SimetricoX.GetValueOrDefault(),
                SimetricoY = p.SimetricoY.GetValueOrDefault(),
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

            // limpa vínculos antigos
            _context.PerfilItemERPs.RemoveRange(perfil.PerfilItemERPs);

            // adiciona novos
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
            var perfil = await _context.Perfis
                .Include(p => p.PerfilItemERPs).ThenInclude(pi => pi.ItemERP)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (perfil == null) return NotFound();
            return View(perfil);
        }

        // GET: Perfis/Create
        public async Task<IActionResult> Create()
        {
            var vm = new PerfilItemERPViewModel
            {
                TodosItensERP = await _context.ItensERP
                    .Select(i => new SelectListItem
                    {
                        Value = i.Id.ToString(),
                        Text = $"{i.ERP} – {i.Descricao}"
                    }).ToListAsync()
            };
            return View(vm);
        }

        // POST: Perfis/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PerfilItemERPViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.TodosItensERP = await _context.ItensERP
                    .Select(i => new SelectListItem
                    {
                        Value = i.Id.ToString(),
                        Text = $"{i.ERP} – {i.Descricao}"
                    }).ToListAsync();
                return View(vm);
            }

            var perfil = new Perfil
            {
                Desenho = vm.Desenho,
                Descricao = vm.Descricao,
                TipoSecao = vm.TipoSecao,
                Peso = vm.Peso,
                AreaBruta = vm.AreaBruta,
                AreaLiq = vm.AreaLiq,
                AreaEq = vm.AreaEq,
                Ix = vm.Ix,
                Sxt = vm.Sxt,
                Sxb = vm.Sxb,
                Zx = vm.Zx,
                Rx = vm.Rx,
                yt = vm.yt,
                yb = vm.yb,
                Ixy = vm.Ixy,
                Iy = vm.Iy,
                Syl = vm.Syl,
                Syr = vm.Syr,
                Zy = vm.Zy,
                ry = vm.ry,
                xl = vm.xl,
                xr = vm.xr,
                xo = vm.xo,
                yo = vm.yo,
                jx = vm.jx,
                jy = vm.jy,
                Cw = vm.Cw,
                J = vm.J,
                Ixe = vm.Ixe,
                Sxet = vm.Sxet,
                Sxeb = vm.Sxeb,
                lye = vm.lye,
                Syel = vm.Syel,
                Syer = vm.Syer,
                p1 = vm.p1,
                p2 = vm.p2,
                p3 = vm.p3,
                SimetricoX = vm.SimetricoX,
                SimetricoY = vm.SimetricoY,
                PerfilItemERPs = vm.ItensERPSelecionados
                    .Select(id => new PerfilItemERP { ItemERPId = id })
                    .ToList()
            };

            _context.Perfis.Add(perfil);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Perfis/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var perfil = await _context.Perfis
                .Include(p => p.PerfilItemERPs)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (perfil == null) return NotFound();

            var vm = new PerfilItemERPViewModel
            {
                Id = perfil.Id,
                Desenho = perfil.Desenho,
                Descricao = perfil.Descricao,
                TipoSecao = perfil.TipoSecao,
                Peso = perfil.Peso,
                AreaBruta = perfil.AreaBruta,
                AreaLiq = perfil.AreaLiq,
                AreaEq = perfil.AreaEq,
                Ix = perfil.Ix,
                Sxt = perfil.Sxt,
                Sxb = perfil.Sxb,
                Zx = perfil.Zx,
                Rx = perfil.Rx,
                yt = perfil.yt,
                yb = perfil.yb,
                Ixy = perfil.Ixy,
                Iy = perfil.Iy,
                Syl = perfil.Syl,
                Syr = perfil.Syr,
                Zy = perfil.Zy,
                ry = perfil.ry,
                xl = perfil.xl,
                xr = perfil.xr,
                xo = perfil.xo,
                yo = perfil.yo,
                jx = perfil.jx,
                jy = perfil.jy,
                Cw = perfil.Cw,
                J = perfil.J,
                Ixe = perfil.Ixe,
                Sxet = perfil.Sxet,
                Sxeb = perfil.Sxeb,
                lye = perfil.lye,
                Syel = perfil.Syel,
                Syer = perfil.Syer,
                p1 = perfil.p1,
                p2 = perfil.p2,
                p3 = perfil.p3,
                SimetricoX = perfil.SimetricoX.GetValueOrDefault(),
                SimetricoY = perfil.SimetricoY.GetValueOrDefault(),
                ItensERPSelecionados = perfil.PerfilItemERPs.Select(pi => pi.ItemERPId).ToList(),
                TodosItensERP = await _context.ItensERP
                                          .Select(i => new SelectListItem
                                          {
                                              Value = i.Id.ToString(),
                                              Text = $"{i.ERP} – {i.Descricao}"
                                          }).ToListAsync()
            };

            return View(vm);
        }

        // POST: Perfis/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PerfilItemERPViewModel vm)
        {
            if (!vm.Id.HasValue) return BadRequest();
            if (!ModelState.IsValid)
            {
                vm.TodosItensERP = await _context.ItensERP
                    .Select(i => new SelectListItem
                    {
                        Value = i.Id.ToString(),
                        Text = $"{i.ERP} – {i.Descricao}"
                    }).ToListAsync();
                return View(vm);
            }

            var perfil = await _context.Perfis
                .Include(p => p.PerfilItemERPs)
                .FirstOrDefaultAsync(p => p.Id == vm.Id.Value);
            if (perfil == null) return NotFound();

            perfil.Desenho = vm.Desenho;
            perfil.Descricao = vm.Descricao;
            perfil.TipoSecao = vm.TipoSecao;
            perfil.Peso = vm.Peso;
            perfil.AreaBruta = vm.AreaBruta;
            perfil.AreaLiq = vm.AreaLiq;
            perfil.AreaEq = vm.AreaEq;
            perfil.Ix = vm.Ix;
            perfil.Sxt = vm.Sxt;
            perfil.Sxb = vm.Sxb;
            perfil.Zx = vm.Zx;
            perfil.Rx = vm.Rx;
            perfil.yt = vm.yt;
            perfil.yb = vm.yb;
            perfil.Ixy = vm.Ixy;
            perfil.Iy = vm.Iy;
            perfil.Syl = vm.Syl;
            perfil.Syr = vm.Syr;
            perfil.Zy = vm.Zy;
            perfil.ry = vm.ry;
            perfil.xl = vm.xl;
            perfil.xr = vm.xr;
            perfil.xo = vm.xo;
            perfil.yo = vm.yo;
            perfil.jx = vm.jx;
            perfil.jy = vm.jy;
            perfil.Cw = vm.Cw;
            perfil.J = vm.J;
            perfil.Ixe = vm.Ixe;
            perfil.Sxet = vm.Sxet;
            perfil.Sxeb = vm.Sxeb;
            perfil.lye = vm.lye;
            perfil.Syel = vm.Syel;
            perfil.Syer = vm.Syer;
            perfil.p1 = vm.p1;
            perfil.p2 = vm.p2;
            perfil.p3 = vm.p3;
            perfil.SimetricoX = vm.SimetricoX;
            perfil.SimetricoY = vm.SimetricoY;

            _context.PerfilItemERPs.RemoveRange(perfil.PerfilItemERPs);
            foreach (var itemId in vm.ItensERPSelecionados.Distinct())
                perfil.PerfilItemERPs.Add(new PerfilItemERP
                {
                    PerfilId = perfil.Id,
                    ItemERPId = itemId
                });

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



        // GET: Perfis/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var perfil = await _context.Perfis
                .FirstOrDefaultAsync(m => m.Id == id);
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
        public IActionResult Import() => View();

        // GET: Perfis/DownloadTemplate
        public FileResult DownloadTemplate()
        {
            var csvLines = new List<string> {
                "ERP,Desenho,Descricao,TipoSecao,Peso,AreaBruta,AreaLiq,AreaEq,Ix,Sxt,Sxb,Zx,Rx,yt,yb,Ixy,Iy,Syl,Syr,Zy,ry,xl,xr,xo,yo,jx,jy,Cw,J,Ixe,Sxet,Sxeb,lye,Syel,Syer,p1,p2,p3,SimetricoX,SimetricoY",
                "ERP123,DES001,Exemplo,SecaoA,12.5,100,95,90,200,30,25,50,5,10,8,2,150,20,15,75,8.7,3.2,1.5,0.5,0.5,0.8,0.9,0.2,0.1,180,25,20,5,3,1,0.5,0.3,0.2,TRUE,FALSE"
            };
            var bytes = System.Text.Encoding.UTF8.GetBytes(string.Join("\r\n", csvLines));
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
            var cfg = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                MissingFieldFound = null,
                HeaderValidated = null
            };
            using var csv = new CsvReader(reader, cfg);
            var records = csv.GetRecords<Perfil>().ToList();

            await _context.Perfis.AddRangeAsync(records);
            await _context.SaveChangesAsync();
            TempData["Success"] = $"{records.Count} perfis importados com sucesso!";
            return RedirectToAction(nameof(Index));
        }



        private bool PerfilExists(int id)
            => _context.Perfis.Any(e => e.Id == id);
    }
}
