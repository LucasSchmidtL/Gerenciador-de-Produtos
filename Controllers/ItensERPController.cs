using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Gerenciador_de_Produtos.Models.Enums;
using Microsoft.Extensions.Logging;
using Gerenciador_de_Produtos.Data;
using Gerenciador_de_Produtos.Models;
using Gerenciador_de_Produtos.Models.ViewModels;
using Gerenciador_de_Produtos.Services;
using Gerenciador_de_Produtos.Models.DTOs;

namespace Gerenciador_de_Produtos.Controllers
{
    public class ItensERPController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ItensERPController> _logger;
        

        public ItensERPController(ApplicationDbContext context, ILogger<ItensERPController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: ItensERP
        public async Task<IActionResult> Index()
        {

            ViewBag.AllTags = await _context.Tags
                  .Select(t => new SelectListItem
                  {
                      Value = t.Id.ToString(),
                      Text = t.Nome
                  }).ToListAsync();


            var itens = await _context.ItensERP
                .Include(i => i.Tags)
                .Include(i => i.AgrupadorItensERP).ThenInclude(ai => ai.Agrupador)
                .Include(i => i.DesenhoItemERPs).ThenInclude(di => di.Desenho)
                .Include(i => i.Revisoes)
                .Include(i => i.PerfilItemERPs).ThenInclude(pi => pi.Revisoes)
                .Include(i => i.PerfilItemERPs).ThenInclude(pi => pi.Perfil)
                .ToListAsync();




            return View(itens);
        }

        // GET: AJAX
        [HttpGet]
        public async Task<IActionResult> BuscarItensERP(string term, string tipoItem, string status)
        {
            var query = _context.ItensERP.AsQueryable();

            if (!string.IsNullOrWhiteSpace(term))
                query = query.Where(i => i.ERP.Contains(term) || i.Descricao.Contains(term));

            if (!string.IsNullOrWhiteSpace(tipoItem) && Enum.TryParse<TipoItem>(tipoItem, out var tipoEnum))
                query = query.Where(i => i.TipoItem == tipoEnum);

            if (!string.IsNullOrWhiteSpace(status) && Enum.TryParse<StatusItemERP>(status, out var statusEnum))
                query = query.Where(i => i.Status == statusEnum);


            var resultados = await query
                .OrderBy(i => i.ERP)
                .Select(i => new
                {
                    id = i.Id,
                    text = $"{i.ERP}|{i.Descricao}|{i.Status}"
                })
                .ToListAsync();

            return Json(resultados);
        }

        // Novo método para carregar resultados filtrados via AJAX na index dinâmica
        [HttpGet]
        public async Task<IActionResult> Filtrar(string? tipo, string? status, string? tag, string? termo)
        {
            var query = _context.ItensERP
                .Include(i => i.Tags)
                .Include(i => i.Revisoes)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(tipo) && Enum.TryParse<TipoItem>(tipo, out var tipoEnum))
                query = query.Where(i => i.TipoItem == tipoEnum);

            if (!string.IsNullOrWhiteSpace(status) && Enum.TryParse<StatusItemERP>(status, out var statusEnum))
                query = query.Where(i => i.Status == statusEnum);

            if (!string.IsNullOrWhiteSpace(tag))
                query = query.Where(i => i.Tags.Any(t => t.Id.ToString() == tag));

            if (!string.IsNullOrWhiteSpace(termo))
                query = query.Where(i => i.ERP.Contains(termo) || i.Descricao.Contains(termo));

            var itens = await query.OrderByDescending(i => i.DataCriacao).Take(50).ToListAsync();

            return PartialView("_ListaItensERP", itens);
        }


