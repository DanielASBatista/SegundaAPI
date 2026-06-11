using ProjetoMidasAPI.Data;
using ProjetoMidasAPI.Models.Enuns;
using ProjetoMidasAPI.DTOs.Recorrencia;
using Microsoft.EntityFrameworkCore;
using ProjetoMidasAPI.Dtos.Lancamentos;


namespace ProjetoMidasAPI.Services
{
    public class LancamentosService
    {
        private readonly AppDbContext _context;

        public LancamentosService(AppDbContext context) 
        {
             _context = context;
        }

        private static void ValidarRecorrencia (RecorrenciaDto qtdeRecorrencia)
        {
            if (qtdeRecorrencia.FrequenciaRecorrencia == FrequenciaRecorrencia.Mensal)
            {
                if(!qtdeRecorrencia.ModoRecorrenciaMensal.HasValue)
                {
                    throw new ArgumentException("Modo de recorrência mensal deve ser preenchido para lançamentos com frequência mensal.");
                }

                if(qtdeRecorrencia.ModoRecorrenciaMensal == ModoRecorrenciaMensal.Dia_Fixo && !qtdeRecorrencia.DataRecorrencia.HasValue)
                {
                    throw new ArgumentException("Data deve ser preenchida para lançamentos com modo de recorrência mensal diferente de dia fixo.");
                }

                if(qtdeRecorrencia.ModoRecorrenciaMensal == ModoRecorrenciaMensal.Intervalo_Dias && !qtdeRecorrencia.DiasIntervalo.HasValue)
                {
                    throw new ArgumentException("Dias de intervalo deve ser preenchido para lançamentos com modo de recorrência mensal diferente de intervalo de dias.");
                }
            }
        }

        public async Task<Lancamento?> GetById(int id, int idUsuario)
        {
            return await _context.Lancamentos
                 .FirstOrDefaultAsync(l => l.IdLancamento == id 
                                        && l.IdUsuario == idUsuario);   
        }

        public async Task<List<Lancamento>> CriarRecorrenciaAsync(NovoLancamentoDto novoLancamentoDto, int idUsuario)

        {
            if (novoLancamentoDto.Recorrencia is null) 
            throw new ArgumentException("Recorrência deve ser preenchida para criar lançamentos recorrentes.");

            ValidarRecorrencia(novoLancamentoDto.Recorrencia);

            var total = novoLancamentoDto.Recorrencia.QtdeRecorrencia;

            var recorrencia = new Recorrencia
            {
                IdUsuario = idUsuario,
                TipoLancamento = novoLancamentoDto.TipoLancamento,
                dsRecorrencia = novoLancamentoDto.DescricaoLancamento,
                dataInicio = novoLancamentoDto.Data,
                qtdeRecorrencia = total,
                Valor = novoLancamentoDto.Valor,
                momentoCriacao = DateTime.UtcNow
            };

            _context.Recorrencias.Add(recorrencia);
            await _context.SaveChangesAsync();

            var lancamentos = new List<Lancamento>();
            
            for (int i = 0; i < total; i++)
            {
                lancamentos.Add(new Lancamento
                {
                    IdUsuario = idUsuario,
                    IdRecorrencia = recorrencia.IdRecorrencia,
                    DescricaoLancamento = novoLancamentoDto.DescricaoLancamento,
                    TipoLancamento = novoLancamentoDto.TipoLancamento, // Exemplo, pode ser definido dinamicamente com base no DTO
                    Valor = novoLancamentoDto.Valor,
                    Data = novoLancamentoDto.Data.AddMonths(i), // Exemplo de incremento de data para recorrência mensal, pode ser ajustado conforme a lógica de recorrência
                    StatusTransacao = novoLancamentoDto.StatusTransacao,
                    NumeroDaOcorrencia = i + 1,
                    TotalOcorrencia = total,
                    DataCriacao = DateTime.UtcNow           
                });
            }

            _context.Lancamentos.AddRange(lancamentos);
            await _context.SaveChangesAsync();

            return lancamentos;
        }
        
        public async Task<List<Lancamento>> Post(NovoLancamentoDto novoLancamento, int idUsuario)
        {
            if(novoLancamento.Recorrencia != null) 
            
            {
               return await CriarRecorrenciaAsync(novoLancamento, idUsuario);
            }

            var lancamento = new Lancamento
            {
                IdUsuario = idUsuario,
                DescricaoLancamento = novoLancamento.DescricaoLancamento,
                TipoLancamento = novoLancamento.TipoLancamento,
                Valor = novoLancamento.Valor,
                Data = novoLancamento.Data,
                StatusTransacao = novoLancamento.StatusTransacao,
                DataCriacao = DateTime.UtcNow
            };

            _context.Lancamentos.Add(lancamento);
            await _context.SaveChangesAsync();

            return new List<Lancamento> { lancamento };
        } 
    }
}