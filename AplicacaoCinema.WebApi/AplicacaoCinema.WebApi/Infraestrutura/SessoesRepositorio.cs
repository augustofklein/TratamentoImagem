using AplicacaoCinema.WebApi.Dominio;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AplicacaoCinema.WebApi.Infraestrutura
{
    public sealed class SessoesRepositorio
    {
        private readonly IConfiguration _configuration;
        private readonly CinemasDbContext _cinemasDbContext;

        public SessoesRepositorio(CinemasDbContext cinemasDbContext, IConfiguration configuracao)
        {
            _cinemasDbContext = cinemasDbContext;
            _configuration = configuracao;
        }

        public async Task InserirAsync(Sessao sessao, CancellationToken cancellationToken = default)
        {
            await _cinemasDbContext.AddAsync(sessao, cancellationToken);
        }

        public async Task<IEnumerable<Sessao>> RecuperarTodosAsync(CancellationToken cancellationToken = default)
        {
            return await _cinemasDbContext
                .Sessoes
                .ToListAsync(cancellationToken);
        }

        public async Task<Sessao> RecuperarPorIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _cinemasDbContext
                .Sessoes
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }

        public async Task CommitAsync(CancellationToken cancellationToken)
        {
            await _cinemasDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
