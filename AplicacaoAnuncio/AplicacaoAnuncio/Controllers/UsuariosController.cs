using AplicacaoAnuncio.Dominio;
using AplicacaoAnuncio.Infraestrutura;
using AplicacaoAnuncio.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AplicacaoAnuncio.Controllers
{
    [DisableCors]
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController: ControllerBase
    {
        private readonly UsuariosRepositorio _usuariosRepositorio;

        public UsuariosController(UsuariosRepositorio usuariosRepositorio)
        {
            _usuariosRepositorio = usuariosRepositorio;
        }

        [EnableCors("AllowOrigin")]
        [HttpPost]
        public async Task<IActionResult> CadastrarAsync([FromBody] NovoUsuarioInputModel servicoInputModel, CancellationToken cancellationToken)
        {
            var usuario = Usuario.Criar(servicoInputModel.Cpf,
                                        servicoInputModel.Nome,
                                        servicoInputModel.DataNascimento,
                                        servicoInputModel.Sexo);

            if (usuario.IsFailure)
                return BadRequest(usuario.Error);

            await _usuariosRepositorio.InserirAsync(usuario.Value, cancellationToken);
            await _usuariosRepositorio.CommitAsync(cancellationToken);

            return CreatedAtAction("RecuperarPorId", new { id = usuario.Value.Id }, usuario.Value.Id);
        }

        [EnableCors("AllowOrigin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> RecuperarPorIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var usuario = await _usuariosRepositorio.RecuperarPorIdAsync(id, cancellationToken);

            if (usuario == null)
                return NotFound();

            return Ok(usuario);
        }

        [EnableCors("AllowOrigin")]
        [HttpGet()]
        public async Task<IActionResult> RecuperarTodosAsync(CancellationToken cancellationToken)
        {
            var usuario = await _usuariosRepositorio.RecuperarTodosAsync(cancellationToken);
            return Ok(usuario);
        }
    }
}
