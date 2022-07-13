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
    public class PagamentosController : ControllerBase
    {
        private readonly PagamentosRepositorio _pagamentosRepositorio;

        public PagamentosController(PagamentosRepositorio pagamentosRepositorio)
        {
            _pagamentosRepositorio = pagamentosRepositorio;
        }

        [EnableCors("AllowOrigin")]
        [HttpPost]
        public async Task<IActionResult> CadastrarAsync([FromBody] NovoPagamentoInputModel pagamentoInputModel, CancellationToken cancellationToken)
        {
            if (!Guid.TryParse(pagamentoInputModel.ServicoId, out var _servicoId))
                return BadRequest("Id do servico inválido");

            if (pagamentoInputModel.ValorParcela <= 0)
                return BadRequest("O valor da parcela precisa ser maior que zero");

            var pagamento = Pagamento.Criar(_servicoId,
                                            pagamentoInputModel.TipoPagamento,
                                            pagamentoInputModel.QuantidadeParcelas,
                                            pagamentoInputModel.ValorParcela);

            if (pagamento.IsFailure)
                return BadRequest(pagamento.Error);

            await _pagamentosRepositorio.InserirAsync(pagamento.Value, cancellationToken);
            await _pagamentosRepositorio.CommitAsync(cancellationToken);

            return CreatedAtAction("RecuperarPorId", new { id = pagamento.Value.Id }, pagamento.Value.Id);
        }

        [EnableCors("AllowOrigin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> RecuperarPorIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var pagamento = await _pagamentosRepositorio.RecuperarPorIdAsync(id, cancellationToken);

            if (pagamento == null)
                return NotFound();

            return Ok(pagamento);
        }

        [EnableCors("AllowOrigin")]
        [HttpGet()]
        public async Task<IActionResult> RecuperarTodosAsync(CancellationToken cancellationToken)
        {
            var pagamentos = await _pagamentosRepositorio.RecuperarTodosAsync(cancellationToken);
            return Ok(pagamentos);
        }

        [EnableCors("AllowOrigin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] NovoPagamentoInputModel pagamentoInputModel, CancellationToken cancellationToken)
        {
            var pagamento = await _pagamentosRepositorio.RecuperarPorIdAsync(id, cancellationToken);

            pagamento.TipoPagamento = pagamentoInputModel.TipoPagamento;
            pagamento.QuantidadeParcelas = pagamentoInputModel.QuantidadeParcelas;
            pagamento.ValorParcela = pagamentoInputModel.ValorParcela;

            await _pagamentosRepositorio.UpdateAsync(cancellationToken);

            return Ok(pagamento);
        }

        [EnableCors("AllowOrigin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var pagamento = await _pagamentosRepositorio.RecuperarPorIdAsync(id, cancellationToken);

            await _pagamentosRepositorio.DeleteAsync(id, cancellationToken);

            return Ok(pagamento);
        }
    }
}
