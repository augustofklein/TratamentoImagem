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
    public class ServicosController : ControllerBase
    {
        private readonly ServicosRepositorio _servicosRepositorio;

        public ServicosController(ServicosRepositorio servicosRepositorio)
        {
            _servicosRepositorio = servicosRepositorio;
        }

        [EnableCors("AllowOrigin")]
        [HttpPost]
        public async Task<IActionResult> CadastrarAsync([FromBody] NovoServicoInputModel servicoInputModel, CancellationToken cancellationToken)
        {
            if(!Guid.TryParse(servicoInputModel.UsuarioId, out var _usuarioId))
                return BadRequest("Id do usuário inválido");

            var servico = Servico.Criar(_usuarioId,
                                        servicoInputModel.NomeServico,
                                        servicoInputModel.Descricao,
                                        servicoInputModel.Categoria,
                                        servicoInputModel.Valor);

            if (servico.IsFailure)
                return BadRequest(servico.Error);

            await _servicosRepositorio.InserirAsync(servico.Value, cancellationToken);
            await _servicosRepositorio.CommitAsync(cancellationToken);

            return CreatedAtAction("RecuperarPorId", new { id = servico.Value.Id}, servico.Value.Id);
        }

        [EnableCors("AllowOrigin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> RecuperarPorIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var servico = await _servicosRepositorio.RecuperarPorIdAsync(id, cancellationToken);

            if (servico == null)
                return NotFound();

            return Ok(servico);
        }

        [EnableCors("AllowOrigin")]
        [HttpGet()]
        public async Task<IActionResult> RecuperarTodosAsync(CancellationToken cancellationToken)
        {
            var servico = await _servicosRepositorio.RecuperarTodosAsync(cancellationToken);
            return Ok(servico);
        }
    }
}
