using AplicacaoCinema.WebApi.Dominio;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AplicacaoCinema.WebApi.Infraestrutura
{
    public sealed class IngressosRepositorio
    {
        private readonly IConfiguration _configuration;
        private readonly CinemasDbContext _cinemasDbContext;

        public IngressosRepositorio(CinemasDbContext cinemasDbContext, IConfiguration configuracao)
        {
            _cinemasDbContext = cinemasDbContext;
            _configuration = configuracao;
        }

        public async Task InserirAsync(Ingresso ingresso, CancellationToken cancellationToken = default)
        {
            await _cinemasDbContext.AddAsync(ingresso, cancellationToken);
        }

        public async Task<IEnumerable<Ingresso>> RecuperarTodosAsync(CancellationToken cancellationToken = default)
        {
            return await _cinemasDbContext
                .Ingressos
                .ToListAsync(cancellationToken);
        }

        public async Task<Ingresso> RecuperarPorIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _cinemasDbContext
                .Ingressos
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }

        public async Task<IEnumerable<Ingresso>> RecuperarIngressosSessao(Guid sessaoId, CancellationToken cancellationToken = default)
        {
            return await _cinemasDbContext
                .Ingressos
                .Include(c => c.SessaoId == sessaoId)
                .ToListAsync(cancellationToken);
        }

        public async Task CommitAsync(CancellationToken cancellationToken)
        {
            await _cinemasDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
