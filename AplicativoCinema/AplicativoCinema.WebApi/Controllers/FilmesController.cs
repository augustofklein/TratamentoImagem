using AplicativoCinema.WebApi.Infraestrutura;
using AplicativoCinema.WebApi.Models;
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
        public IActionResult Cadastrar([FromBody]Filme filme)
        {
            filme.Id = Guid.NewGuid().ToString();
            _filmesRepositorio.Inserir(filme);

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
            var guid = Guid.Parse(id);

            return Ok(guid);
        }

    }
}
