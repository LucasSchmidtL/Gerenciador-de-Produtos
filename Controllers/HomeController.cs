using System.Diagnostics;
using System.Linq;
using Gerenciador_de_Produtos.Data;
using Gerenciador_de_Produtos.Models;
using Gerenciador_de_Produtos.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gerenciador_de_Produtos.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;
        private readonly GraphService _graphService;

        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger, GraphService graphService)
        {
            _context = context;
            _logger = logger;
            _graphService = graphService;
        }

        public IActionResult Index() => View();

        // -----------------------------
        //       GRAFO COMPLETO
        // -----------------------------
        [HttpGet]
        public async Task<JsonResult> GetGraph()
        {
            var (nodes, edges) = await _graphService.GetGraphDataAsync();
            return Json(new { nodes, edges });
        }

        // -----------------------------
        //         PROGRESSIVO
        // -----------------------------



        [HttpGet]
        public async Task<JsonResult> GetGraphProduto(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);
            if (produto == null) return Json(new { nodes = new object[0], edges = new object[0] });

            var node = new { id = $"prd-{produto.Id}", label = produto.NomeComercial, group = "produto" };

            var edges = await _context.ProdutoAgrupadores
                .Where(x => x.ProdutoId == id)
                .Select(x => new { from = $"agr-{x.AgrupadorId}", to = $"prd-{id}" })
                .ToListAsync();

            var agrupadores = await _context.ProdutoAgrupadores
                .Where(x => x.ProdutoId == id)
                .Include(x => x.Agrupador)
                .Select(x => new { id = $"agr-{x.Agrupador.Id}", label = x.Agrupador.Nome, group = "agrupador" })
                .ToListAsync();

            return Json(new { nodes = new[] { node }.Concat(agrupadores), edges });
        }

        [HttpGet]
        public async Task<JsonResult> GetGraphAgrupador(int id)
        {
            var comps = await _context.AgrupadorComponentes
                .Where(x => x.AgrupadorId == id)
                .Include(x => x.Componente)
                .ToListAsync();

            var nodes = comps.Select(c => new { id = $"cmp-{c.Componente.Id}", label = c.Componente.Nome, group = "componente" }).ToList();
            var edges = comps.Select(c => new { from = $"cmp-{c.Componente.Id}", to = $"agr-{id}" }).ToList();

            return Json(new { nodes, edges });
        }

        [HttpGet]
        public async Task<JsonResult> GetGraphComponente(int id)
        {
            var items = await _context.ComponenteItemERPs
                .Where(x => x.ComponenteId == id)
                .Include(x => x.ItemERP)
                .ToListAsync();

            var nodes = items.Select(i => new { id = $"item-{i.ItemERP.Id}", label = i.ItemERP.ERP, group = "item" }).ToList();
            var edges = items.Select(i => new { from = $"item-{i.ItemERP.Id}", to = $"cmp-{id}" }).ToList();

            return Json(new { nodes, edges });
        }


        public class NodeEdgeTemp
        {
            public string Id { get; set; }
            public string Label { get; set; }
            public string Group { get; set; }
            public string EdgeFrom { get; set; }
            public string EdgeTo { get; set; }
        }


        [HttpGet]
        public async Task<JsonResult> GetGraphItem(int id)
        {
            var item = await _context.ItensERP
                .Where(i => i.Id == id)
                .Select(i => new
                {
                    Perfis = i.PerfilItemERPs.Select(p => new NodeEdgeTemp
                    {
                        Id = $"pf-{p.Perfil.Id}",
                        Label = p.Perfil.Descricao,
                        Group = "perfil",
                        EdgeFrom = $"item-{id}",
                        EdgeTo = $"pf-{p.Perfil.Id}"
                    }).ToList(),

                    Desenhos = i.DesenhoItemERPs.Select(d => new NodeEdgeTemp
                    {
                        Id = $"des-{d.Desenho.DesenhoId}",
                        Label = d.Desenho.Nome,
                        Group = "desenho",
                        EdgeFrom = $"item-{id}",
                        EdgeTo = $"des-{d.Desenho.DesenhoId}"
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            var elementos = item?.Perfis.Concat(item.Desenhos) ?? Enumerable.Empty<NodeEdgeTemp>();

            var nodes = elementos
                .Select(x => new { id = x.Id, label = x.Label, group = x.Group })
                .Distinct()
                .ToList();

            var edges = elementos
                .Select(x => new { from = x.EdgeFrom, to = x.EdgeTo })
                .ToList();

            return Json(new { nodes, edges });
        }




        [HttpGet]
        public async Task<JsonResult> GetProdutos()
        {
            var produtos = await _context.Produtos
                .Select(p => new { id = p.Id, nome = p.NomeComercial })
                .ToListAsync();
            return Json(produtos);
        }

        [HttpGet]
        public async Task<JsonResult> GetGraphProdutosIniciais()
        {
            var produtos = await _context.Produtos
                .AsNoTracking()
                .Select(p => new {
                    id = $"prd-{p.Id}",
                    label = p.NomeComercial,
                    group = "produto"
                })
                .ToListAsync();

            return Json(new { nodes = produtos, edges = new object[0] });
        }



        [HttpGet]
        public async Task<JsonResult> GetAgrupadoresPorProduto(int produtoId)
        {
            var agrupadores = await _context.ProdutoAgrupadores
                .Where(pa => pa.ProdutoId == produtoId)
                .Include(pa => pa.Agrupador)
                .Select(pa => new { id = pa.Agrupador.Id, nome = pa.Agrupador.Nome })
                .Distinct()
                .ToListAsync();
            return Json(agrupadores);
        }

        [HttpGet]
        public async Task<JsonResult> GetComponentesPorAgrupador(int agrupadorId)
        {
            var componentes = await _context.AgrupadorComponentes
                .Where(ac => ac.AgrupadorId == agrupadorId)
                .Include(ac => ac.Componente)
                .Select(ac => new { id = ac.Componente.Id, nome = ac.Componente.Nome })
                .Distinct()
                .ToListAsync();
            return Json(componentes);
        }

        [HttpGet]
        public async Task<JsonResult> GetItensPorComponente(int componenteId)
        {
            var itens = await _context.ComponenteItemERPs
                .Where(ci => ci.ComponenteId == componenteId)
                .Include(ci => ci.ItemERP)
                .Select(ci => new {
                    id = ci.ItemERP.Id,
                    erp = ci.ItemERP.ERP,
                    descricao = ci.ItemERP.Descricao
                })
                .Distinct()
                .ToListAsync();
            return Json(itens);
        }

        [HttpGet]
        public async Task<JsonResult> GetDetalhesItemERP(int itemId)
        {
            var detalhes = await _context.ItensERP
                .Where(i => i.Id == itemId)
                .Select(i => new {
                    i.Id,
                    i.ERP,
                    i.Descricao,
                    perfis = i.PerfilItemERPs.Select(p => p.Perfil.Desenho),
                    desenhos = i.DesenhoItemERPs.Select(d => d.Desenho.Nome)
                })
                .FirstOrDefaultAsync();

            return Json(detalhes);
        }


        [HttpPost]
        public IActionResult SalvarTema(string corHex, string tema)
        {
            if (!string.IsNullOrEmpty(corHex))
                HttpContext.Session.SetString("SidebarColor", corHex);

            if (!string.IsNullOrEmpty(tema))
                HttpContext.Session.SetString("TemaSistema", tema);

            return Redirect(Request.Headers["Referer"].ToString());
        }



        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() =>
            View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
