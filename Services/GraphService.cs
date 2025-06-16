using Gerenciador_de_Produtos.Data;
using Microsoft.EntityFrameworkCore;

namespace Gerenciador_de_Produtos.Services
{
    public class GraphService
    {
        private readonly ApplicationDbContext _context;

        public GraphService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<(List<object> nodes, List<object> edges)> GetGraphDataAsync()
        {
            var nodes = new List<object>();
            var edges = new List<object>();

            // Carregar entidades
            var itensERP = await _context.ItensERP.AsNoTracking().ToListAsync().ConfigureAwait(false);
            var componentes = await _context.Componentes.AsNoTracking().ToListAsync().ConfigureAwait(false);
            var agrupadores = await _context.Agrupadores.AsNoTracking().ToListAsync().ConfigureAwait(false);
            var produtos = await _context.Produtos.AsNoTracking().ToListAsync().ConfigureAwait(false);
            var perfis = await _context.Perfis.AsNoTracking().ToListAsync().ConfigureAwait(false);
            var desenhos = await _context.Desenhos.AsNoTracking().ToListAsync().ConfigureAwait(false);
            var relacoes = await _context.ItemERPRelacionados
                .AsNoTracking()
                .Include(r => r.Relacionado)
                .Include(r => r.ItemERP)
                .ToListAsync().ConfigureAwait(false);

            // Nós
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

            // Arestas
            var aprs = await _context.ProdutoAgrupadores
                .AsNoTracking()
                .Include(pa => pa.Agrupador)
                .Include(pa => pa.Produto)
                .ToListAsync().ConfigureAwait(false);

            edges.AddRange(aprs.Select(ap => new {
                from = $"agr-{ap.Agrupador.Id}",
                to = $"prd-{ap.Produto.Id}"
            }));

            var agrComps = await _context.AgrupadorComponentes
                .AsNoTracking()
                .Include(ac => ac.Agrupador)
                .Include(ac => ac.Componente)
                .ToListAsync().ConfigureAwait(false);

            edges.AddRange(agrComps.Select(ac => new {
                from = $"cmp-{ac.Componente.Id}",
                to = $"agr-{ac.Agrupador.Id}"
            }));

            var compItens = await _context.ComponenteItemERPs
                .AsNoTracking()
                .Include(ci => ci.Componente)
                .Include(ci => ci.ItemERP)
                .ToListAsync().ConfigureAwait(false);

            edges.AddRange(compItens.Select(ci => new {
                from = $"item-{ci.ItemERP.Id}",
                to = $"cmp-{ci.Componente.Id}"
            }));

            edges.AddRange(relacoes.Select(rel => new {
                from = $"item-{rel.ItemERPId}",
                to = $"item-{rel.RelacionadoId}",
                label = "relacionado"
            }));

            var piers = await _context.PerfilItemERPs
                .AsNoTracking()
                .Include(pi => pi.ItemERP)
                .Include(pi => pi.Perfil)
                .ToListAsync().ConfigureAwait(false);

            edges.AddRange(piers.Select(pi => new {
                from = $"item-{pi.ItemERP.Id}",
                to = $"pf-{pi.Perfil.Id}"
            }));

            var dieLinks = await _context.DesenhoItemERPs
                .AsNoTracking()
                .Include(die => die.Desenho)
                .Include(die => die.ItemERP)
                .ToListAsync().ConfigureAwait(false);

            edges.AddRange(dieLinks.Select(die => new {
                from = $"item-{die.ItemERP.Id}",
                to = $"des-{die.Desenho.DesenhoId}"
            }));

            return (nodes, edges);
        }
    }
}
