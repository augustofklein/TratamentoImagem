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
    public sealed class FilmesRepositorio
    {
        private readonly IConfiguration _configuration;
        private readonly CinemasDbContext _cinemasDbContext;

        public FilmesRepositorio(CinemasDbContext cinemasDbContext, IConfiguration configuracao)
        {
            _cinemasDbContext = cinemasDbContext;
            _configuration = configuracao;
        }

        public async Task InserirAsync(Filme filme, CancellationToken cancellationToken = default)
        {
            await _cinemasDbContext.AddAsync(filme, cancellationToken);
        }

        public async Task<IEnumerable<Filme>> RecuperarTodosAsync(CancellationToken cancellationToken = default)
        {
            return await _cinemasDbContext
                .Filmes
                .ToListAsync(cancellationToken);
        }

        public async Task<Filme> RecuperarPorIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _cinemasDbContext
                .Filmes
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }

        public async Task CommitAsync(CancellationToken cancellationToken)
        {
            await _cinemasDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
