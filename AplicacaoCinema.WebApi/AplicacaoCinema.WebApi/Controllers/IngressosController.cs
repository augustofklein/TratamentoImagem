using AplicacaoCinema.WebApi.Dominio;
using AplicacaoCinema.WebApi.Infraestrutura;
using AplicacaoCinema.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AplicacaoCinema.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IngressosController : ControllerBase
    {
        private readonly IngressosRepositorio _ingressosRepositorio;

        public IngressosController(IngressosRepositorio ingressosRepositorio)
        {
            _ingressosRepositorio = ingressosRepositorio;
        }

        [HttpPost]
        public async Task<IActionResult> CadastrarAsync([FromBody] NovoIngressoInputModel ingressoInputModel, CancellationToken cancellationToken)
        {
            if (!Guid.TryParse(ingressoInputModel.SessaoId, out var _sessaoId))
                return BadRequest("Id da sessão invalido");

            if (ingressoInputModel.QuantidadeIngressos <= 0)
                return BadRequest("Quantidade de ingressos inválida");

            var ingressos = await _ingressosRepositorio.RecuperarIngressosSessao(_sessaoId, cancellationToken);

            var ingresso = Ingresso.Criar(_sessaoId, ingressoInputModel.QuantidadeIngressos);

            await _ingressosRepositorio.InserirAsync(ingresso.Value, cancellationToken);
            await _ingressosRepositorio.CommitAsync(cancellationToken);

            return CreatedAtAction("RecuperarPorId", new { id = ingresso.Value.Id }, ingresso.Value.Id);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> RecuperarPorIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var ingresso = await _ingressosRepositorio.RecuperarPorIdAsync(id, cancellationToken);

            if (ingresso == null)
                return NotFound();

            return Ok(ingresso);
        }

        [HttpGet()]
        public async Task<IActionResult> RecuperarTodosAsync(CancellationToken cancellationToken)
        {
            var ingresso = await _ingressosRepositorio.RecuperarTodosAsync(cancellationToken);
            return Ok(ingresso);
        }
    }
}
