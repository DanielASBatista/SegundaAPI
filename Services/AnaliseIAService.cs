using Microsoft.EntityFrameworkCore;
using ProjetoMidasAPI.Data;
using ProjetoMidasAPI.Models;
using System.Text;
using System.Text.Json;

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
                throw new KeyNotFoundException("Projeção não encontrada");

            // Busca dados financeiros do usuário para análise contextual
            var lancamentos = await _context.Lancamentos
                .Where(l => l.IdUsuario == idUsuario)
                .OrderByDescending(l => l.Data)
                .Take(50)
                .ToListAsync();

            var totalReceitas = lancamentos
                .Where(l => l.TipoLancamento.HasValue && (int)l.TipoLancamento == 0)
                .Sum(l => l.Valor);

            var totalDespesas = lancamentos
                .Where(l => l.TipoLancamento.HasValue && (int)l.TipoLancamento == 1)
                .Sum(l => l.Valor);

            var saldo = totalReceitas - totalDespesas;

            // Monta o prompt
            var prompt = $@"Você é um analista financeiro especializado em finanças pessoais e empresariais.

## Dados do Histórico do Usuário
- Total de lançamentos registrados: {lancamentos.Count}
- Receitas totais (histórico): R$ {totalReceitas:N2}
- Despesas totais (histórico): R$ {totalDespesas:N2}
- Saldo atual: R$ {saldo:N2}

## Projeção a ser analisada
- Título: {projecao.Titulo}
- Valor Previsto: R$ {projecao.ValorPrevisto:N2}
- Data de Referência: {projecao.DataReferencia:dd/MM/yyyy}

## Sua análise deve conter:
1. **Análise detalhada** (2-3 parágrafos) — avalie se a projeção é realista, se o usuário tem capacidade financeira para realizá-la com base no histórico, e destaque riscos e oportunidades.
2. **Recomendação** — uma das opções: 'favoravel', 'desfavoravel' ou 'neutra'
3. **Pontos fortes** (lista de 1-3 itens)
4. **Pontos de atenção** (lista de 1-3 itens)

