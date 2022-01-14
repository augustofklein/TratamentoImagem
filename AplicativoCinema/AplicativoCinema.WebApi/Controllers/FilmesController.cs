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
        public IActionResult Cadastrar([FromBody] NovoFilmeInputModel filmeInputModel)
        {
            var filme = Filme.Criar(filmeInputModel.Titulo, filmeInputModel.Duracao, filmeInputModel.Sinopse);
            if (filme.IsFailure)
                return BadRequest(filme.Error);
            foreach (var sessaoInput in filmeInputModel.Sessao)
            {
                var horarioInicial = Horario.Criar(sessaoInput.HorarioInicial);
                if (horarioInicial.IsFailure)
                    return BadRequest(horarioInicial.Error);
                filme.Value.AdicionarSessao((EDiaSemana)sessaoInput.DiaSemana, horarioInicial.Value, sessaoInput.QuantidadeLugares, sessaoInput.Preco);
            }

            _filmesRepositorio.Inserir(filme.Value);

            return CreatedAtAction(nameof(RecuperarPoId), new { id = filme.Value.Id}, filme.Value);
        }

        [HttpPut("id")]
        public IActionResult Atualizar(string id, [FromBody] AlterarFilmeInputModel filmeInputModel)
        {
            if (!Guid.TryParse(id, out var guid))
                return BadRequest("Id inválido");

            var filme = _filmesRepositorio.RecuperarPorId(guid);
            if (filme == null)
                return NotFound();
            filme.LimparSessoes();
            foreach (var sessaoInput in filmeInputModel.Sessao)
            {
                var horarioInicial = Horario.Criar(sessaoInput.HorarioInicial);
                if (horarioInicial.IsFailure)
                    return BadRequest(horarioInicial.Error);
                if (string.IsNullOrEmpty(sessaoInput.Id))
                {
                    filme.AdicionarSessao((EDiaSemana)sessaoInput.DiaSemana, horarioInicial.Value, sessaoInput.QuantidadeLugares, sessaoInput.Preco);
                }
                else
                {
                    if (!Guid.TryParse(sessaoInput.Id, out var guidSessao))
                        return BadRequest("Id da sessão inválido");
                    var sessao = new Sessao(guidSessao, (EDiaSemana)sessaoInput.DiaSemana, horarioInicial.Value, sessaoInput.QuantidadeLugares, sessaoInput.Preco);
                    filme.AdicionarSessao(sessao);
                }
            }

            _filmesRepositorio.Alterar(filme);

            return Ok(filme);
        }


        [HttpGet]
        public IActionResult RecuperarTodos()
        {
            return Ok(_filmesRepositorio.RecuperarTodos());
        }

        /*
         * Rota aguardando um parâmetro
         */
        [HttpGet("{id}")]
        public IActionResult RecuperarPoId(string id)
        {
            if (!Guid.TryParse(id, out var guid))
                return BadRequest("Id inválido");

            var filme = _filmesRepositorio.RecuperarPorId(guid);
            if (filme == null)
                return NotFound();

            return Ok(filme);
        }
    }
}
