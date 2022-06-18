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
    public class ServicosRepositorio
    {
        private readonly IConfiguration _configuration;
        private readonly AnuncioDbContext _anuncioDbContext;

        public ServicosRepositorio(AnuncioDbContext anuncioDbContext, IConfiguration configuracao)
        {
            _anuncioDbContext = anuncioDbContext;
            _configuration = configuracao;
        }

        public async Task InserirAsync(Servico servico, CancellationToken cancellationToken = default)
        {
            await _anuncioDbContext.AddAsync(servico, cancellationToken);
        }

        public async Task<IEnumerable<Servico>> RecuperarTodosAsync(CancellationToken cancellationToken = default)
        {
            return await _anuncioDbContext
                .Servicos
                .ToListAsync(cancellationToken);
        }

        public async Task<Servico> RecuperarPorIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _anuncioDbContext
                .Servicos
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }

        public async Task CommitAsync(CancellationToken cancellationToken)
        {
            await _anuncioDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
