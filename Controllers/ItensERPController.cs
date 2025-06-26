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
using System.Text.Json;
using Azure;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

        // esse cara aqyu é so pra buscar o desenho da aba, não ta na outra controller pq se não eles não se enxergam, vem DDD por favor
        [HttpGet]
        public async Task<IActionResult> BuscarDesenhoItemERP(string? term, string? status)
        {
            var query = _context.Desenhos.AsQueryable();

            if (!string.IsNullOrEmpty(term))
                query = query.Where(d => d.Nome.Contains(term) || d.Descricao.Contains(term));

            if (!string.IsNullOrEmpty(status))
                query = query.Where(d => d.Status.ToString() == status);

            var resultados = await query
                .OrderByDescending(d => d.DataCriacao)
                .Select(d => new
                {
                    id = d.DesenhoId,
                    text = $"{d.Nome} | Rev. {d.Revisao} | {d.Descricao}",
                    nome = d.Nome,
                    descricao = d.Descricao,
                    revisao = d.Revisao,
                    dataCriacao = d.DataCriacao.HasValue ? d.DataCriacao.Value.ToString("yyyy-MM-dd") : ""
                })
                .Take(30)
                .ToListAsync();

            return Json(resultados);
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

        // metodo para carregar resultados filtrados via AJAX na INDEX
        [HttpGet]
        public async Task<IActionResult> Filtrar(string? tipo, string? status, string? tag, string? termo)
        {
            var query = _context.ItensERP
                .Include(i => i.Tags)
                .Include(i => i.Revisoes)
                .Include(i => i.DesenhoItemERPs).ThenInclude(di => di.Desenho)
                .Include(i => i.AgrupadorItensERP).ThenInclude(ai => ai.Agrupador)
                .Include(i => i.PerfilItemERPs).ThenInclude(pi => pi.Perfil)
                .Include(i => i.ComponenteItemERPs).ThenInclude(ci => ci.Componente)
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


        //-------------------------------------------------------------------configurador-------------------------------------------------------------------

        // GET: ItensERP/ConfiguradorItemERP/5
        public async Task<IActionResult> ConfiguradorItemERP(int? id)
        {
            if (id == null) return NotFound();

            var item = await _context.ItensERP
                .Include(i => i.Tags)
                .Include(i => i.AgrupadorItensERP)
                .Include(i => i.ComponenteItemERPs)
                .Include(i => i.DesenhoItemERPs).ThenInclude(d => d.Desenho)
                .Include(i => i.Revisoes)
                .Include(i => i.PerfilItemERPs).ThenInclude(pi => pi.Revisoes)
                .Include(i => i.PerfilItemERPs).ThenInclude(pi => pi.Perfil)
                .Include(i => i.ItensVinculados)
                .Include(i => i.ItensCompostos).ThenInclude(ic => ic.ItemFilho)
                .Include(i => i.ItensCompostos).ThenInclude(ic => ic.Variaveis) 
                .Include(i => i.VariaveisComposicao)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (item == null) return NotFound();




            // carrega descricooes dos itens vinculados
            var idsVinculados = item.ItensVinculados.Select(v => v.VinculadoId).ToList();
            var descricoesItens = await _context.ItensERP
                .Where(i => idsVinculados.Contains(i.Id))
                .ToDictionaryAsync(i => i.Id, i => $"{i.ERP} | {i.Descricao}");

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
                SelectedTagIds = item.Tags.Select(t => t.Id).ToList(),


                Desenhos = item.DesenhoItemERPs.Select(d => new DesenhoLinhaViewModel
                {
                    Id = (int)d.DesenhoId,
                    Nome = d.Desenho?.Nome,
                    Descricao = d.Desenho?.Descricao,
                    Revisao = d.Desenho?.Revisao
                }).ToList(),

                Revisoes = item.Revisoes.Select(r => new RevisaoLinhaViewModel
                {
                    Id = r.Id,
                    Numero = r.Numero,
                    MotivoRevisao = r.Motivo,
                    DataRevisao = r.Data
                }).ToList(),

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

                ComponentesFamily = item.ComponenteItemERPs.Select(ci => new FamilyComponenteViewModel
                {
                    Id = ci.Id,
                    ComponenteId = ci.ComponenteId
                }).ToList(),

                AgrupadoresFamily = item.AgrupadorItensERP.Select(ai => new FamilyAgrupadorViewModel
                {
                    Id = ai.Id,
                    AgrupadorId = ai.AgrupadorId
                }).ToList(),

                ItensCompostos = item.ItensCompostos.Select(ic => new ItemERPCompostoViewModel
                {
                    Id = ic.Id,
                    ItemERPId = ic.ItemFilhoId,
                    Comprimento = ic.Comprimento,
                    Profundidade = ic.Profundidade,
                    Altura = ic.Altura,
                    Quantidade = ic.Quantidade,
                    ItemERPDescricao = ic.ItemFilho != null ? $"{ic.ItemFilho.ERP} | {ic.ItemFilho.Descricao}" : string.Empty,

                    Variaveis = ic.Variaveis.Select(v => new VariaveisItemERPComposto
                    {
                        Id = v.Id,
                        Nome = v.Nome,
                        Descricao = v.Descricao,
                        Tipo = v.Tipo,
                        Valor = v.Valor,
                        Status = v.Status,
                        ItemERPCompostoId = v.ItemERPCompostoId
                    }).ToList()
                }).ToList(),


                ItensVinculadosPintado = item.ItensVinculados
                    .Where(v => v.Tipo == TipoVinculoERP.Pintado)
                    .Select(v => new ItemERPVinculadoViewModel
                    {
                        ItemERPId = v.VinculadoId,
                        Tipo = v.Tipo,
                        DesenhoId = v.DesenhoId,
                        ItemERPDescricao = descricoesItens.ContainsKey(v.VinculadoId)
                            ? descricoesItens[v.VinculadoId]
                            : string.Empty
                    }).ToList(),

                ItensVinculadosGalvanizado = item.ItensVinculados
                    .Where(v => v.Tipo == TipoVinculoERP.Galvanizado)
                    .Select(v => new ItemERPVinculadoViewModel
                    {
                        ItemERPId = v.VinculadoId,
                        Tipo = v.Tipo,
                        DesenhoId = v.DesenhoId,
                        ItemERPDescricao = descricoesItens.ContainsKey(v.VinculadoId)
                            ? descricoesItens[v.VinculadoId]
                            : string.Empty
                    }).ToList()

                

            };

            // Select2 - Pintado (para buscas filtradas via ajax com pré-seleção)
            var idsPintados = vm.ItensVinculadosPintado.Select(v => v.ItemERPId).ToList();
            vm.ItensVinculadosPintadoSelecionados = await _context.ItensERP
                .Where(i => idsPintados.Contains(i.Id))
                .Select(i => new SelectListItem
                {
                    Value = i.Id.ToString(),
                    Text = $"{i.ERP}|{i.Descricao}|{i.Status}"
                }).ToListAsync();

            // Preenche os combos auxiliares
            PopulateAuxLists(vm);
            return View(vm);
        }


        // POST DO CONFIGURADOR

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfiguradorItemERP(ConfiguradorItemERPViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                PopulateAuxLists(vm);
                return View(vm);
            }

            var item = await _context.ItensERP
                .Include (i => i.Tags)
                .Include(i => i.DesenhoItemERPs)
                .Include(i => i.Revisoes)
                .Include(i => i.PerfilItemERPs).ThenInclude(p => p.Revisoes)
                .Include(i => i.ComponenteItemERPs)
                .Include(i => i.AgrupadorItensERP)
                .Include(i => i.ItensVinculados)
                .Include(i => i.ItensCompostos)
                .Include(i => i.ItensVinculados)
                .FirstOrDefaultAsync(i => i.Id == vm.Id);

            if (item == null) return NotFound();

            // Atualiza campos principais
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

            // Atualiza desenhos
            item.DesenhoItemERPs.Clear();
            if (vm.Desenhos != null)
            {
                foreach (var d in vm.Desenhos)
                {
                    item.DesenhoItemERPs.Add(new DesenhoItemERP { DesenhoId = d.Id, ItemERPId = item.Id });
                }
            }


            // Limpa compostos e variáveis
            var compostosAntigos = await _context.ItensERPCompostos
                .Where(c => c.ItemPaiId == item.Id)
                .ToListAsync();

            var idsCompostos = compostosAntigos.Select(c => c.Id).ToList();
            var variaveisAntigas = await _context.VariaveisItemERPCompostos
                .Where(v => idsCompostos.Contains(v.ItemERPCompostoId)).ToListAsync();

            _context.VariaveisItemERPCompostos.RemoveRange(variaveisAntigas);
            _context.ItensERPCompostos.RemoveRange(compostosAntigos);

            await _context.SaveChangesAsync();

            // Adiciona novos compostos e variáveis
            if (vm.ItensCompostos != null)
            {
                foreach (var ic in vm.ItensCompostos)
                {
                    var novoComposto = new ItemERPComposto
                    {
                        ItemPaiId = item.Id,
                        ItemFilhoId = ic.ItemERPId,
                        Comprimento = ic.Comprimento,
                        Altura = ic.Altura,
                        Profundidade = ic.Profundidade,
                        Quantidade = ic.Quantidade
                    };

                    _context.ItensERPCompostos.Add(novoComposto);
                    await _context.SaveChangesAsync();

                    item.ItensCompostos.Add(novoComposto); // <- aqui ó 👈

                    foreach (var variavel in ic.Variaveis ?? new())
                    {
                        _context.VariaveisItemERPCompostos.Add(new VariaveisItemERPComposto
                        {
                            Nome = variavel.Nome,
                            Descricao = variavel.Descricao,
                            Tipo = variavel.Tipo,
                            Valor = variavel.Valor,
                            Status = variavel.Status,
                            ItemERPCompostoId = novoComposto.Id
                        });
                    }

                    await _context.SaveChangesAsync();
                }
            }

            // VÍNCULOS ===

            foreach (var vinculo in item.ItensVinculados.Where(v => v.Tipo == TipoVinculoERP.Integrante).ToList())
                item.ItensVinculados.Remove(vinculo);

            foreach (var v in vm.ItensIntegrantes ?? new())
            {
                item.ItensVinculados.Add(new ItemERPVinculado
                {
                    ItemERPId = item.Id,
                    VinculadoId = v.ItemERPId,
                    Tipo = TipoVinculoERP.Integrante
                });
            }

            foreach (var vinculo in item.ItensVinculados.Where(v => v.Tipo == TipoVinculoERP.Pintado).ToList())
                item.ItensVinculados.Remove(vinculo);

            foreach (var v in vm.ItensVinculadosPintado ?? new())
            {
                if (v.ItemERPId == 0) continue;
                item.ItensVinculados.Add(new ItemERPVinculado
                {
                    ItemERPId = item.Id,
                    VinculadoId = v.ItemERPId,
                    Tipo = TipoVinculoERP.Pintado
                });
            }

            foreach (var vinculo in item.ItensVinculados.Where(v => v.Tipo == TipoVinculoERP.Galvanizado).ToList())
                item.ItensVinculados.Remove(vinculo);

            foreach (var v in vm.ItensVinculadosGalvanizado ?? new())
            {
                if (v.ItemERPId == 0) continue;
                var vinculado = await _context.ItensERP.AsNoTracking().FirstOrDefaultAsync(i => i.Id == v.ItemERPId);
                if (vinculado != null)
                {
                    item.ItensVinculados.Add(new ItemERPVinculado
                    {
                        ItemERPId = item.Id,
                        VinculadoId = v.ItemERPId,
                        Tipo = TipoVinculoERP.Galvanizado,
                        ItemERPDescricao = $"{vinculado.ERP} – {vinculado.Descricao}"
                    });
                }
            }

            // Atualiza Tags
            item.Tags.Clear();

            if (vm.SelectedTagIds != null && vm.SelectedTagIds.Any())
            {
                var tagsSelecionadas = await _context.Tags
                    .Where(t => vm.SelectedTagIds.Contains(t.Id))
                    .ToListAsync();

                foreach (var tag in tagsSelecionadas)
                {
                    item.Tags.Add(tag);
                }
            }


            // Revisao do ItemERP
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


            // Perfis
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



            // Agrupadores
            _context.AgrupadorItemERPs.RemoveRange(item.AgrupadorItensERP);
            foreach (var a in vm.AgrupadoresFamily)
                item.AgrupadorItensERP.Add(new AgrupadorItemERP { ItemERPId = item.Id, AgrupadorId = a.AgrupadorId, Status = true });



            // Famílias
            _context.ComponenteItemERPs.RemoveRange(item.ComponenteItemERPs);
            foreach (var c in vm.ComponentesFamily)
                item.ComponenteItemERPs.Add(new ComponenteItemERP { ItemERPId = item.Id, ComponenteId = c.ComponenteId });
            




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

            vm.AllTags = _context.Tags
                 .Select(t => new SelectListItem(t.Nome, t.Id.ToString(), vm.SelectedTagIds.Contains(t.Id)))
                 .ToList();


        }
    }
}
