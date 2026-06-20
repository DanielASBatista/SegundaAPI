using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoMidasAPI.Data;
using ProjetoMidasAPI.Services;
using System.Security.Claims;
using MidasApi.Models.Enuns;
using ProjetoMidasAPI.Dtos.Lancamentos;
using ProjetoMidasAPI.Models.Enuns;


namespace ProjetoMidasAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class LancamentosController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly LancamentosService _lancamentosService;
        public LancamentosController(AppDbContext context, LancamentosService lancamentosService)
        {
            _context = context;
            _lancamentosService = lancamentosService;
        }

        private int GetUserId() =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        private async Task<int?> GetUserEmpresaId()
        {
            var user = await _context.Usuarios.FindAsync(GetUserId());
            return user?.IdEmpresa > 0 ? user.IdEmpresa : null;
        }

        // READ - Lista todos os lançamentos (DEBUG: ignorando filtro)
        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<Lancamento>>> GetAll()
        {
            var idUsuario = GetUserId();
            System.Console.WriteLine($"[DEBUG GetAll] IdUsuario={idUsuario}");

            var lancamentos = await _context.Lancamentos
                .Where(l => l.IdUsuario == idUsuario)
                .ToListAsync();

            System.Console.WriteLine($"[DEBUG GetAll] Encontrou {lancamentos.Count} registros");
            return lancamentos;
        }

        // READ - Busca por ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Lancamento>> GetById(int id)
        {
            var idUsuario = GetUserId();

            var lancamento = await _context.Lancamentos
                .FirstOrDefaultAsync(l => l.IdLancamento == id 
                                       && l.IdUsuario == idUsuario);

            return lancamento == null ? NotFound() : lancamento;
        }

        // CREATE - Adiciona novo lançamento
        [HttpPost("New")]
        public async Task<ActionResult<List<Lancamento>>> Post(NovoLancamentoDto novoLancamento)
        
        {
            var idUsuario = GetUserId();

            var lancamentosCriados = await _lancamentosService.Post(novoLancamento, idUsuario);

            if (lancamentosCriados == null || lancamentosCriados.Count == 0)
                return BadRequest("Não foi possível criar o lançamento.");  

            return CreatedAtAction(nameof(GetById), new { id = lancamentosCriados.First().IdLancamento }, lancamentosCriados);
        }

        // UPDATE (parcial) - Atualiza campos específicos do lançamento
        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(int id, [FromBody] ProjetoMidasAPI.Dtos.Lancamentos.PatchLancamentoDto patch)
        {
            if (patch == null)
                return BadRequest();

            var idUsuario = GetUserId();

            var lancamento = await _context.Lancamentos
                .FirstOrDefaultAsync(l => l.IdLancamento == id
                                       && l.IdUsuario == idUsuario);

            if (lancamento == null)
                return NotFound();

            // Atualiza apenas os campos que vieram preenchidos
            if (patch.DescricaoLancamento != null)
                lancamento.DescricaoLancamento = patch.DescricaoLancamento;

            if (patch.Valor.HasValue)
                lancamento.Valor = patch.Valor.Value;

            if (patch.TipoLancamento.HasValue)
                lancamento.TipoLancamento = patch.TipoLancamento.Value == 0 ? TipoLancamentoEnum.Receita : TipoLancamentoEnum.Despesa;

            if (patch.Data.HasValue)
                lancamento.Data = patch.Data.Value;

            if (patch.CategoriaGasto.HasValue)
                lancamento.CategoriaGasto = (CategoriaGastoEnum)patch.CategoriaGasto.Value;
            else if (patch.CategoriaGasto == null && patch.GetType().GetProperty(nameof(patch.CategoriaGasto))?.GetValue(patch) == null)
            {
                // null = não alterar, mas se explicitamente null, quer remover
                // (PatchLancamentoDto usa Nullable<int?>, então null padrão = não alterar)
            }

            if (patch.ObservacaoLancamento != null)
                lancamento.ObservacaoLancamento = patch.ObservacaoLancamento;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // UPDATE - Atualiza lançamento existente (completo)
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Lancamento lancamento)
        {
            var idUsuario = GetUserId();

            if (id != lancamento.IdLancamento)
                return BadRequest();

            var lancamentoExistente = await _context.Lancamentos
                .FirstOrDefaultAsync(l => l.IdLancamento == id 
                                       && l.IdUsuario == idUsuario);

            if (lancamentoExistente == null)
                return NotFound();

            // Atualiza apenas os campos permitidos
            lancamentoExistente.Valor = lancamento.Valor;
            lancamentoExistente.Data = lancamento.Data;
            lancamentoExistente.DescricaoLancamento = lancamento.DescricaoLancamento;
            lancamentoExistente.CategoriaGasto = lancamento.CategoriaGasto;
            lancamentoExistente.TipoLancamento = lancamento.TipoLancamento;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE - Remove lançamento
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var idUsuario = GetUserId();

            var lancamento = await _context.Lancamentos
                .FirstOrDefaultAsync(l => l.IdLancamento == id 
                                       && l.IdUsuario == idUsuario);

            if (lancamento == null)
                return NotFound();

            _context.Lancamentos.Remove(lancamento);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Busca por Data
        [HttpGet("DataReferencia/{data}")]
        public async Task<ActionResult<IEnumerable<Lancamento>>> GetByData(DateTime data)
        {
            var idUsuario = GetUserId();

            return await _context.Lancamentos
                .Where(l => l.IdUsuario == idUsuario &&
                            l.Data.Date == data.Date)
                .ToListAsync();
        }

        // Busca por DataCriacao
        [HttpGet("DataCriacao/{data}")]
        public async Task<ActionResult<IEnumerable<Lancamento>>> GetByDataCriacao(DateTime data)
        {
            var idUsuario = GetUserId();

            return await _context.Lancamentos
                .Where(l => l.IdUsuario == idUsuario &&
                            l.DataCriacao.Date == data.Date)
                .ToListAsync();
        }

        // Busca por Valor
        [HttpGet("valor/{valor}")]
        public async Task<ActionResult<IEnumerable<Lancamento>>> GetByValor(decimal valor)
        {
            var idUsuario = GetUserId();

            return await _context.Lancamentos
                .Where(l => l.IdUsuario == idUsuario &&
                            l.Valor == valor)
                .ToListAsync();
        }

        // Busca por Ano
        [HttpGet("ano/{ano}")]
        public async Task<ActionResult<IEnumerable<Lancamento>>> GetByAno(int ano)
        {
            var idUsuario = GetUserId();

            return await _context.Lancamentos
                .Where(l => l.IdUsuario == idUsuario &&
                            l.Data.Year == ano)
                .ToListAsync();
        }

        // Busca por Ano/Mês    
        [HttpGet("mes/{ano}/{mes}")]
        public async Task<ActionResult<IEnumerable<Lancamento>>> GetByMes(int ano, int mes)
        {
            var idUsuario = GetUserId();

            return await _context.Lancamentos
                .Where(l => l.IdUsuario == idUsuario &&
                            l.Data.Year == ano &&
                            l.Data.Month == mes)
                .ToListAsync();
        }

        // Busca por Ano/Mês/Dia    
        [HttpGet("dia/{ano}/{mes}/{dia}")]
        public async Task<ActionResult<IEnumerable<Lancamento>>> GetByDia(int ano, int mes, int dia)
        {
            var idUsuario = GetUserId();

            return await _context.Lancamentos
                .Where(l => l.IdUsuario == idUsuario &&
                            l.Data.Year == ano &&
                            l.Data.Month == mes &&
                            l.Data.Day == dia)
                .ToListAsync();
        }

        // Somatória de valores (com receitas/despesas/saldo)
        [HttpGet("somatoria")]
        public async Task<ActionResult<object>> GetSomatoria()
        {
            var idUsuario = GetUserId();

            var lancamentos = await _context.Lancamentos
                .Where(l => l.IdUsuario == idUsuario)
                .ToListAsync();

            var totalReceitas = lancamentos
                .Where(l => l.TipoLancamento.HasValue && l.TipoLancamento.Value == TipoLancamentoEnum.Receita)
                .Sum(l => l.Valor);
            var totalDespesas = lancamentos
                .Where(l => l.TipoLancamento.HasValue && l.TipoLancamento.Value == TipoLancamentoEnum.Despesa)
                .Sum(l => l.Valor);

            return Ok(new
            {
                totalReceitas,
                totalDespesas,
                saldo = totalReceitas - totalDespesas
            });
        }

        // READ - Lista todos os lançamentos da empresa (exclui lançamentos de Admins)
        [HttpGet("Empresa")]
        public async Task<ActionResult<IEnumerable<Lancamento>>> GetAllEmpresa()
        {
            var empresaId = await GetUserEmpresaId();

            if (empresaId == null)
                return Ok(Array.Empty<Lancamento>());

            var usuariosEmpresa = await _context.Usuarios
                .Where(u => u.IdEmpresa == empresaId.Value
                         && u.Perfil != null
                         && u.Perfil != "Administrador")
                .Select(u => u.IdUsuario)
                .ToListAsync();

            return await _context.Lancamentos
                .Where(l => usuariosEmpresa.Contains(l.IdUsuario))
                .ToListAsync();
        }

        // READ - Somatória de valores da empresa (exclui Admins)
        [HttpGet("Empresa/Somatoria")]
        public async Task<ActionResult<object>> GetSomatoriaEmpresa()
        {
            var empresaId = await GetUserEmpresaId();

            if (empresaId == null)
                return Ok(new { totalReceitas = 0m, totalDespesas = 0m, saldo = 0m });

            var idsUsuarios = await _context.Usuarios
                .Where(u => u.IdEmpresa == empresaId.Value
                         && u.Perfil != null
                         && u.Perfil != "Administrador")
                .Select(u => u.IdUsuario)
                .ToListAsync();

            var lancamentos = await _context.Lancamentos
                .Where(l => idsUsuarios.Contains(l.IdUsuario))
                .ToListAsync();

            var totalReceitas = lancamentos
                .Where(l => l.TipoLancamento.HasValue && l.TipoLancamento.Value == TipoLancamentoEnum.Receita)
                .Sum(l => l.Valor);
            var totalDespesas = lancamentos
                .Where(l => l.TipoLancamento.HasValue && l.TipoLancamento.Value == TipoLancamentoEnum.Despesa)
                .Sum(l => l.Valor);

            return Ok(new
            {
                totalReceitas,
                totalDespesas,
                saldo = totalReceitas - totalDespesas
            });
        }

        // IMPORTAR EM LOTE - Recebe array de lançamentos e salva todos
        [HttpPost("Importar")]
        public async Task<ActionResult<object>> Importar([FromBody] ImportarLancamentoRequestDto request)
        {
            if (request == null || request.Lancamentos == null || request.Lancamentos.Count == 0)
                return BadRequest(new { importados = 0, mensagem = "Nenhum lançamento para importar." });

            var idUsuario = GetUserId();
            var dataCriacao = DateTime.UtcNow;
            var entidades = new List<Lancamento>();

            System.Console.WriteLine($"[DEBUG Importar] IdUsuario={idUsuario}, {request.Lancamentos.Count} itens recebidos");

            foreach (var item in request.Lancamentos)
            {
                if (string.IsNullOrWhiteSpace(item.DescricaoLancamento))
                    continue;

                var descricao = item.DescricaoLancamento;
                if (descricao.Length > 120)
                    descricao = descricao[..120];

                var tipo = item.TipoLancamento == 0 ? TipoLancamentoEnum.Receita : TipoLancamentoEnum.Despesa;

                entidades.Add(new Lancamento
                {
                    IdUsuario = idUsuario,
                    TipoLancamento = tipo,
                    DescricaoLancamento = descricao,
                    Valor = item.Valor,
                    Data = item.Data,
                    DataCriacao = dataCriacao,
                    CategoriaGasto = item.CategoriaGasto.HasValue ? (CategoriaGastoEnum)item.CategoriaGasto.Value : null,
                    StatusTransacao = MidasApi.Models.Enuns.StatusTransacao.Confirmada,
                });
            }

            if (entidades.Count == 0)
                return BadRequest(new { importados = 0, mensagem = "Nenhum lançamento válido para importar." });

            _context.Lancamentos.AddRange(entidades);
            await _context.SaveChangesAsync();

            return Ok(new { importados = entidades.Count });
        }

        // Comparação (maior que valor informado)
        [HttpGet("comparacao/{valor}")]
        public async Task<ActionResult<IEnumerable<Lancamento>>> GetComparacao(decimal valor)
        {
            var idUsuario = GetUserId();

            return await _context.Lancamentos
                .Where(l => l.IdUsuario == idUsuario &&
                            l.Valor > valor)
                .ToListAsync();
        }
    }
}