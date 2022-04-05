using AplicativoCinema.WebApi.Dominio;
using AplicativoCinema.WebApi.Infraestrutura;
using AplicativoCinema.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AplicativoCinema.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessoesController : ControllerBase
    {
        private readonly SessoesRepositorio _sessaoRepositorio;
        private readonly FilmesRepositorio _filmeRepositorio;

        public SessoesController(SessoesRepositorio sessaoRepositorio, FilmesRepositorio filmeRepositorio)
        {
            _sessaoRepositorio = sessaoRepositorio;
            _filmeRepositorio = filmeRepositorio;
        }

        [HttpPost]
        public async Task<IActionResult> CadastrarAsync([FromBody] NovaSessaoInputModel sessaoInputModel, CancellationToken cancellationToken)
        {
            var _horario = Horario.Criar(sessaoInputModel.Horario);
            if (_horario.IsFailure)
                return BadRequest(_horario.Error);

            if (!Guid.TryParse(sessaoInputModel.IdFilme, out var _filmeId))
                return BadRequest("O Id do filme é inválido");

            var sessao = Sessao.Criar(_filmeId, (EDiaSemana)sessaoInputModel.DiaSemana, _horario.Value, sessaoInputModel.QuantidadeLugares, sessaoInputModel.Preco, sessaoInputModel.TotalIngressos);
            return CreatedAtAction("RecuperarPorId", new { id = sessao.Value.Id }, sessao.Value.Id);
        }
    }
}
