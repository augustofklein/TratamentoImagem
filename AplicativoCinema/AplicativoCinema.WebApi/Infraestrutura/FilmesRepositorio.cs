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

namespace AplicativoCinema.WebApi.Infraestrutura
{
    public sealed class FilmesRepositorio
    {
        private readonly IConfiguration _configuracao;

        public FilmesRepositorio(IConfiguration configuracao)
        {
            _configuracao = configuracao;
        }

        public void Inserir(Filme filme)
        {
            using(SqlConnection connection = new SqlConnection(_configuracao.GetConnectionString("Cinemas")))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    const string sqlFilme = @"INSERT INTO Filmes (Id, Titulo, Duracao, Sinopse) VALUES (@id, @titulo, @duracao, @sinopse)";
                    const string sqlSessao = @"INSERT INTO FilmesSessao (Id, IdFilme, DiaSemana, HorarioInicial, QuantidadeLugares, Preco) VALUES (@id, @idFilme, @diaSemana, @horarioInicial, @quantidadeLugares, @preco)";
                    
                    connection.Query(
                        sqlFilme,
                        param: new { id = filme.Id, titulo = filme.Titulo, duracao = filme.Duracao, sinopse = filme.Sinopse },
                        transaction:transaction);
                    
                    
                    var sessoes =
                    connection.Execute(
                        sqlSessao,
                        param: filme.Sessao.Select(a => new
                        {
                            id = Guid.NewGuid(),
                            idFilme = filme.Id,
                            diaSemana = a.DiaSemana,
                            horarioInicial = a.HorarioInicial,
                            quantidadeLugares = a.QuantidadeLugares,
                            preco = a.Preco
                        }),
                        transaction: transaction);
                    transaction.Commit();
                }
            }
        }

        public void Alterar(Filme filme)
        {
            using (SqlConnection connection = new SqlConnection(_configuracao.GetConnectionString("Cinemas")))
            {
                connection.Open();
                const string sqlSessoesParaAtualizar = @"SELECT Id FROM FilmesSessao WHERE Id IN @ids";
                var idsSessao = filme.Sessao.Select(a => a.Id).ToArray();
                var idsExistentes = connection.Query<Guid>(sqlSessoesParaAtualizar, new { ids = idsSessao });

                using (var transaction = connection.BeginTransaction())
                {
                    const string sqlFilme = @"UPDATE Filmes SET Titulo = @Titulo WHERE Id = @Id";
                    const string sqlInsertSessao = @"INSERT INTO FilmesSessao (Id, IdFilme, DiaSemana, HorarioInicial, QuantidadeLugares, Preco) VALUES (@Id, @IdTurma, @DiaSemana, @HorarioInicial, @QuantidadeLugares, @Preco)";
                    const string sqlUpdateSessao = @"UPDATE FilmesSessao SET DiaSemana = @DiaSemana, HorarioInicial = @HorarioInicial, QuantidadeLugares = @QuantidadeLugares, Preco = @Preco WHERE Id = @Id";
                    const string sqlDeleteSessao = @"DELETE FROM FilmesSessao WHERE Id IN (SELECT Id FROM FilmesSessao WHERE IdFilme = @idFilme AND Id NOT IN @ids)";

                    var sessoesIncluir = filme.Sessao.Where(c => !idsExistentes.Any(i => i == c.Id));
                    if (sessoesIncluir.Any())
                    {
                        connection.Execute(
                            sqlInsertSessao,
                            param: sessoesIncluir
                                .Select(a => new
                                {
                                    iD = a.Id,
                                    IdFilme = filme.Id,
                                    a.DiaSemana,
                                    a.HorarioInicial,
                                    a.QuantidadeLugares,
                                    a.Preco
                                }),
                            transaction: transaction);
                    }

                    connection.Execute(
                        sqlDeleteSessao,
                        new { ids = idsSessao, idFilme = filme.Id },
                        transaction: transaction);

                    foreach (var sessao in filme.Sessao.Where(c=> idsExistentes.Any(i=> i == c.Id))) {
                        connection.Execute(
                            sqlUpdateSessao, new
                            {
                                Id = sessao.Id,
                                IdFilme = filme.Id,
                                sessao.DiaSemana,
                                sessao.HorarioInicial,
                                sessao.QuantidadeLugares,
                                sessao.Preco
                            },
                            transaction: transaction);
                    }

                    connection.Execute(
                        sqlFilme, new
                        {
                            filme.Id,
                            filme.Titulo,
                            filme.Duracao,
                            filme.Sinopse
                        },
                        transaction: transaction);

                    transaction.Commit();
                }
            }
        }

        public IEnumerable<Filme> RecuperarTodos()
        {
            using (SqlConnection connection = new SqlConnection(_configuracao.GetConnectionString("Cinemas")))
            {
                const string sql = @"SELECT Filmes.Id,
                                            Filmes.Titulo,
                                            Filmes.Duracao,
                                            Filmes.Sinopse,
                                            FilmesSessao.Id as IdSessaoSplit,
                                            FilmesSessao.Id,
                                            FilmesSessao.DiaSemana,
                                            FilmesSessao.HorarioInicial,
                                            FilmesSessao.QuantidadeLugares,
                                            FilmesSessao.Preco
                                       FROM Filmes
                                       LEFT JOIN FilmesSessao ON FilmesSessao.IdFilme = Filmes.Id";

                var filmeDicionario = new Dictionary<Guid, Filme>();
                var lista = connection.Query<FilmesDTO, SessaoDTO, Filme>(
                    sql,
                    (filme, sessao) =>
                    {
                        if (filmeDicionario.TryGetValue(filme.Id, out var filmeExistente))
                        {
                            filmeExistente.AdicionarSessao(sessao.DiaSemana, sessao.HorarioInicial, sessao.QuantidadeLugares, sessao.Preco);
                            return filmeExistente;
                        }
                        else
                        {
                            var novoFilme = new Filme(filme.Id, filme.Titulo, filme.Duracao, filme.Sinopse, new List<Sessao>());
                            novoFilme.AdicionarSessao(sessao.DiaSemana, sessao.HorarioInicial, sessao.QuantidadeLugares, sessao.Preco);
                            return novoFilme;
                        }
                    },
                    splitOn: "IdSessaoSplit");
                return lista;
            }
        }

        public Filme RecuperarPorId(Guid id)
        {
            using (SqlConnection connection = new SqlConnection(_configuracao.GetConnectionString("Cinemas")))
            {
                const string sql = @"SELECT Filmes.Id,
                                            Filmes.Titulo,
                                            Filmes.Duracao,
                                            Filmes.Sinopse,
                                            FilmesSessao.Id as IdSessaoSplit,
                                            FilmesSessao.Id,
                                            FilmesSessao.DiaSemana,
                                            FilmesSessao.HorarioInicial,
                                            FilmesSessao.QunatidadesLugares,
                                            FilmesSessao.Preco
                                       FROM Filmes
                                       LEFT JOIN FilmesSessao ON FilmesSessao.IdFilme = Filmes.Id
                                      WHERE Filme.Id = @id";

                var filmeDicionario = new Dictionary<Guid, Filme>();
                var lista = connection.Query<FilmesDTO, SessaoDTO, Filme>(
                    sql,
                    (filme, sessao) =>
                    {
                        if (filmeDicionario.TryGetValue(filme.Id, out var filmeExistente))
                        {
                            filmeExistente.AdicionarSessao(sessao.DiaSemana, sessao.HorarioInicial, sessao.QuantidadeLugares, sessao.Preco);
                            return filmeExistente;
                        }
                        else
                        {
                            var novoFilme = new Filme(filme.Id, filme.Titulo, filme.Duracao, filme.Sinopse, new List<Sessao>());
                            novoFilme.AdicionarSessao(sessao.DiaSemana, sessao.HorarioInicial, sessao.QuantidadeLugares, sessao.Preco);
                            filmeDicionario.Add(filme.Id, novoFilme);
                            return novoFilme;
                        }
                    },
                    splitOn: "IdSessaoSplit",
                    param: new { id = id.ToString() });
                return lista.FirstOrDefault();
            }
        }
    }
}
