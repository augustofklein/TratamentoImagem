using AplicacaoAnuncio.Dominio;
using AplicacaoAnuncio.Infraestrutura;
using AplicacaoAnuncio.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AplicacaoAnuncio.Controllers
{
    [DisableCors]
    [Route("api/[controller]")]
    [ApiController]
    public class AvaliacoesController : ControllerBase
    {
        private readonly AvaliacoesRepositorio _avaliacoesRepositorio;

        public AvaliacoesController(AvaliacoesRepositorio avaliacoesRepositorio)
        {
            _avaliacoesRepositorio = avaliacoesRepositorio;
        }

        [EnableCors("AllowOrigin")]
        [HttpPost]
        public async Task<IActionResult> CadastrarAsync([FromBody] NovaAvaliacaoInputModel avaliacaoInputModel, CancellationToken cancellationToken)
        {
            if (!Guid.TryParse(avaliacaoInputModel.ServicoId, out var _servicoId))
                return BadRequest("Id do servico inválido");

            if (avaliacaoInputModel.Nota < 0)
                return BadRequest("A avaliação precisa ser maior ou igual a zero");

            var avaliacao = Avaliacao.Criar(_servicoId, avaliacaoInputModel.Nota);

            if (avaliacao.IsFailure)
                return BadRequest(avaliacao.Error);

            await _avaliacoesRepositorio.InserirAsync(avaliacao.Value, cancellationToken);
            await _avaliacoesRepositorio.CommitAsync(cancellationToken);

            return CreatedAtAction("RecuperarPorId", new { id = avaliacao.Value.Id }, avaliacao.Value.Id);
        }

        [EnableCors("AllowOrigin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> RecuperarPorIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var avaliacao = await _avaliacoesRepositorio.RecuperarPorIdAsync(id, cancellationToken);

            if (avaliacao == null)
                return NotFound();

            return Ok(avaliacao);
        }

        [EnableCors("AllowOrigin")]
        [HttpGet()]
        public async Task<IActionResult> RecuperarTodosAsync(CancellationToken cancellationToken)
        {
            var avaliacoes = await _avaliacoesRepositorio.RecuperarTodosAsync(cancellationToken);
            return Ok(avaliacoes);
        }

        [EnableCors("AllowOrigin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] NovaAvaliacaoInputModel avaliacaoInputModel, CancellationToken cancellationToken)
        {
            var avaliacao = await _avaliacoesRepositorio.RecuperarPorIdAsync(id, cancellationToken);

            avaliacao.Nota = avaliacaoInputModel.Nota;

            await _avaliacoesRepositorio.UpdateAsync(cancellationToken);

            return Ok(avaliacao);
        }
        
        [EnableCors("AllowOrigin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var avaliacao = await _avaliacoesRepositorio.RecuperarPorIdAsync(id, cancellationToken);

            await _avaliacoesRepositorio.DeleteAsync(id, cancellationToken);

            return Ok(avaliacao);
        }
    }
}