Responda EXCLUSIVAMENTE em JSON, sem markdown, sem formatação extra, seguindo este schema:
{{
  ""analise"": ""texto da análise"",
  ""recomendacao"": ""favoravel ou desfavoravel ou neutra"",
  ""pontosFortes"": [""item1"", ""item2""],
  ""pontosAtencao"": [""item1"", ""item2""]
}}";

            try
            {
                var apiKey = _configuration.GetValue<string>("DEEPSEEK_API_KEY")
                    ?? Environment.GetEnvironmentVariable("DEEPSEEK_API_KEY")
                    ?? "";

                if (string.IsNullOrEmpty(apiKey))
                {
                    Console.Error.WriteLine("DeepSeek API key não configurada, usando fallback");
                    return GerarAnaliseSimulada(projecao, totalReceitas, totalDespesas, saldo);
                }

                var requestBody = new
                {
                    model = "deepseek-chat",
                    messages = new[]
                    {
                        new { role = "user", content = prompt }
                    },
                    temperature = 0.7,
                    max_tokens = 1000
                };

                var jsonContent = JsonSerializer.Serialize(requestBody);
                var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var request = new HttpRequestMessage(HttpMethod.Post, "https://api.deepseek.com/v1/chat/completions");
                request.Headers.Add("Authorization", $"Bearer {apiKey}");
                request.Content = httpContent;

                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync();
                var deepSeekResponse = JsonSerializer.Deserialize<DeepSeekResponse>(responseBody, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (deepSeekResponse?.Choices == null || deepSeekResponse.Choices.Length == 0)
                    return GerarAnaliseSimulada(projecao, totalReceitas, totalDespesas, saldo);

                var content = deepSeekResponse.Choices[0].Message.Content;

                // Tenta parsear como JSON
                try
                {
                    var result = JsonSerializer.Deserialize<AnaliseIAResult>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (result != null && !string.IsNullOrEmpty(result.Analise))
                        return result;
                }
                catch
                {
                    // Se não conseguir parsear, usa fallback
                }

                return GerarAnaliseSimulada(projecao, totalReceitas, totalDespesas, saldo);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Erro na análise de IA: {ex.Message}");
                return GerarAnaliseSimulada(projecao, totalReceitas, totalDespesas, saldo);
            }
        }

        private AnaliseIAResult GerarAnaliseSimulada(Projecao projecao, decimal totalReceitas, decimal totalDespesas, decimal saldo)
        {
            var analise = new List<string>();

            if (projecao.ValorPrevisto <= 0)
            {
                analise.Add("A projeção informada possui valor zerado ou negativo, o que sugere que pode ser apenas um registro preliminar.");
                analise.Add("Recomendamos revisar o valor antes de tomar qualquer decisão baseada nessa projeção.");
                return new AnaliseIAResult
                {
                    Analise = string.Join("\n\n", analise),
                    Recomendacao = "neutra",
                    PontosFortes = new[] { "Registro criado — já é um primeiro passo" },
                    PontosAtencao = new[] { "Valor precisa ser definido", "Sem dados suficientes para análise precisa" }
                };
            }

            string recomendacao;
            string[] pontosFortes;
            string[] pontosAtencao;

            if (saldo >= projecao.ValorPrevisto * 1.5m)
            {
                recomendacao = "favoravel";
                analise.Add($"Com base no histórico financeiro, o saldo atual de R$ {saldo:N2} é suficiente para cobrir mais de 1,5x o valor projetado de R$ {projecao.ValorPrevisto:N2}.");
                analise.Add($"Isso indica uma boa saúde financeira e baixo risco para a realização desta projeção. Recomendamos prosseguir com o planejamento.");
                pontosFortes = new[] {
                    $"Saldo atual (R$ {saldo:N2}) supera o valor projetado com folga",
                    "Capacidade financeira demonstrada pelo histórico de receitas"
                };
                pontosAtencao = new[] {
                    "Monitore o fluxo de caixa para garantir que o saldo se mantenha até a data de referência",
                    "Considere imprevistos que possam impactar o orçamento"
                };
            }
            else if (saldo > 0 && saldo < projecao.ValorPrevisto * 1.5m)
            {
                recomendacao = "neutra";
                analise.Add($"O saldo atual de R$ {saldo:N2} cobre parcialmente o valor projetado de R$ {projecao.ValorPrevisto:N2}, mas sem grande margem de segurança.");
                analise.Add($"É viável, mas requer planejamento cuidadoso. Considere ajustar o valor ou o prazo para aumentar a margem de segurança.");
                pontosFortes = new[] {
                    "Saldo positivo demonstra organização financeira",
                    "Projeção está dentro de um patamar factível"
                };
                pontosAtencao = new[] {
                    "Margem de segurança apertada — qualquer imprevisto pode comprometer",
                    "Avalie se há despesas sazonais próximas à data de referência"
                };
            }
            else
            {
                recomendacao = "desfavoravel";
                analise.Add($"O saldo atual é negativo ou insuficiente (R$ {saldo:N2}) para cobrir o valor projetado de R$ {projecao.ValorPrevisto:N2}.");
                analise.Add($"Recomendamos revisar a projeção para um valor mais alinhado com a realidade financeira atual, ou criar um plano de redução de despesas antes de assumir esse compromisso.");
                pontosFortes = new[] {
                    "Ter clareza da situação financeira já é um ponto positivo",
                    "Registrar a projeção ajuda no planejamento futuro"
                };
                pontosAtencao = new[] {
                    "Saldo insuficiente para cobrir o valor projetado",
                    "Risco de endividamento se a projeção for executada sem planejamento",
                    "Considere aumentar receitas ou reduzir despesas antes"
                };
            }

            return new AnaliseIAResult
            {
                Analise = string.Join("\n\n", analise),
                Recomendacao = recomendacao,
                PontosFortes = pontosFortes,
                PontosAtencao = pontosAtencao
            };
        }
    }

    // Modelo pra resposta da DeepSeek (compatível com API da OpenAI)
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
    }
}
