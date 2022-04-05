using AplicativoCinema.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Dapper;
using AplicativoCinema.WebApi.Dominio;
using static AplicativoCinema.WebApi.Infraestrutura.FilmesDTO;
using System.Threading;
using Microsoft.EntityFrameworkCore;

namespace AplicativoCinema.WebApi.Infraestrutura
{
    public sealed class FilmesRepositorio
    {
        private readonly CinemasDbContext _cinemasDbContext;
        private readonly IConfiguration _configuracao;

        public FilmesRepositorio(CinemasDbContext cinemasDbContext, IConfiguration configuracao)
        {
            _cinemasDbContext = cinemasDbContext;
            _configuracao = configuracao;
        }

        public async Task InserirAsync(Filme filme, CancellationToken cancellationToken = default)
        {
            await _cinemasDbContext.AddAsync(filme, cancellationToken);
        }

        public void Alterar(Filme filme)
        {
            // Nada a fazer EF CORE fazer o Tracking da Entidade quando recuperamos a mesma
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

        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            await _cinemasDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
