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
    public class EnderecosController : ControllerBase
    {
        private readonly EnderecosRepositorio _enderecosRepositorio;

        public EnderecosController(EnderecosRepositorio enderecosRepositorio)
        {
            _enderecosRepositorio = enderecosRepositorio;
        }

        [EnableCors("AllowOrigin")]
        [HttpPost]
        public async Task<IActionResult> CadastrarAsync([FromBody] NovoEnderecoInputModel enderecoInputModel, CancellationToken cancellationToken)
        {
            if (!Guid.TryParse(enderecoInputModel.UsuarioId, out var _usuarioId))
                return BadRequest("Id do usuário inválido");

            if(enderecoInputModel.Cep.Length != 9)
                return BadRequest("Cep inválido");

            var endereco = Endereco.Criar(_usuarioId,
                                          enderecoInputModel.Cep,
                                          enderecoInputModel.Estado,
                                          enderecoInputModel.Cidade,
                                          enderecoInputModel.Logradouro,
                                          enderecoInputModel.Numero,
                                          enderecoInputModel.Bairro);

            if (endereco.IsFailure)
                return BadRequest(endereco.Error);

            await _enderecosRepositorio.InserirAsync(endereco.Value, cancellationToken);
            await _enderecosRepositorio.CommitAsync(cancellationToken);

            return CreatedAtAction("RecuperarPorId", new { id = endereco.Value.Id }, endereco.Value.Id);
        }

        [EnableCors("AllowOrigin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> RecuperarPorIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var endereco = await _enderecosRepositorio.RecuperarPorIdAsync(id, cancellationToken);

            if (endereco == null)
                return NotFound();

            return Ok(endereco);
        }

        [EnableCors("AllowOrigin")]
        [HttpGet]
        public async Task<IActionResult> RecuperarTodosAsync(CancellationToken cancellationToken)
        {
            var avaliacoes = await _enderecosRepositorio.RecuperarTodosAsync(cancellationToken);

            return Ok(avaliacoes);
        }
    }
}
