using AplicativoCinema.WebApi.Infraestrutura;
using AplicativoCinema.WebApi.Models;
using AplicativoCinema.WebApi.Dominio;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace AplicativoCinema.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilmesController : ControllerBase
    {
        private readonly FilmesRepositorio _filmesRepositorio;

        public FilmesController(FilmesRepositorio filmesRepositorio)
        {
            _filmesRepositorio = filmesRepositorio;
        }

        [HttpPost]
        public async Task<IActionResult> CadastrarAsync([FromBody] NovoFilmeInputModel filmeInputModel, CancellationToken cancellationToken)
        {
            var filme = Filme.Criar(filmeInputModel.Titulo, filmeInputModel.Duracao, filmeInputModel.Sinopse);

            if (filme.IsFailure)
                return BadRequest(filme.Error);

            await _filmesRepositorio.InserirAsync(filme.Value, cancellationToken);
            await _filmesRepositorio.CommitAsync(cancellationToken);

            return CreatedAtAction("RecuperarPorId", new { id = filme.Value.Id }, filme.Value.Id);
        }

        [HttpPut("id")]
        public async Task<IActionResult> Atualizar(string id, [FromBody] AlterarFilmeInputModel filmeInputModel, CancellationToken cancellationToken)
        {
            if (!Guid.TryParse(id, out var guid))
                return BadRequest("Id inválido");

            var filme = await _filmesRepositorio.RecuperarPorIdAsync(guid, cancellationToken);
            if (filme == null)
                return NotFound();

            _filmesRepositorio.Alterar(filme);
            await _filmesRepositorio.CommitAsync(cancellationToken);

            return Ok(filme);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> RecuperarPorIdAsync(string id, CancellationToken cancellationToken)
        {
            if (!Guid.TryParse(id, out var guid))
                return BadRequest("Id inválido");

            var filme = await _filmesRepositorio.RecuperarPorIdAsync(guid, cancellationToken);
            if (filme == null)
                return NotFound();

            return Ok(filme);
        }
    }
}
