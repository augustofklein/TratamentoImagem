using AplicativoCinema.WebApi.Dominio;
using AplicativoCinema.WebApi.Infraestrutura;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AplicativoCinema.WebApi.Controllers
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

        [HttpGet("{id},{diaSemana}")]
        public IActionResult RecuperarPorIdeDia(string id, EDiaSemana diaSemana)
        {
            if (!Guid.TryParse(id, out var guid))
                return BadRequest("Id inválido");

            var filme = _sessoesRepositorio.RecuperarPorIdeDia(id, diaSemana);
            if(filme == null)
                return NotFound();

            return Ok(filme.Sessao);
        }
    }
}
