using Microsoft.EntityFrameworkCore;
using ProjetoMidasAPI.Data;
using ProjetoMidasAPI.Models;
using ProjetoMidasAPI.Models.Enuns;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace ProjetoMidasAPI.Services
{
    public class AnaliseIAService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public AnaliseIAService(AppDbContext context, IConfiguration configuration, HttpClient httpClient)
        {
            _context = context;
            _configuration = configuration;
            _httpClient = httpClient;
        }

        public async Task<AnaliseIAResult> AnalisarProjecao(int idProjecao, int idUsuario)
        {
            var projecao = await _context.Projecoes
                .FirstOrDefaultAsync(p => p.IdProjecao == idProjecao && p.IdUsuario == idUsuario);

            if (projecao == null)
                throw new KeyNotFoundException("Projecao nao encontrada");

            var lancamentos = await _context.Lancamentos
                .Where(l => l.IdUsuario == idUsuario)
                .OrderByDescending(l => l.Data)
                .Take(200)
                .ToListAsync();

            var totalReceitas = lancamentos
                .Where(l => l.TipoLancamento.HasValue && l.TipoLancamento.Value == TipoLancamentoEnum.Receita)
                .Sum(l => l.Valor);

            var totalDespesas = lancamentos
                .Where(l => l.TipoLancamento.HasValue && l.TipoLancamento.Value == TipoLancamentoEnum.Despesa)
                .Sum(l => l.Valor);

            var saldo = totalReceitas - totalDespesas;

            var seisMesesAtras = DateTime.UtcNow.AddMonths(-6);
            var lancamentosMensais = lancamentos
                .Where(l => l.Data >= seisMesesAtras)
                .GroupBy(l => new { l.Data.Year, l.Data.Month })
                .OrderBy(g => g.Key.Year).ThenBy(g => g.Key.Month)
                .Select(g => new AgregadoMensal
                {
                    Periodo = $"{g.Key.Month:D2}/{g.Key.Year}",
                    Receitas = g.Where(l => l.TipoLancamento.HasValue && l.TipoLancamento.Value == TipoLancamentoEnum.Receita).Sum(l => l.Valor),
                    Despesas = g.Where(l => l.TipoLancamento.HasValue && l.TipoLancamento.Value == TipoLancamentoEnum.Despesa).Sum(l => l.Valor)
                })
                .ToList();

            var resumoMensal = string.Join("\n", lancamentosMensais.Select(m =>
                $"- {m.Periodo}: Receitas R$ {m.Receitas:N2} | Despesas R$ {m.Despesas:N2} | Saldo R$ {m.Receitas - m.Despesas:N2}"));

            var recorrencias = await _context.Recorrencias
                .Where(r => r.IdUsuario == idUsuario)
                .Take(20)
                .ToListAsync();

            var resumoRecorrencias = recorrencias.Count > 0
                ? string.Join("\n", recorrencias.Select(r =>
                    $"- {r.dsRecorrencia ?? "Sem descricao"}: R$ {r.Valor:N2}"))
                : "Nenhuma recorrencia ativa registrada.";

            var pequenos = lancamentos.Count(l => l.Valor < 100);
            var medios = lancamentos.Count(l => l.Valor >= 100 && l.Valor < 1000);
            var grandes = lancamentos.Count(l => l.Valor >= 1000);

            var apiKey = _configuration.GetValue<string>("DEEPSEEK_API_KEY")
                ?? Environment.GetEnvironmentVariable("DEEPSEEK_API_KEY");

            if (!string.IsNullOrEmpty(apiKey))
            {
                try
                {
                    return await ChamarDeepSeek(apiKey, projecao, totalReceitas, totalDespesas, saldo,
                        resumoMensal, resumoRecorrencias, pequenos, medios, grandes);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"[AnaliseIA] Erro na chamada DeepSeek: {ex.GetType().Name} - {ex.Message}");
                }
            }

            return GerarAnaliseSimulada(projecao, totalReceitas, totalDespesas, saldo,
                lancamentosMensais, recorrencias, pequenos, medios, grandes);
        }

        public async Task<string> ChatComProjecao(int idProjecao, int idUsuario, string pergunta)
        {
            if (string.IsNullOrWhiteSpace(pergunta))
                throw new ArgumentException("Pergunta não pode ser vazia");

            var apiKey = _configuration.GetValue<string>("DEEPSEEK_API_KEY")
                ?? Environment.GetEnvironmentVariable("DEEPSEEK_API_KEY");

            if (string.IsNullOrEmpty(apiKey))
                return "Desculpe, a IA não está disponível no momento (chave de API não configurada).";

            // Pega dados de contexto
            var lancamentos = await _context.Lancamentos
                .Where(l => l.IdUsuario == idUsuario)
                .OrderByDescending(l => l.Data)
                .Take(100)
                .ToListAsync();

            var totalReceitas = lancamentos
                .Where(l => l.TipoLancamento.HasValue && l.TipoLancamento.Value == TipoLancamentoEnum.Receita)
                .Sum(l => l.Valor);

            var totalDespesas = lancamentos
                .Where(l => l.TipoLancamento.HasValue && l.TipoLancamento.Value == TipoLancamentoEnum.Despesa)
                .Sum(l => l.Valor);

            var projecao = await _context.Projecoes
                .FirstOrDefaultAsync(p => p.IdProjecao == idProjecao && p.IdUsuario == idUsuario);

            var contexto = $"Contexto financeiro do usuário:\n" +
                $"- Receitas totais: R$ {totalReceitas:N2}\n" +
                $"- Despesas totais: R$ {totalDespesas:N2}\n" +
                $"- Saldo: R$ {totalReceitas - totalDespesas:N2}\n" +
                $"- Total de lançamentos analisados: {lancamentos.Count}\n" +
                (projecao != null ? $"- Projeção atual: {projecao.Titulo} - R$ {projecao.ValorPrevisto:N2} (ref: {projecao.DataReferencia:dd/MM/yyyy})\n" : "");

            var requestBody = new
            {
                model = "deepseek-chat",
                messages = new[]
                {
                    new { role = "system", content = "Voce e um analista financeiro especializado em financas pessoais e empresariais. Responda em portugues de forma clara e objetiva. Use os dados fornecidos como contexto." },
                    new { role = "user", content = $"{contexto}\n\nPergunta do usuario: {pergunta}" }
                },
                temperature = 0.7,
                max_tokens = 800
            };

            try
            {
                var jsonContent = JsonSerializer.Serialize(requestBody);
                using var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                using var request = new HttpRequestMessage(HttpMethod.Post, "https://api.deepseek.com/v1/chat/completions");
                request.Headers.Add("Authorization", $"Bearer {apiKey}");
                request.Content = httpContent;

                using var response = await _httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    var errorBody = await response.Content.ReadAsStringAsync();
                    Console.Error.WriteLine($"[ChatIA] Erro HTTP {(int)response.StatusCode}: {errorBody}");
                    return "Desculpe, não foi possível obter resposta da IA agora. Tente novamente mais tarde.";
                }

                var responseBody = await response.Content.ReadAsStringAsync();
                var deepSeekResponse = JsonSerializer.Deserialize<DeepSeekResponse>(responseBody,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (deepSeekResponse?.Choices == null || deepSeekResponse.Choices.Length == 0)
                    return "A IA não retornou uma resposta válida.";

                return deepSeekResponse.Choices[0].Message.Content.Trim();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"[ChatIA] Erro: {ex.GetType().Name} - {ex.Message}");
                return $"Erro ao comunicar com a IA: {ex.Message}";
            }
        }

        private async Task<AnaliseIAResult> ChamarDeepSeek(
            string apiKey,
            Projecao projecao,
            decimal totalReceitas, decimal totalDespesas, decimal saldo,
            string resumoMensal, string resumoRecorrencias,
            int pequenos, int medios, int grandes)
        {
            var schemaExemplo = "{\"analise\":\"sua analise detalhada aqui\",\"recomendacao\":\"favoravel\",\"pontosFortes\":[\"item 1\",\"item 2\"],\"pontosAtencao\":[\"item 1\",\"item 2\"]}";

            var prompt = string.Join("\n",
                "Voce e um analista financeiro especializado em financas pessoais e empresariais. Analise a projecao abaixo com base no historico real do usuario.",
                "",
                "## Historico Financeiro do Usuario",
                $"- Lancamentos analisados: {pequenos + medios + grandes} registros",
                $"  - Pequenos (< R$ 100): {pequenos}",
                $"  - Medios (R$ 100 - R$ 1.000): {medios}",
                $"  - Grandes (> R$ 1.000): {grandes}",
                $"- Receitas totais (historico): R$ {totalReceitas:N2}",
                $"- Despesas totais (historico): R$ {totalDespesas:N2}",
                $"- Saldo geral: R$ {saldo:N2}",
                "",
                "## Evolucao Mensal (ultimos 6 meses)",
                resumoMensal,
                "",
                "## Recorrencias Ativas",
                resumoRecorrencias,
                "",
                "## Projecao a ser analisada",
                $"- Titulo: {projecao.Titulo}",
                $"- Valor Previsto: R$ {projecao.ValorPrevisto:N2}",
                $"- Data de Referencia: {projecao.DataReferencia:dd/MM/yyyy}",
                "",
                "## Instrucoes",
                "1. Seja especifico â€” mencione valores reais do historico.",
                "2. Avalie se a projecao e coerente com o padrao de gastos.",
                "3. Considere as recorrencias como compromissos fixos.",
                "4. Destaque riscos reais, nao genericos.",
                "5. A recomendacao deve ser APENAS uma string: 'favoravel' ou 'desfavoravel' ou 'neutra'.",
                "",
                "## FORMATO DE RESPOSTA (OBRIGATORIO)",
                "Voce DEVE responder APENAS com um JSON valido neste formato exato, sem texto antes ou depois, sem marcadores de codigo (```), sem comentarios:",
                "",
                schemaExemplo,
                "",
                "A chave 'analise' deve conter texto corrido em portugues. A chave 'recomendacao' deve ser exatamente uma das tres opcoes. As listas podem ter de 1 a 4 itens cada."
            );

            var requestBody = new
            {
                model = "deepseek-chat",
                messages = new[]
                {
                    new { role = "system", content = "Voce e um analista financeiro. Responda APENAS JSON puro, sem markdown, sem blocos de codigo, sem texto extra." },
                    new { role = "user", content = prompt }
                },
                temperature = 0.7,
                max_tokens = 1000
            };

            var jsonContent = JsonSerializer.Serialize(requestBody);
            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            using var request = new HttpRequestMessage(HttpMethod.Post, "https://api.deepseek.com/v1/chat/completions");
            request.Headers.Add("Authorization", $"Bearer {apiKey}");
            request.Content = httpContent;

            using var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Content.ReadAsStringAsync();
                throw new Exception($"HTTP {(int)response.StatusCode}: {errorBody}");
            }

            var responseBody = await response.Content.ReadAsStringAsync();
            var deepSeekResponse = JsonSerializer.Deserialize<DeepSeekResponse>(responseBody,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (deepSeekResponse?.Choices == null || deepSeekResponse.Choices.Length == 0)
                throw new Exception("Resposta vazia da DeepSeek");

            var content = deepSeekResponse.Choices[0].Message.Content.Trim();

            // Remove blocos markdown ```json ... ``` ou ``` ... ```
            if (content.Contains("```"))
            {
                var match = Regex.Match(content, @"```(?:json)?\s*([\s\S]*?)\s*```");
                if (match.Success)
                    content = match.Groups[1].Value.Trim();
            }

            // Extrai bloco JSON delimitado por { }
            var jsonStart = content.IndexOf('{');
            var jsonEnd = content.LastIndexOf('}');
            if (jsonStart >= 0 && jsonEnd > jsonStart)
            {
                content = content.Substring(jsonStart, jsonEnd - jsonStart + 1);
            }

            // Reparo de JSON malformado
            content = RepairJson(content);

            // Desserializa
            AnaliseIAResult result;
            try
            {
                result = JsonSerializer.Deserialize<AnaliseIAResult>(content,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (JsonException ex)
            {
                var preview = content.Length > 300 ? content[..300] + "..." : content;
                Console.Error.WriteLine($"[AnaliseIA] JSON invalido. Preview: {preview}");
                Console.Error.WriteLine($"[AnaliseIA] Erro: {ex.Message}");
                throw new Exception($"JSON invalido na resposta da DeepSeek: {ex.Message}");
            }

            if (result == null || string.IsNullOrEmpty(result.Analise))
                throw new Exception("JSON invalido: analise vazia ou nula");

            result.FonteIA = true;
            return result;
        }

        private static string RepairJson(string json)
        {
            var pat1 = "\"(\\w+)\"\\s*\\[";
            json = Regex.Replace(json, pat1, "\"$1\": [");

            // Remove trailing comma before ] or }
            json = Regex.Replace(json, ",\\s*\\]", "]");
            json = Regex.Replace(json, ",\\s*\\}", "}");

            // Remove quebras de linha dentro de strings (nao escapadas)
            var inString = false;
            var sb = new StringBuilder();
            for (int i = 0; i < json.Length; i++)
            {
                var c = json[i];
                if (c == '"' && (i == 0 || json[i - 1] != '\\'))
                    inString = !inString;
                if (inString && (c == '\n' || c == '\r'))
                    sb.Append(' ');
                else
                    sb.Append(c);
            }
            return sb.ToString();
        }

        private AnaliseIAResult GerarAnaliseSimulada(
            Projecao projecao,
            decimal totalReceitas, decimal totalDespesas, decimal saldo,
            List<AgregadoMensal> lancamentosMensais,
            List<Recorrencia> recorrencias,
            int pequenos, int medios, int grandes)
        {
            var analise = new List<string>();

            var mesesComDados = lancamentosMensais.Count;
            string tendencia;
            if (mesesComDados >= 2)
            {
                var ultimo = lancamentosMensais[^1];
                var penultimo = lancamentosMensais[^2];
                decimal saldoUltimo = ultimo.Receitas - ultimo.Despesas;
                decimal saldoPenultimo = penultimo.Receitas - penultimo.Despesas;

                if (saldoUltimo > saldoPenultimo * 1.1m)
                    tendencia = "melhora gradual";
                else if (saldoUltimo < saldoPenultimo * 0.9m)
                    tendencia = "piora gradual";
                else
                    tendencia = "estabilidade";
            }
            else
            {
                tendencia = saldo > 0 ? "positiva" : "negativa";
            }

            analise.Add($"Com base nos ultimos {mesesComDados} meses de dados financeiros, a tendencia observada e de {tendencia}.");

            if (recorrencias.Count > 0)
            {
                var totalRecorrencias = recorrencias.Sum(r => r.Valor);
                analise.Add($"O usuario possui {recorrencias.Count} compromisso(s) recorrente(s) ativo(s), totalizando R$ {totalRecorrencias:N2} em obrigacoes periodicas.");
            }

            var totalLancamentos = pequenos + medios + grandes;
            if (totalLancamentos > 0)
            {
                var pctPequenos = (decimal)pequenos / totalLancamentos * 100;
                var pctMedios = (decimal)medios / totalLancamentos * 100;
                var pctGrandes = (decimal)grandes / totalLancamentos * 100;

                analise.Add($"O perfil de gastos mostra {pctPequenos:F0}% de transacoes pequenas, {pctMedios:F0}% medias e {pctGrandes:F0}% grandes. {(pctGrandes > 30 ? "A concentracao em valores altos sugere atencao com o fluxo de caixa." : "A distribuicao indica um perfil equilibrado de despesas.")}");
            }

            var margem = saldo - projecao.ValorPrevisto;
            var razao = saldo > 0 ? projecao.ValorPrevisto / saldo : 99;

            string recomendacao;
            string[] pontosFortes;
            string[] pontosAtencao;

            if (margem >= projecao.ValorPrevisto * 0.5m)
            {
                recomendacao = "favoravel";
                analise.Add($"O saldo atual de R$ {saldo:N2} representa {razao:P0} do valor projetado, com margem de seguranca de R$ {margem:N2}.");
                pontosFortes = new[] {
                    $"Saldo de R$ {saldo:N2} cobre o valor projetado com folga",
                    $"Tendencia de {tendencia} nos ultimos meses",
                    "Margem de seguranca confortavel"
                };
                pontosAtencao = recorrencias.Count > 0
                    ? new[] { "Compromissos recorrentes exigem monitoramento continuo", "Avalie se a data de referencia coincide com outras despesas sazonais" }
                    : new[] { "Monitore o fluxo de caixa ate a data de referencia" };
            }
            else if (margem > 0)
            {
                recomendacao = "neutra";
                analise.Add($"O saldo de R$ {saldo:N2} cobre o valor projetado de R$ {projecao.ValorPrevisto:N2}, mas com margem apertada (R$ {margem:N2}).");
                pontosFortes = new[] {
                    "Saldo positivo demonstra organizacao financeira",
                    "Projecao esta dentro de um patamar factivel"
                };
                pontosAtencao = new[] {
                    $"Margem de seguranca de apenas R$ {margem:N2}",
                    "Qualquer despesa inesperada pode comprometer",
                    recorrencias.Count > 0 ? "Recorrencias ativas consomem parte do orcamento mensal" : "Verifique despesas sazonais proximas"
                };
            }
            else
            {
                recomendacao = "desfavoravel";
                analise.Add($"Com saldo de R$ {saldo:N2} e projecao de R$ {projecao.ValorPrevisto:N2}, o deficit e de R$ {Math.Abs(margem):N2}.");
                pontosFortes = new[] {
                    "Registrar a projecao ajuda no planejamento de longo prazo",
                    "A consciencia da situacao financeira e o primeiro passo"
                };
                pontosAtencao = new[] {
                    $"Deficit de R$ {Math.Abs(margem):N2} em relacao ao saldo atual",
                    "Risco de comprometer outras obrigacoes financeiras",
                    "Considere aumentar receitas ou reduzir despesas primeiro"
                };
            }

            return new AnaliseIAResult
            {
                Analise = string.Join("\n\n", analise),
                Recomendacao = recomendacao,
                PontosFortes = pontosFortes,
                PontosAtencao = pontosAtencao,
                FonteIA = false
            };
        }
    }

    public class DeepSeekResponse
    {
        public DeepSeekChoice[] Choices { get; set; } = Array.Empty<DeepSeekChoice>();
    }

    public class DeepSeekChoice
    {
        public DeepSeekMessage Message { get; set; } = new();
    }

    public class DeepSeekMessage
    {
        public string Content { get; set; } = "";
    }

    public class AnaliseIAResult
    {
        public string Analise { get; set; } = "";
        public string Recomendacao { get; set; } = "neutra";
        public string[] PontosFortes { get; set; } = Array.Empty<string>();
        public string[] PontosAtencao { get; set; } = Array.Empty<string>();
        public bool FonteIA { get; set; } = false;
    }

    public class AgregadoMensal
    {
        public string Periodo { get; set; } = "";
        public decimal Receitas { get; set; }
        public decimal Despesas { get; set; }
    }
}

