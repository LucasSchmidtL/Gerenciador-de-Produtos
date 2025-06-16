using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using Gerenciador_de_Produtos.Models.DTOs;
using Microsoft.Extensions.Logging;

namespace Gerenciador_de_Produtos.Services
{
    public class LSTParserService
    {
        public List<ItemERPImportDto> ParseLST(Stream stream, ILogger logger)
        {
            var lista = new List<ItemERPImportDto>();
            using var reader = new StreamReader(stream, Encoding.GetEncoding("Windows-1252"));

            string? linha;
            int linhaNumero = 0;
            int erros = 0;

            // Pula cabeçalhos até encontrar uma linha que começa com ERP (4 a 6 dígitos)
            while ((linha = reader.ReadLine()) != null)
            {
                linhaNumero++;
                if (Regex.IsMatch(linha, @"^\d{4,6}"))
                    break;
            }

            // Começa a ler e processar o restante do arquivo
            while ((linha = reader.ReadLine()) != null)
            {
                linhaNumero++;

                if (string.IsNullOrWhiteSpace(linha)) continue;

                var col1 = linha;

                // Ignora linhas que não começam com ERP numérico
                if (!Regex.IsMatch(col1, @"^\d{4,6}")) continue;

                // Ignora cabeçalhos ou separadores
                if (Regex.IsMatch(col1, @"^(Item\s+Descrição|Família\s+Peso|Grupo:|[-]{5,})", RegexOptions.IgnoreCase))
                    continue;

                try
                {
                    var col2 = reader.ReadLine();
                    linhaNumero++;

                    if (string.IsNullOrWhiteSpace(col2)) continue;

                    // Extrai campos da primeira linha
                    var erp = col1.Substring(0, 10).Trim();
                    var linhaSemErp = col1.Substring(6).Trim();

                    // Pega a descrição até antes da palavra "UN"
                    var descricao = Regex.Split(linhaSemErp, @"\sUN\b|\sun\b|\sUn\b")[0].Trim();

                    // Extrai data se presente
                    var dataMatch = Regex.Match(col1, @"\d{2}/\d{2}/\d{4}");
                    string? dataStr = dataMatch.Success ? dataMatch.Value : null;

                    // Extrai pesos da segunda linha
                    float pesoLiq = 0, pesoBruto = 0;
                    if (col2.Length >= 35)
                    {
                        var pesoLiqStr = col2.Substring(13, 10).Trim();
                        var pesoBrutoStr = col2.Substring(24, 10).Trim();
                        float.TryParse(pesoLiqStr, NumberStyles.Float, new CultureInfo("pt-BR"), out pesoLiq);
                        float.TryParse(pesoBrutoStr, NumberStyles.Float, new CultureInfo("pt-BR"), out pesoBruto);
                    }

                    // Tenta interpretar a data
                    DateTime dataImplantacao;
                    if (string.IsNullOrWhiteSpace(dataStr) ||
                        !DateTime.TryParseExact(dataStr, new[] { "dd/MM/yyyy", "MM/yyyy", "yyyy" },
                            CultureInfo.InvariantCulture, DateTimeStyles.None, out dataImplantacao))
                    {
                        // Tenta com fallback usando "01/" no início do mês
                        var matchMesAno = Regex.Match(col1, @"\d{2}/\d{4}");
                        if (matchMesAno.Success &&
                            DateTime.TryParseExact("01/" + matchMesAno.Value, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dataImplantacao))
                        {
                            // OK com fallback
                        }
                        else
                        {
                            erros++;
                            logger?.LogWarning("Data inválida na linha {LinhaNumero}: \"{Data}\". Corrija antes de importar. Sugestão: adicione o dia como '01'.", linhaNumero, dataStr ?? "nulo");
                            if (erros > 50)
                                throw new Exception("Mais de 50 linhas inválidas encontradas. Corrija o arquivo antes de tentar novamente.");
                            continue;
                        }
                    }

                    // Adiciona item importado à lista final
                    lista.Add(new ItemERPImportDto
                    {
                        ERP = erp,
                        Descricao = descricao,
                        PesoLiquidoMetro = pesoLiq,
                        PesoBrutoMetro = pesoBruto,
                        DataCriacao = dataImplantacao
                    });
                }
                catch (Exception ex)
                {
                    erros++;
                    logger?.LogError(ex, "Erro ao processar a linha {LinhaNumero}: {Linha}", linhaNumero, linha);
                    if (erros > 50)
                        throw new Exception("Mais de 50 linhas inválidas encontradas. Corrija o arquivo antes de tentar novamente.");
                }
            }

            return lista;
        }
    }
}
