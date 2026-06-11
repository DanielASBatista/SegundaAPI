//A criação dessa controller se fez necessária pq criou a classe de recorrência ai já viu, né
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoMidasAPI.Data;
using ProjetoMidasAPI.Models;

[Authorize]
[ApiController]
[Route("[controller]")]
public class TipoRecorrenciaController : ControllerBase
{
    private readonly AppDbContext _context;

    public TipoRecorrenciaController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("GetAll")]
    public async Task<ActionResult<IEnumerable<TipoRecorrencia>>> Get()
    {
        return await _context.TipoRecorrencias
            .OrderBy(t => t.PadraoSistema ? 0 : 1)
            .ThenBy(t => t.Nome)
            .ToListAsync();
    }
    [HttpPost("New")]
    public async Task<ActionResult<TipoRecorrencia>> Post(TipoRecorrencia tipo)
    {
        tipo.Id = 0;
        tipo.PadraoSistema = false;

        _context.TipoRecorrencias.Add(tipo);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(Get), new { id = tipo.Id }, tipo);
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var tipo = await _context.TipoRecorrencias.FindAsync(id);

        if (tipo == null)
            return NotFound();

        if (tipo.PadraoSistema)
            return BadRequest("Não é permitido excluir tipos padrão do sistema.");

        _context.TipoRecorrencias.Remove(tipo);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
