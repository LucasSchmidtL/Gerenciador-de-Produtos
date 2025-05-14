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

        public async Task<IActionResult> Index()
        {
            // KPIs
            ViewBag.TotalItensERP = await _context.ItensERP.CountAsync();
            ViewBag.TotalTags = await _context.Tags.CountAsync();
            ViewBag.TotalAgrupadores = await _context.Agrupadores.CountAsync();
            ViewBag.TotalPerfis = await _context.Perfis.CountAsync();
            ViewBag.TotalDesenhos = await _context.Desenhos.CountAsync();

            // Status chart data (Status == "Ativo" considered active)
            var ativos = await _context.ItensERP.CountAsync(i => i.Status == "Ativo");
            var inativos = ViewBag.TotalItensERP - ativos;
            ViewBag.ActiveCount = ativos;
            ViewBag.InactiveCount = inativos;

            // Recent logs removed: implement Logs DbSet and entity before using this section.
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
