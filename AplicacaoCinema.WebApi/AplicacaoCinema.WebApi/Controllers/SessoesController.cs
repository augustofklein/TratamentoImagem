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
    public class SessoesController : ControllerBase
    {
        private readonly SessoesRepositorio _sessoesRepositorio;

        public SessoesController(SessoesRepositorio sessoesRepositorio)
        {
            _sessoesRepositorio = sessoesRepositorio;
        }

        [HttpPost]
        public async Task<IActionResult> CadastrarAsync([FromBody] NovaSessaoInputModel sessaoInputModel, CancellationToken cancellationToken)
        {
            var _horarioInicial = Horario.Criar(sessaoInputModel.HorarioInicial);
            if (_horarioInicial.IsFailure)
                return BadRequest(_horarioInicial.Error);

            if (!Guid.TryParse(sessaoInputModel.FilmeId, out var _filmeId))
                return BadRequest("Id de filme inválido");

            if (sessaoInputModel.TotalIngressos <= 0)
                return BadRequest("Quantidade de ingressos inválida");

            var sessao = Sessao.Criar(_filmeId, (EDiaSemana)sessaoInputModel.DiaSemana, _horarioInicial.Value, sessaoInputModel.Preco, sessaoInputModel.TotalIngressos) ;

            if (sessao.IsFailure)
                return BadRequest(sessao.Error);

            await _sessoesRepositorio.InserirAsync(sessao.Value, cancellationToken);
            await _sessoesRepositorio.CommitAsync(cancellationToken);

            return CreatedAtAction("RecuperarPorId", new { id = sessao.Value.Id }, sessao.Value.Id);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> RecuperarPorIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var sessao = await _sessoesRepositorio.RecuperarPorIdAsync(id, cancellationToken);

            if (sessao == null)
                return NotFound();

            return Ok(sessao);
        }

        [HttpGet()]
        public async Task<IActionResult> RecuperarTodosAsync(CancellationToken cancellationToken)
        {
            var sessao = await _sessoesRepositorio.RecuperarTodosAsync(cancellationToken);
            return Ok(sessao);
        }
    }
}
