using AplicacaoAnuncio.Dominio;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AplicacaoAnuncio.Infraestrutura
{
    public class AvaliacoesRepositorio
    {
        private readonly IConfiguration _configuration;
        private readonly AnuncioDbContext _anuncioDbContext;

        public AvaliacoesRepositorio(AnuncioDbContext anuncioDbContext, IConfiguration configuracao)
        {
            _anuncioDbContext = anuncioDbContext;
            _configuration = configuracao;
        }

        public async Task InserirAsync(Avaliacao avaliacao, CancellationToken cancellationToken = default)
        {
            await _anuncioDbContext.AddAsync(avaliacao, cancellationToken);
        }

        public async Task<IEnumerable<Avaliacao>> RecuperarTodosAsync(CancellationToken cancellationToken = default)
        {
            return await _anuncioDbContext
                .Avaliacoes
                .ToListAsync(cancellationToken);
        }

        public async Task<Avaliacao> RecuperarPorIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _anuncioDbContext
                .Avaliacoes
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }

        public async Task CommitAsync(CancellationToken cancellationToken)
        {
            await _anuncioDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