        // GET: ItensERP/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var item = await _context.ItensERP
                .Include(i => i.Tags)
                .Include(i => i.AgrupadorItensERP).ThenInclude(ai => ai.Agrupador)
                .Include(i => i.DesenhoItemERPs).ThenInclude(di => di.Desenho)
                .Include(i => i.Revisoes)
                .Include(i => i.PerfilItemERPs).ThenInclude(pi => pi.Revisoes)
                .Include(i => i.PerfilItemERPs).ThenInclude(pi => pi.Perfil)
                .FirstOrDefaultAsync(i => i.Id == id);
            if (item == null) return NotFound();
            return View(item);
        }

        // GET: ItensERP/Create
        public IActionResult Create()
        {
            var vm = new ItemERPCreateEditViewModel();
            PopulateSelections(vm);
            return View(vm);
        }

        // POST: ItensERP/Create
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ItemERPCreateEditViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                PopulateSelections(vm);
                return View(vm);
            }

            var item = new ItemERP
            {
                ERP = vm.ERP,
                Descricao = vm.Descricao,
                TipoItem = vm.TipoItem,
                Acabamento = vm.Acabamento,
                Classificacao = vm.Classificacao,
                ChapaAberta = vm.ChapaAberta,
                Altura = vm.Espessura,
                AreaSuperficial = vm.AreaSuperficial,
                PesoLiquidoMetro = vm.PesoLiquidoMetro,
                PesoBrutoMetro = vm.PesoBrutoMetro,
                QuantidadeDobras = vm.QuantidadeDobras
            };

            _context.ItensERP.Add(item);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ConfiguradorItemERP), new { id = item.Id });
        }

        // GET: ItensERP/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var item = await _context.ItensERP
                .Include(i => i.Tags)
                .Include(i => i.AgrupadorItensERP)
                .Include(i => i.DesenhoItemERPs)
                .Include(i => i.PerfilItemERPs)
                .FirstOrDefaultAsync(i => i.Id == id);
            if (item == null) return NotFound();

            var vm = new ItemERPCreateEditViewModel
            {
                Id = item.Id,
                ERP = item.ERP,
                Descricao = item.Descricao,
                TipoItem = item.TipoItem,
                Acabamento = item.Acabamento,
                Classificacao = item.Classificacao,
                ChapaAberta = item.ChapaAberta,
                Espessura = item.Altura,
                AreaSuperficial = item.AreaSuperficial,
                PesoLiquidoMetro = item.PesoLiquidoMetro,
                PesoBrutoMetro = item.PesoBrutoMetro,
                QuantidadeDobras = item.QuantidadeDobras,
                SelectedTagIds = item.Tags.Select(t => t.Id).ToList(),
                SelectedAgrupadorIds = item.AgrupadorItensERP.Select(ai => ai.AgrupadorId).ToList(),
                SelectedDesenhoIds = item.DesenhoItemERPs.Select(d => (int)d.DesenhoId).ToList(),
                SelectedPerfilIds = item.PerfilItemERPs.Select(pi => pi.PerfilId).ToList()
            };
            PopulateSelections(vm);
            return View(vm);
        }

        // POST: ItensERP/Edit/5
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ItemERPCreateEditViewModel vm)
        {
            if (id != vm.Id) return BadRequest();
            if (!ModelState.IsValid)
            {
                PopulateSelections(vm);
                return View(vm);
            }

            var item = await _context.ItensERP
                .Include(i => i.Tags)
                .Include(i => i.AgrupadorItensERP)
                .Include(i => i.DesenhoItemERPs)
                .Include(i => i.PerfilItemERPs)
                .FirstOrDefaultAsync(i => i.Id == id);
            if (item == null) return NotFound();

            // Atualiza campos básicos
            item.ERP = vm.ERP;
            item.Descricao = vm.Descricao;
            item.TipoItem = vm.TipoItem;
            item.Acabamento = vm.Acabamento;
            item.Classificacao = vm.Classificacao;
            item.ChapaAberta = vm.ChapaAberta;
            item.Altura = vm.Espessura;
            item.AreaSuperficial = vm.AreaSuperficial;
            item.PesoLiquidoMetro = vm.PesoLiquidoMetro;
            item.PesoBrutoMetro = vm.PesoBrutoMetro;
            item.QuantidadeDobras = vm.QuantidadeDobras;

            // Tags
            item.Tags.Clear();
            var tags = await _context.Tags.Where(t => vm.SelectedTagIds.Contains(t.Id)).ToListAsync();
            tags.ForEach(t => item.Tags.Add(t));

            // Agrupadores
            _context.AgrupadorItemERPs.RemoveRange(item.AgrupadorItensERP);
            foreach (var agrId in vm.SelectedAgrupadorIds)
                item.AgrupadorItensERP.Add(new AgrupadorItemERP { ItemERPId = item.Id, AgrupadorId = agrId, Status = true });

            // Desenhos
            item.DesenhoItemERPs.Clear();
            var desenhos = await _context.Desenhos
                .Where(d => vm.SelectedDesenhoIds.Contains((int)d.DesenhoId))
                .ToListAsync();
            desenhos.ForEach(d => item.DesenhoItemERPs.Add(new DesenhoItemERP
            {
                ItemERPId = item.Id,
                DesenhoId = d.DesenhoId
            }));


            // Perfis
            item.PerfilItemERPs.Clear();
            var perfis = await _context.Perfis
                .Where(p => vm.SelectedPerfilIds.Contains(p.Id))
                .ToListAsync();
            perfis.ForEach(p => item.PerfilItemERPs.Add(new PerfilItemERP { ItemERPId = item.Id, PerfilId = p.Id }));

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //-------------------------------------------------------------------configurador-------------------------------------------------------------------

        // GET: ItensERP/ConfiguradorItemERP/5
        public async Task<IActionResult> ConfiguradorItemERP(int? id)
        {
            if (id == null) return NotFound();

            var item = await _context.ItensERP
                .Include(i => i.AgrupadorItensERP)
                .Include(i => i.ComponenteItemERPs)
                .Include(i => i.DesenhoItemERPs)
                .Include(i => i.Revisoes)
                .Include(i => i.PerfilItemERPs).ThenInclude(pi => pi.Revisoes)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (item == null) return NotFound();


            // monta VM
            var vm = new ConfiguradorItemERPViewModel
            {
                Id = item.Id,
                ERP = item.ERP,
                Descricao = item.Descricao,
                TipoItem = item.TipoItem,
                Acabamento = item.Acabamento,
                Classificacao = item.Classificacao,
                Status = item.Status,
                ChapaAberta = item.ChapaAberta,
                Espessura = item.Altura,
                Aco = item.Aco,
                PesoLiquidoMetro = item.PesoLiquidoMetro,
                PesoBrutoMetro = item.PesoBrutoMetro,
                QuantidadeDobras = item.QuantidadeDobras,

                // Seção 04
                SelectedAgrupadorIds = item.AgrupadorItensERP.Select(ai => ai.AgrupadorId).ToList(),

                // Seção 01 — monta lista de linhas completas
                Desenhos = item.DesenhoItemERPs
                    .Where(d => d.Desenho != null)
                    .Select(d => new DesenhoLinhaViewModel
                    {
                        Id = (int)d.DesenhoId,
                        Nome = d.Desenho!.Nome,
                        Descricao = d.Desenho!.Descricao,
                        Revisao = d.Desenho!.Revisao
                    }).ToList(),


                // Seção 02
                Revisoes = item.Revisoes.Select(r => new RevisaoLinhaViewModel
                {
                    Id = r.Id,
                    Numero = r.Numero,
                    MotivoRevisao = r.Motivo,
                    DataRevisao = r.Data
                }).ToList(),

                // Seção 04 — perfis
                PerfisSection = item.PerfilItemERPs.Select(pi => new PerfilLinhaViewModel
                {
                    Id = pi.Id,
                    PerfilId = pi.PerfilId,
                    Aco = pi.Aco,
                    Revisoes = pi.Revisoes.Select(rr => new RevisaoLinhaViewModel
                    {
                        Id = rr.Id,
                        Numero = rr.Numero,
                        MotivoRevisao = rr.Motivo,
                        DataRevisao = rr.Data
                    }).ToList()
                }).ToList(),

                // Seção 06 — famílias
                ComponentesFamily = item.ComponenteItemERPs.Select(ci => new FamilyComponenteViewModel
                {
                    Id = ci.Id,
                    ComponenteId = ci.ComponenteId
                }).ToList(),

                AgrupadoresFamily = item.AgrupadorItensERP.Select(ai => new FamilyAgrupadorViewModel
                {
                    Id = ai.Id,
                    AgrupadorId = ai.AgrupadorId
                }).ToList()
            };

            PopulateAuxLists(vm);
            vm.AllComponentes = _context.Componentes
                .Select(c => new SelectListItem(c.Nome, c.Id.ToString()))
                .ToList();

            ViewBag.ItensComposicaoSelecionados = vm.ItensIntegrantes
                ?.Select(r => new SelectListItem
                {
                    Value = r.ItemERPId.ToString(),
                    Text = _context.ItensERP
                        .Where(i => i.Id == r.ItemERPId)
                        .Select(i => $"{i.ERP}|{i.Descricao}|{i.Status}")
                        .FirstOrDefault() ?? "Desconhecido"
                }).ToList();

            ViewBag.ItensPintadosSelecionados = vm.ItensPintados?.Select(r => new SelectListItem
            {
                Value = r.ItemERPId.ToString(),
                Text = _context.ItensERP
                    .Where(i => i.Id == r.ItemERPId)
                    .Select(i => $"{i.ERP}|{i.Descricao}|{i.Status}")
                    .FirstOrDefault() ?? "Desconhecido"
            }).ToList();

            ViewBag.ItensGalvanizadosSelecionados = vm.ItensGalvanizados?.Select(r => new SelectListItem
            {
                Value = r.ItemERPId.ToString(),
                Text = _context.ItensERP
                    .Where(i => i.Id == r.ItemERPId)
                    .Select(i => $"{i.ERP}|{i.Descricao}|{i.Status}")
                    .FirstOrDefault() ?? "Desconhecido"
            }).ToList();

            ViewBag.AllTags = await _context.Tags
    .Select(t => new SelectListItem
    {
        Value = t.Id.ToString(),
        Text = t.Nome
    })
    .ToListAsync();




            return View(vm);
        }

        // POST: ItensERP/ConfiguradorItemERP
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfiguradorItemERP(ConfiguradorItemERPViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                PopulateAuxLists(vm);
                return View(vm);
            }

            var item = await _context.ItensERP
                .Include(i => i.DesenhoItemERPs).ThenInclude(d => d.Desenho)
                .Include(i => i.Revisoes)
                .Include(i => i.PerfilItemERPs).ThenInclude(pi => pi.Revisoes)
                .Include(i => i.ComponenteItemERPs)
                .Include(i => i.AgrupadorItensERP)
                .FirstOrDefaultAsync(i => i.Id == vm.Id);
            if (item == null) return NotFound();

            // 0) Campos básicos
            item.ERP = vm.ERP;
            item.Descricao = vm.Descricao;
            item.TipoItem = vm.TipoItem;
            item.Acabamento = vm.Acabamento;
            item.Classificacao = vm.Classificacao;
            item.Status = vm.Status!.Value;
            item.ChapaAberta = vm.ChapaAberta;
            item.Altura = vm.Espessura;
            item.Aco = vm.Aco;
            item.PesoLiquidoMetro = vm.PesoLiquidoMetro;
            item.PesoBrutoMetro = vm.PesoBrutoMetro;
            item.QuantidadeDobras = vm.QuantidadeDobras;

            // 1) SEÇÃO 01 — Desenhos
            await _context.Entry(item).Collection(i => i.DesenhoItemERPs).LoadAsync();
            item.DesenhoItemERPs.Clear();
            if (vm.Desenhos != null && vm.Desenhos.Any())
            {
                foreach (var dvm in vm.Desenhos)
                {
                    if (dvm.Id <= 0) continue;
                    item.DesenhoItemERPs.Add(new DesenhoItemERP
                    {
                        ItemERPId = item.Id,
                        DesenhoId = dvm.Id
                    });

                }
            }

            // 2) SEÇÃO 02 — Revisões do ItemERP
            _context.RevisaoItemERPs.RemoveRange(item.Revisoes);
            foreach (var rvm in vm.Revisoes)
            {
                item.Revisoes.Add(new RevisaoItemERP
                {
                    Numero = rvm.Numero,
                    Motivo = rvm.MotivoRevisao,
                    Data = rvm.DataRevisao!.Value,
                    ItemERPId = item.Id
                });
            }

            // 3) SEÇÃO 04 — Perfis
            _context.PerfilItemERPs.RemoveRange(item.PerfilItemERPs);
            foreach (var pvm in vm.PerfisSection)
            {
                var perfilItem = new PerfilItemERP
                {
                    ItemERPId = item.Id,
                    PerfilId = pvm.PerfilId
                };
                foreach (var rr in pvm.Revisoes)
                {
                    perfilItem.Revisoes.Add(new RevisaoPerfilItemERP
                    {
                        Numero = rr.Numero,
                        Motivo = rr.MotivoRevisao,
                        Data = rr.DataRevisao!.Value,
                        PerfilItemERPId = perfilItem.Id
                    });
                }
                item.PerfilItemERPs.Add(perfilItem);
            }

            // 4) Seção 04 — Agrupadores
            _context.AgrupadorItemERPs.RemoveRange(item.AgrupadorItensERP);
            foreach (var agrId in vm.SelectedAgrupadorIds)
                item.AgrupadorItensERP.Add(new AgrupadorItemERP { ItemERPId = item.Id, AgrupadorId = agrId, Status = true });


            // 6) Seção 06 — Famílias
            _context.ComponenteItemERPs.RemoveRange(item.ComponenteItemERPs);
            _context.AgrupadorItemERPs.RemoveRange(item.AgrupadorItensERP);
            foreach (var c in vm.ComponentesFamily)
                item.ComponenteItemERPs.Add(new ComponenteItemERP { ItemERPId = item.Id, ComponenteId = c.ComponenteId });
            foreach (var a in vm.AgrupadoresFamily)
                item.AgrupadorItensERP.Add(new AgrupadorItemERP { ItemERPId = item.Id, AgrupadorId = a.AgrupadorId, Status = true });

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        //-----------------------------------------------------------------------importar erp datasul----------------------------------------------------

        // ---------------------------
        //        Importar
        // ---------------------------

        [HttpPost]
        public async Task<IActionResult> ImportarLST(IFormFile arquivo, [FromServices] LSTParserService parser)
        {
            if (arquivo == null || arquivo.Length == 0)
                return BadRequest("Nenhum arquivo enviado.");

            List<ItemERPImportDto> itensImportados;
            try
            {
                using var stream = arquivo.OpenReadStream();
                itensImportados = parser.ParseLST(stream, _logger);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao processar o arquivo LST");
                TempData["Erro"] = "Erro ao processar o arquivo. Verifique se está no formato correto.";
                return RedirectToAction("ImportarLST");
            }

            // Verifica quais já existem no banco
            var erpsExistentes = _context.ItensERP.Select(i => i.ERP).ToHashSet();
            var novosItens = itensImportados
                .Where(i => !string.IsNullOrWhiteSpace(i.ERP) && !erpsExistentes.Contains(i.ERP))
                .ToList();

            return View("ImportarPreview", novosItens);
        }


        // ---------------------------
        //    Confirmar Importação
        // ---------------------------
        [HttpGet]
        public IActionResult ImportarLST()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmarImportacao(List<ItemERPImportDto> itens)
        {
            _logger.LogInformation("Quantidade de itens recebidos: {Qtd}", itens?.Count ?? -1);

            if (itens == null || !itens.Any())
            {
                TempData["Erro"] = "Nenhum item recebido para importação.";
                return RedirectToAction("Index");
            }

            var novosItens = new List<ItemERP>();

            foreach (var dto in itens)
            {
                // Verifica se o item já existe
                bool jaExiste = await _context.ItensERP
                    .AnyAsync(i => i.ERP == dto.ERP);

                if (jaExiste)
                    continue;

                var novoItem = new ItemERP
                {
                    ERP = dto.ERP.Trim(),
                    Descricao = dto.Descricao?.Trim(),
                    PesoLiquidoMetro = dto.PesoLiquidoMetro,
                    PesoBrutoMetro = dto.PesoBrutoMetro,
                    DataCriacao = dto.DataCriacao,
                    Status = StatusItemERP.Ativo, // ou o valor padrao 
                    TipoItem = TipoItem.Componente, // definir um padrao 
                    Acabamento = null,
                    Classificacao = null
                };

                novosItens.Add(novoItem);
            }

            if (novosItens.Any())
            {
                await _context.ItensERP.AddRangeAsync(novosItens);
                await _context.SaveChangesAsync();
                TempData["Sucesso"] = $"{novosItens.Count} itens importados com sucesso.";
            }
            else
            {
                TempData["Aviso"] = "Todos os itens da lista já existem no banco.";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmarImportacaoLote([FromBody] List<ItemERPImportDto> itens)
        {
            _logger.LogInformation("Recebido lote com {Qtd} itens para importação", itens?.Count ?? -1);

            if (itens == null || !itens.Any())
            {
                return BadRequest(new { sucesso = false, mensagem = "Nenhum item recebido no lote." });
            }

            var adicionados = new List<string>();
            var erros = new List<string>();

            foreach (var dto in itens)
            {
                try
                {
                    if (await _context.ItensERP.AnyAsync(i => i.ERP == dto.ERP))
                    {
                        erros.Add($"ERP {dto.ERP} já existe.");
                        continue;
                    }

                    var novoItem = new ItemERP
                    {
                        ERP = dto.ERP?.Trim(),
                        Descricao = dto.Descricao?.Trim(),
                        PesoLiquidoMetro = dto.PesoLiquidoMetro,
                        PesoBrutoMetro = dto.PesoBrutoMetro,
                        DataCriacao = dto.DataCriacao,
                        Status = StatusItemERP.Ativo,
                        TipoItem = TipoItem.Componente
                    };

                    await _context.ItensERP.AddAsync(novoItem);
                    adicionados.Add(novoItem.ERP);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao importar item ERP: {ERP}", dto.ERP);
                    erros.Add($"Erro no ERP {dto.ERP}: {ex.Message}");
                }
            }

            await _context.SaveChangesAsync();

            return Json(new
            {
                sucesso = true,
                salvos = adicionados.Count,
                erros = erros
            });
        }


        // ---------------------------
        // Helpers para popular dropdowns
        // ---------------------------

        private void PopulateSelections(ItemERPCreateEditViewModel vm)
        {
            vm.AllTags = _context.Tags
                .Select(t => new SelectListItem(t.Nome, t.Id.ToString(), vm.SelectedTagIds.Contains(t.Id)))
                .ToList();

            vm.AllAgrupadores = _context.Agrupadores
                .Select(a => new SelectListItem(a.Nome, a.Id.ToString(), vm.SelectedAgrupadorIds.Contains(a.Id)))
                .ToList();

            vm.AllDesenhos = _context.Desenhos
                .Select(d => new SelectListItem(d.Nome, d.DesenhoId.ToString(), vm.SelectedDesenhoIds.Contains((int)d.DesenhoId)))
                .ToList();

            vm.AllPerfis = _context.Perfis
                .Select(p => new SelectListItem(p.Descricao ?? p.Id.ToString(), p.Id.ToString(), vm.SelectedPerfilIds.Contains(p.Id)))
                .ToList();
        }

        private void PopulateAuxLists(ConfiguradorItemERPViewModel vm)
        {
            vm.AllAgrupadores = _context.Agrupadores
                .Select(a => new SelectListItem(a.Nome, a.Id.ToString(), vm.SelectedAgrupadorIds.Contains(a.Id)))
                .ToList();

            vm.AllDesenhos = _context.Desenhos
                .Select(d => new SelectListItem(d.Nome, d.DesenhoId.ToString()))
                .ToList();

            vm.AllPerfisSection = _context.Perfis
                .Select(p => new SelectListItem(p.Descricao, p.Id.ToString()))
                .ToList();

            vm.AllItensERP = _context.ItensERP
                .Select(i => new SelectListItem(i.ERP, i.Id.ToString()))
                .ToList();

            vm.AllComponentes = _context.Componentes
                .Select(c => new SelectListItem(c.Nome, c.Id.ToString()))
                .ToList();
        }
    }
}
