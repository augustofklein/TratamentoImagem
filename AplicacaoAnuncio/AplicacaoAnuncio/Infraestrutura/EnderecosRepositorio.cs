using AplicacaoAnuncio.Dominio;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AplicacaoAnuncio.Infraestrutura
{
    public class EnderecosRepositorio
    {
        private readonly IConfiguration _configuration;
        private readonly AnuncioDbContext _anuncioDbContext;

        public EnderecosRepositorio(AnuncioDbContext anuncioDbContext, IConfiguration configuracao)
        {
            _anuncioDbContext = anuncioDbContext;
            _configuration = configuracao;
        }

        public async Task InserirAsync(Endereco endereco, CancellationToken cancellationToken = default)
        {
            await _anuncioDbContext.AddAsync(endereco, cancellationToken);
        }

        public async Task<IEnumerable<Endereco>> RecuperarTodosAsync(CancellationToken cancellationToken = default)
        {
            return await _anuncioDbContext
                .Enderecos
                .ToListAsync(cancellationToken);
        }

        public async Task<Endereco> RecuperarPorIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _anuncioDbContext
                .Enderecos
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }

        public async Task CommitAsync(CancellationToken cancellationToken)
        {
            await _anuncioDbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<Endereco> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var result = await _anuncioDbContext
                        .Enderecos
                        .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

            if (result != null)
            {
                _anuncioDbContext.Remove(result);
                await _anuncioDbContext.SaveChangesAsync(cancellationToken);
                return result;
            }

            return result;
        }

        public async Task UpdateAsync(CancellationToken cancellationToken = default)
        {
            await _anuncioDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
