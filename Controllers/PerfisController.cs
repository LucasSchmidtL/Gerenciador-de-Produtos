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

        public async Task<IActionResult> Index()
        {
            var perfis = await _context.Perfis
                .Include(p => p.PerfilItemERPs).ThenInclude(pi => pi.ItemERP)
                .Include(p => p.DesenhosPerfil).ThenInclude(pd => pd.Desenho)
                .ToListAsync();

            var allErps = await _context.ItensERP
                .Select(i => new SelectListItem
                {
                    Value = i.Id.ToString(),
                    Text = $"{i.ERP}|{i.Descricao}|{i.TipoItem}"
                }).ToListAsync();

            var allDesenhos = await _context.Desenhos
                .Select(d => new SelectListItem
                {
                    Value = d.DesenhoId.ToString(),
                    Text = $"{d.Nome} | Rev. {d.Revisao} | {d.Descricao}"
                }).ToListAsync();

            var vm = perfis.Select(p => new PerfilItemERPViewModel
            {
                Id = p.Id,
                Descricao = p.Descricao,
                TipoSecao = p.TipoSecao,
                TodosItensERP = allErps,
                TodosDesenhos = allDesenhos,
                ItensERPSelecionados = p.PerfilItemERPs.Select(pi => pi.ItemERPId).ToList(),
                DesenhosSelecionados = p.DesenhosPerfil.Select(pd => pd.DesenhoId).ToList()
            }).ToList();

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AtualizaItens(PerfilItemERPViewModel vm)
        {
            if (!vm.Id.HasValue) return BadRequest();

            var perfil = await _context.Perfis
                .Include(p => p.PerfilItemERPs)
                .Include(p => p.DesenhosPerfil)
                .FirstOrDefaultAsync(p => p.Id == vm.Id.Value);

            if (perfil == null) return NotFound();

            _context.PerfilItemERPs.RemoveRange(perfil.PerfilItemERPs);
            _context.Entry(perfil).Collection(p => p.DesenhosPerfil).CurrentValue = new List<DesenhoPerfil>();

            perfil.PerfilItemERPs = vm.ItensERPSelecionados.Distinct()
                .Select(id => new PerfilItemERP { PerfilId = perfil.Id, ItemERPId = id }).ToList();

            if (vm.DesenhosSelecionados != null)
            {
                perfil.DesenhosPerfil = vm.DesenhosSelecionados.Distinct()
                    .Select(did => new DesenhoPerfil { PerfilId = perfil.Id, DesenhoId = did }).ToList();
            }

            await _context.SaveChangesAsync();
            TempData["Success"] = "Itens ERP e Desenhos atualizados!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var perfil = await _context.Perfis
                .Include(p => p.PerfilItemERPs).ThenInclude(pi => pi.ItemERP)
                .Include(p => p.DesenhosPerfil).ThenInclude(pd => pd.Desenho)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (perfil == null) return NotFound();
            return View(perfil);
        }

        public async Task<IActionResult> Create()
        {
            var vm = new PerfilItemERPViewModel
            {
                TodosItensERP = await _context.ItensERP.Select(i => new SelectListItem
                {
                    Value = i.Id.ToString(),
                    Text = $"{i.ERP} – {i.Descricao}"
                }).ToListAsync(),

                TodosDesenhos = await _context.Desenhos.Select(d => new SelectListItem
                {
                    Value = d.DesenhoId.ToString(),
                    Text = $"{d.Nome} | Rev. {d.Revisao} | {d.Descricao}"
                }).ToListAsync()
            };

            ViewBag.AllTags = await _context.Tags.Select(t => new SelectListItem
            {
                Value = t.Nome,
                Text = t.Nome
            }).ToListAsync();

            ViewBag.ItensSelecionados = vm.ItensERPSelecionados != null
                ? await _context.ItensERP
                    .Where(i => vm.ItensERPSelecionados.Contains(i.Id))
                    .Select(i => new SelectListItem
                    {
                        Value = i.Id.ToString(),
                        Text = $"{i.ERP} | {i.Descricao} | {i.TipoItem}"
                    }).ToListAsync()
                : new List<SelectListItem>();


            ViewBag.DesenhosSelecionados = vm.DesenhosSelecionados != null
                ? await _context.Desenhos
                    .Where(d => vm.DesenhosSelecionados.Contains(d.DesenhoId))
                    .Select(d => new SelectListItem
                    {
                        Value = d.DesenhoId.ToString(),
                        Text = $"{d.Nome} | Rev. {d.Revisao} | {d.Descricao}"
                    }).ToListAsync()
                : new List<SelectListItem>();


            ViewBag.ClassificacoesDesenho = await _context.Desenhos
                .Where(d => d.Classificacao != null)
                .Select(d => d.Classificacao)
                .Distinct()
                .OrderBy(c => c)
                .Select(c => new SelectListItem { Value = c, Text = c })
                .ToListAsync();

            var revisoes = await _context.Desenhos
                .Where(d => d.Revisao != null)
                .Select(d => d.Revisao)
                .Distinct()
                .OrderBy(r => r)
                .ToListAsync();

            ViewBag.RevisoesDesenho = revisoes.Select(r => new SelectListItem
            {
                Value = r.ToString(),
                Text = r.ToString()
            }).ToList();

            ViewBag.AllTags = await _context.Tags.Select(t => new SelectListItem
            {
                Value = t.Nome,
                Text = t.Nome
            }).ToListAsync();

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PerfilItemERPViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                await PreencherViewBagsAsync(vm);
                return View(vm);
            }

            var perfil = new Perfil
            {
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
                PerfilItemERPs = vm.ItensERPSelecionados?.Select(id => new PerfilItemERP { ItemERPId = id }).ToList() ?? new List<PerfilItemERP>(),
                DesenhosPerfil = vm.DesenhosSelecionados?.Select(did => new DesenhoPerfil { DesenhoId = did }).ToList() ?? new List<DesenhoPerfil>()
            };

            _context.Perfis.Add(perfil);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task PreencherViewBagsAsync(PerfilItemERPViewModel vm)
        {
            vm.TodosItensERP = await _context.ItensERP.Select(i => new SelectListItem
            {
                Value = i.Id.ToString(),
                Text = $"{i.ERP} – {i.Descricao}"
            }).ToListAsync();

            vm.TodosDesenhos = await _context.Desenhos.Select(d => new SelectListItem
            {
                Value = d.DesenhoId.ToString(),
                Text = $"{d.Nome} | Rev. {d.Revisao} | {d.Descricao}"
            }).ToListAsync();

            ViewBag.ItensSelecionados = vm.ItensERPSelecionados != null
                ? await _context.ItensERP
                    .Where(i => vm.ItensERPSelecionados.Contains(i.Id))
                    .Select(i => new SelectListItem
                    {
                        Value = i.Id.ToString(),
                        Text = $"{i.ERP} | {i.Descricao} | {i.TipoItem}"
                    }).ToListAsync()
                : new List<SelectListItem>();


            ViewBag.DesenhosSelecionados = vm.DesenhosSelecionados != null
                ? await _context.Desenhos
                    .Where(d => vm.DesenhosSelecionados.Contains(d.DesenhoId))
                    .Select(d => new SelectListItem
                    {
                        Value = d.DesenhoId.ToString(),
                        Text = $"{d.Nome} | Rev. {d.Revisao} | {d.Descricao}"
                    }).ToListAsync()
                : new List<SelectListItem>();


            ViewBag.ClassificacoesDesenho = await _context.Desenhos
                .Where(d => d.Classificacao != null)
                .Select(d => d.Classificacao)
                .Distinct()
                .OrderBy(c => c)
                .Select(c => new SelectListItem { Value = c, Text = c })
                .ToListAsync();

            var revisoes = await _context.Desenhos
                .Where(d => d.Revisao != null)
                .Select(d => d.Revisao)
                .Distinct()
                .OrderBy(r => r)
                .ToListAsync();

            ViewBag.RevisoesDesenho = revisoes.Select(r => new SelectListItem
            {
                Value = r.ToString(),
                Text = r.ToString()
            }).ToList();

            ViewBag.AllTags = await _context.Tags.Select(t => new SelectListItem
            {
                Value = t.Nome,
                Text = t.Nome
            }).ToListAsync();
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var perfil = await _context.Perfis
                .Include(p => p.PerfilItemERPs)
                .Include(p => p.DesenhosPerfil).ThenInclude(pd => pd.Desenho)

                .FirstOrDefaultAsync(p => p.Id == id);
            if (perfil == null) return NotFound();

            var vm = new PerfilItemERPViewModel
            {
                Id = perfil.Id,
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
                DesenhosSelecionados = perfil.DesenhosPerfil.Select(pd => pd.DesenhoId).ToList(),
                TodosItensERP = await _context.ItensERP.Select(i => new SelectListItem
                {
                    Value = i.Id.ToString(),
                    Text = $"{i.ERP} – {i.Descricao}"
                }).ToListAsync(),
                TodosDesenhos = await _context.Desenhos.Select(d => new SelectListItem
                {
                    Value = d.DesenhoId.ToString(),
                    Text = $"{d.Nome} | Rev. {d.Revisao} | {d.Descricao}"
                }).ToListAsync()
            };

            ViewBag.ItensSelecionados = await _context.ItensERP
                .Where(i => vm.ItensERPSelecionados.Contains(i.Id))
                .Select(i => new SelectListItem
                {
                    Value = i.Id.ToString(),
                    Text = $"{i.ERP} | {i.Descricao} | {i.TipoItem}"
                }).ToListAsync();

            ViewBag.DesenhosSelecionados = await _context.Desenhos
                .Where(d => vm.DesenhosSelecionados.Contains(d.DesenhoId))
                .Select(d => new SelectListItem
                {
                    Value = d.DesenhoId.ToString(),
                    Text = $"{d.Nome} | Rev. {d.Revisao} | {d.Descricao}"
                }).ToListAsync();

            ViewBag.ClassificacoesDesenho = await _context.Desenhos
                .Select(d => d.Classificacao)
                .Where(c => c != null)
                .Distinct()
                .OrderBy(c => c)
                .Select(c => new SelectListItem { Value = c, Text = c })
                .ToListAsync();
           
            
            var revisoes = await _context.Desenhos
                .Select(d => d.Revisao)
                .Where(r => r != null)
                .Distinct()
                .OrderBy(r => r)
                .ToListAsync();

            ViewBag.RevisoesDesenho = revisoes
                .Select(r => new SelectListItem
                {
                    Value = r.ToString(),
                    Text = r.ToString()
                })
                .ToList();



            ViewBag.AllTags = await _context.Tags
                .Select(t => new SelectListItem
                {
                    Value = t.Nome,
                    Text = t.Nome
                }).ToListAsync();


            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PerfilItemERPViewModel vm)
        {
            if (!vm.Id.HasValue) return BadRequest();
            if (!ModelState.IsValid)
            {
                vm.TodosItensERP = await _context.ItensERP.Select(i => new SelectListItem
                {
                    Value = i.Id.ToString(),
                    Text = $"{i.ERP} – {i.Descricao}"
                }).ToListAsync();
                vm.TodosDesenhos = await _context.Desenhos.Select(d => new SelectListItem
                {
                    Value = d.DesenhoId.ToString(),
                    Text = $"{d.Nome} | Rev. {d.Revisao} | {d.Descricao}"
                }).ToListAsync();
                return View(vm);
            }

            var perfil = await _context.Perfis
                .Include(p => p.PerfilItemERPs)
                .Include(p => p.DesenhosPerfil).ThenInclude(pd => pd.Desenho)

                .FirstOrDefaultAsync(p => p.Id == vm.Id.Value);
            if (perfil == null) return NotFound();

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
            perfil.PerfilItemERPs = vm.ItensERPSelecionados.Select(id => new PerfilItemERP
            {
                PerfilId = perfil.Id,
                ItemERPId = id
            }).ToList();

            if (vm.DesenhosSelecionados != null)
            {
                var desenhos = await _context.Desenhos
                    .Where(d => vm.DesenhosSelecionados.Contains(d.DesenhoId)).ToListAsync();
                perfil.DesenhosPerfil = desenhos
                    .Select(d => new DesenhoPerfil { PerfilId = perfil.Id, DesenhoId = d.DesenhoId })
                    .ToList();

            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Perfis/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var perfil = await _context.Perfis
                .Include(p => p.PerfilItemERPs).ThenInclude(pi => pi.ItemERP)
                .Include(p => p.DesenhosPerfil).ThenInclude(pd => pd.Desenho)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (perfil == null) return NotFound();
            return View(perfil);
        }

        // POST: Perfis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var perfil = await _context.Perfis
                .Include(p => p.PerfilItemERPs)
                .Include(p => p.DesenhosPerfil)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (perfil == null) return NotFound();

            // Remove os vínculos primeiro
            _context.PerfilItemERPs.RemoveRange(perfil.PerfilItemERPs);
            _context.RemoveRange(perfil.DesenhosPerfil);


            // Depois remove o perfil
            _context.Perfis.Remove(perfil);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }



        // GET: Perfis/Import
        public IActionResult Import() => View();

        // GET: Perfis/DownloadTemplate
        public FileResult DownloadTemplate()
        {
            var header = string.Join(",", new[] {
                "ItensERP", "Desenhos", // novos campos
                "Descricao", "TipoSecao", "Peso", "AreaBruta", "AreaLiq", "AreaEq", "Ix", "Sxt", "Sxb", "Zx", "Rx", "yt", "yb",
                "Ixy", "Iy", "Syl", "Syr", "Zy", "ry", "xl", "xr", "xo", "yo", "jx", "jy", "Cw", "J", "Ixe",
                "Sxet", "Sxeb", "lye", "Syel", "Syer", "p1", "p2", "p3", "SimetricoX", "SimetricoY"
            });

            var exemplo = string.Join(",", new[] {
                "\"ERP123;ERP456\"",  // exemplo de múltiplos ERPs
                "\"DES001;DES002\"",  // exemplo de múltiplos desenhos
                "Perfil Exemplo", "SecaoA", "12.5", "100", "95", "90", "200", "30", "25", "50", "5", "10", "8", "2", "150", "20", "15",
                "75", "8.7", "3.2", "1.5", "0.5", "0.5", "0.8", "0.9", "0.2", "0.1", "180", "25", "20", "5", "3", "1", "0.5", "0.3", "0.2", "TRUE", "FALSE"
             });

            var csvLines = new List<string> { header, exemplo };
            var bytes = System.Text.Encoding.UTF8.GetBytes(string.Join("\r\n", csvLines));
            return File(bytes, "text/csv", "template_perfis.csv");
        }


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
            var rows = new List<dynamic>();
            csv.Read();
            csv.ReadHeader();

            while (csv.Read())
            {
                var row = csv.GetRecord<dynamic>();
                rows.Add(row);
            }

            int importados = 0;


            float? ParseFloat(string value) => double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var result) ? (float?)result : null;

            foreach (var row in rows)
            {
                var dict = row as IDictionary<string, object>;
                if (dict == null) continue; // pular linha inválida


                var perfil = new Perfil
                {
                    Descricao = dict["Descricao"]?.ToString(),
                    TipoSecao = dict["TipoSecao"]?.ToString(),
                    Peso = ParseFloat(dict["Peso"]?.ToString()),
                    AreaBruta = ParseFloat(dict["AreaBruta"]?.ToString()),
                    AreaLiq = ParseFloat(dict["AreaLiq"]?.ToString()),
                    AreaEq = ParseFloat(dict["AreaEq"]?.ToString()),
                    Ix = ParseFloat(dict["Ix"]?.ToString()),
                    Sxt = ParseFloat(dict["Sxt"]?.ToString()),
                    Sxb = ParseFloat(dict["Sxb"]?.ToString()),
                    Zx = ParseFloat(dict["Zx"]?.ToString()),
                    Rx = ParseFloat(dict["Rx"]?.ToString()),
                    yt = ParseFloat(dict["yt"]?.ToString()),
                    yb = ParseFloat(dict["yb"]?.ToString()),
                    Ixy = ParseFloat(dict["Ixy"]?.ToString()),
                    Iy = ParseFloat(dict["Iy"]?.ToString()),
                    Syl = ParseFloat(dict["Syl"]?.ToString()),
                    Syr = ParseFloat(dict["Syr"]?.ToString()),
                    Zy = ParseFloat(dict["Zy"]?.ToString()),
                    ry = ParseFloat(dict["ry"]?.ToString()),
                    xl = ParseFloat(dict["xl"]?.ToString()),
                    xr = ParseFloat(dict["xr"]?.ToString()),
                    xo = ParseFloat(dict["xo"]?.ToString()),
                    yo = ParseFloat(dict["yo"]?.ToString()),
                    jx = ParseFloat(dict["jx"]?.ToString()),
                    jy = ParseFloat(dict["jy"]?.ToString()),
                    Cw = ParseFloat(dict["Cw"]?.ToString()),
                    J = ParseFloat(dict["J"]?.ToString()),
                    Ixe = ParseFloat(dict["Ixe"]?.ToString()),
                    Sxet = ParseFloat(dict["Sxet"]?.ToString()),
                    Sxeb = ParseFloat(dict["Sxeb"]?.ToString()),
                    lye = ParseFloat(dict["lye"]?.ToString()),
                    Syel = ParseFloat(dict["Syel"]?.ToString()),
                    Syer = ParseFloat(dict["Syer"]?.ToString()),
                    p1 = ParseFloat(dict["p1"]?.ToString()),
                    p2 = ParseFloat(dict["p2"]?.ToString()),
                    p3 = ParseFloat(dict["p3"]?.ToString()),
                    SimetricoX = bool.TryParse(dict["SimetricoX"]?.ToString(), out var simx) ? simx : (bool?)null,
                    SimetricoY = bool.TryParse(dict["SimetricoY"]?.ToString(), out var simy) ? simy : (bool?)null
                };

                // ✅ ISSO TEM QUE ESTAR AQUI DENTRO
                var itensRaw = dict["ItensERP"]?.ToString();
                if (!string.IsNullOrWhiteSpace(itensRaw))
                {
                    var erps = itensRaw.Split(';', StringSplitOptions.RemoveEmptyEntries);
                    foreach (var erp in erps)
                    {
                        var item = await _context.ItensERP.FirstOrDefaultAsync(i => i.ERP == erp.Trim());
                        if (item != null)
                        {
                            if (!perfil.PerfilItemERPs.Any(pi => pi.ItemERPId == item.Id))
                            {
                                perfil.PerfilItemERPs.Add(new PerfilItemERP { ItemERPId = item.Id });
                            }

                        }
                    }
                }

                var desenhosRaw = dict["Desenhos"]?.ToString();
                if (!string.IsNullOrWhiteSpace(desenhosRaw))
                {
                    var nomes = desenhosRaw.Split(';', StringSplitOptions.RemoveEmptyEntries);
                    foreach (var nome in nomes)
                    {
                        var desenho = await _context.Desenhos.FirstOrDefaultAsync(d => d.Nome == nome.Trim());
                        if (desenho != null)
                        {
                            if (!perfil.DesenhosPerfil.Any(dp => dp.DesenhoId == desenho.DesenhoId))
                            {
                                perfil.DesenhosPerfil.Add(new DesenhoPerfil { DesenhoId = desenho.DesenhoId });
                            }

                        }
                    }
                }

                _context.Perfis.Add(perfil);
                importados++;
            }

            await _context.SaveChangesAsync();
            TempData["Success"] = $"{importados} perfis importados com sucesso!";
            return RedirectToAction(nameof(Index));
        }



        private bool PerfilExists(int id)
            => _context.Perfis.Any(e => e.Id == id);
    }
}