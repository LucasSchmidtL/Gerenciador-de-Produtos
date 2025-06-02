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
using Gerenciador_de_Produtos.Models.ViewModels;
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
            var perfis = await _context.Perfis.ToListAsync();
            return View(perfis);
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
        public async Task<IActionResult> Create()
        {
            var viewModel = new PerfilItemERPViewModel
            {
                TodosItensERP = await _context.ItensERP
                    .Select(i => new SelectListItem
                    {
                        Value = i.Id.ToString(),
                        Text = i.Descricao

                    }).ToListAsync()
            };

            return View(viewModel);
        }


        // POST: Perfis/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PerfilItemERPViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.TodosItensERP = await _context.ItensERP
                    .Select(i => new SelectListItem
                    {
                        Value = i.Id.ToString(),
                        Text = $"{i.ERP} - {i.Descricao}"
                    }).ToListAsync();

                return View(model);
            }

            var perfil = new Perfil
            {
                Desenho = model.Desenho,
                Descricao = model.Descricao,
                TipoSecao = model.TipoSecao,
                Peso = model.Peso,
                AreaBruta = model.AreaBruta,
                AreaLiq = model.AreaLiq,
                AreaEq = model.AreaEq,
                Ix = model.Ix,
                Sxt = model.Sxt,
                Sxb = model.Sxb,
                Zx = model.Zx,
                Rx = model.Rx,
                yt = model.yt,
                yb = model.yb,
                Ixy = model.Ixy,
                Iy = model.Iy,
                Syl = model.Syl,
                Syr = model.Syr,
                Zy = model.Zy,
                ry = model.ry,
                xl = model.xl,
                xr = model.xr,
                xo = model.xo,
                yo = model.yo,
                jx = model.jx,
                jy = model.jy,
                Cw = model.Cw,
                J = model.J,
                Ixe = model.Ixe,
                Sxet = model.Sxet,
                Sxeb = model.Sxeb,
                lye = model.lye,
                Syel = model.Syel,
                Syer = model.Syer,
                p1 = model.p1,
                p2 = model.p2,
                p3 = model.p3,
                SimetricoX = model.SimetricoX,
                SimetricoY = model.SimetricoY,

                // Vínculo com os ItensERP selecionados
                PerfilItemERPs = model.ItensERPSelecionados
                    .Select(itemId => new PerfilItemERP
                    {
                        ItemERPId = itemId
                    }).ToList()
            };

            _context.Perfis.Add(perfil);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Desenho,Descricao,TipoSecao,Peso,AreaBruta,AreaLiq,AreaEq,Ix,Sxt,Sxb,Zx,Rx,yt,yb,Ixy,Iy,Syl,Syr,Zy,ry,xl,xr,xo,yo,jx,jy,Cw,J,Ixe,Sxet,Sxeb,lye,Syel,Syer,p1,p2,p3,SimetricoX,SimetricoY")] Perfil perfil)
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
                "Desenho,Descricao,TipoSecao,Peso,AreaBruta,AreaLiq,AreaEq,Ix,Sxt,Sxb,Zx,Rx,yt,yb,Ixy,Iy,Syl,Syr,Zy,ry,xl,xr,xo,yo,jx,jy,Cw,J,Ixe,Sxet,Sxeb,lye,Syel,Syer,p1,p2,p3,SimetricoX,SimetricoY",
                // Exemplo de linha
                "DES-123,Nome-Perfil,SecaoA,12.5,100,95,90,200,30,25,50,5,10,8,2,150,20,15,75,8.7,3.2,1.5,0.5,0.5,0.8,0.9,0.2,0.1,180,25,20,5,3,1,0.5,0.3,0.2,TRUE,FALSE"
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

            var erros = new List<string>();
            var perfisImportados = new List<Perfil>();

            using var stream = file.OpenReadStream();
            using var reader = new StreamReader(stream);
            var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                MissingFieldFound = null,
                HeaderValidated = null
            };
            using var csv = new CsvReader(reader, csvConfig);

            var records = csv.GetRecords<Perfil>().ToList();

            foreach (var record in records)
            {
                // Cria o perfil e vincula ao ItemERP existente
                var perfil = new Perfil
                {
                    Desenho = record.Desenho,
                    Descricao = record.Descricao,
                    TipoSecao = record.TipoSecao,
                    Peso = record.Peso,
                    AreaBruta = record.AreaBruta,
                    AreaLiq = record.AreaLiq,
                    AreaEq = record.AreaEq,
                    Ix = record.Ix,
                    Sxt = record.Sxt,
                    Sxb = record.Sxb,
                    Zx = record.Zx,
                    Rx = record.Rx,
                    yt = record.yt,
                    yb = record.yb,
                    Ixy = record.Ixy,
                    Iy = record.Iy,
                    Syl = record.Syl,
                    Syr = record.Syr,
                    Zy = record.Zy,
                    ry = record.ry,
                    xl = record.xl,
                    xr = record.xr,
                    xo = record.xo,
                    yo = record.yo,
                    jx = record.jx,
                    jy = record.jy,
                    Cw = record.Cw,
                    J = record.J,
                    Ixe = record.Ixe,
                    Sxet = record.Sxet,
                    Sxeb = record.Sxeb,
                    lye = record.lye,
                    Syel = record.Syel,
                    Syer = record.Syer,
                    p1 = record.p1,
                    p2 = record.p2,
                    p3 = record.p3,
                    SimetricoX = record.SimetricoX,
                    SimetricoY = record.SimetricoY
                };

                perfisImportados.Add(perfil);
            }

            if (perfisImportados.Any())
            {
                await _context.Perfis.AddRangeAsync(perfisImportados);
                await _context.SaveChangesAsync();
                TempData["Success"] = $"{perfisImportados.Count} perfis importados com sucesso!";
            }

            if (erros.Any())
            {
                TempData["ErroImportacao"] = string.Join("<br/>", erros);
            }

            return RedirectToAction(nameof(Index));
        }



        private bool PerfilExists(int id) => _context.Perfis.Any(e => e.Id == id);
    }
}
