using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using Gerenciador_de_Produtos.Data;
using Gerenciador_de_Produtos.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Gerenciador_de_Produtos.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: /
        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetGraph()
        {
            // 1) Carrega as entidades
            var itensERP = await _context.ItensERP.ToListAsync();
            var componentes = await _context.Componentes.ToListAsync();
            var agrupadores = await _context.Agrupadores.ToListAsync();
            var produtos = await _context.Produtos.ToListAsync();
            var perfis = await _context.Perfis.ToListAsync();
            var desenhos = await _context.Desenhos.ToListAsync();
            var relacoes = await _context.ItemERPRelacionados
                .Include(r => r.Desenho)
                .Include(r => r.Relacionado)
                .Include(r => r.ItemERP)
                .ToListAsync();


            var nodes = new List<object>();
            var edges = new List<object>();

            // 2) Monta os nós
            nodes.AddRange(itensERP.Select(i => new {
                id = $"item-{i.Id}",
                label = i.ERP,
                group = "item"
            }));
            nodes.AddRange(componentes.Select(c => new {
                id = $"cmp-{c.Id}",
                label = c.Nome,
                group = "componente"
            }));
            nodes.AddRange(agrupadores.Select(a => new {
                id = $"agr-{a.Id}",
                label = a.Nome,
                group = "agrupador"
            }));
            nodes.AddRange(produtos.Select(p => new {
                id = $"prd-{p.Id}",
                label = p.NomeComercial,
                group = "produto"
            }));
            nodes.AddRange(perfis.Select(pf => new {
                id = $"pf-{pf.Id}",
                label = pf.Desenho,
                group = "perfil"
            }));
            nodes.AddRange(desenhos.Select(d => new {
                id = $"des-{d.DesenhoId}",
                label = d.Nome,
                group = "desenho"
            }));

            // 3) Monta as arestas existentes

            // 3.1) ItemERP - Componente
            var ciers = await _context.ComponenteItemERPs
                                      .Include(ci => ci.ItemERP)
                                      .Include(ci => ci.Componente)
                                      .ToListAsync();
            foreach (var ci in ciers)
            {
                edges.Add(new
                {
                    from = $"item-{ci.ItemERP.Id}",
                    to = $"cmp-{ci.Componente.Id}"
                });
            }

            // 3.2) ItemERP - Agrupador
            var aiers = await _context.AgrupadorItemERPs
                                      .Include(ai => ai.ItemERP)
                                      .Include(ai => ai.Agrupador)
                                      .ToListAsync();
            foreach (var ai in aiers)
            {
                edges.Add(new
                {
                    from = $"item-{ai.ItemERP.Id}",
                    to = $"agr-{ai.Agrupador.Id}"
                });
            }

            // 3.3) Agrupador - Produto
            var aprs = await _context.ProdutoAgrupadores
                                    .Include(pa => pa.Agrupador)
                                    .Include(pa => pa.Produto)
                                    .ToListAsync();
            foreach (var ap in aprs)
            {
                edges.Add(new
                {
                    from = $"agr-{ap.Agrupador.Id}",
                    to = $"prd-{ap.Produto.Id}"
                });
            }

            // 3.4) ItemERPRelacionado (pai - relacionado)
            foreach (var rel in relacoes)
            {
                edges.Add(new
                {
                    from = $"item-{rel.ItemERPId}",
                    to = $"item-{rel.RelacionadoId}",
                    label = "relacionado"
                });
            }

            // 3.5) ItemERP - Perfil
            var piers = await _context.PerfilItemERPs
                                      .Include(pi => pi.ItemERP)
                                      .Include(pi => pi.Perfil)
                                      .ToListAsync();
            foreach (var pi in piers)
            {
                edges.Add(new
                {
                    from = $"item-{pi.ItemERP.Id}",
                    to = $"pf-{pi.Perfil.Id}"
                });
            }

            // 3.6) ItemERP - Desenho
            var desenhoItemERPs = await _context.DesenhoItemERPs
                .Include(die => die.Desenho)
                .Include(die => die.ItemERP)
                .ToListAsync();

            foreach (var die in desenhoItemERPs)
            {
                edges.Add(new
                {
                    from = $"item-{die.ItemERP.Id}",
                    to = $"des-{die.Desenho.DesenhoId}"
                });
            }
        

        // 3.7) Agrupador - Componente
        var agrComps = await _context.AgrupadorComponentes
                                         .Include(ac => ac.Componente)
                                         .Include(ac => ac.Agrupador)
                                         .ToListAsync();
            foreach (var ac in agrComps)
            {
                edges.Add(new
                {
                    from = $"cmp-{ac.Componente.Id}",
                    to = $"agr-{ac.Agrupador.Id}"
                });
            }

            return Json(new { nodes, edges });
        }

        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() =>
            View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
