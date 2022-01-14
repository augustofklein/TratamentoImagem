using AplicativoCinema.WebApi.Dominio;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using static AplicativoCinema.WebApi.Infraestrutura.FilmesDTO;

namespace AplicativoCinema.WebApi.Infraestrutura
{
    public class SessoesRepositorio
    {
        private readonly IConfiguration _configuracao;

        public SessoesRepositorio(IConfiguration configuracao)
        {
            _configuracao = configuracao;
        }

        public Filme RecuperarPorIdeDia(string id, EDiaSemana diaSemana)
        {
            using (SqlConnection connection = new SqlConnection(_configuracao.GetConnectionString("Cinemas")))
            {
                const string sql = @"SELECT FilmeSessao.Id,
                                            FilmeSessao.IdFilme,
                                            FilmeSessao.DiaSemana,
                                            FilmeSessao.HorarioInicial,
                                            FilmeSessao.QuantidadeLugares,
                                            FilmeSessao.Preco
                                       FROM FilmesSessao
                                       LEFT JOIN Filmes ON Filmes.Id = FilmesSessao.IdFilme
                                      WHERE FilmesSessao.IdFilme = @id
                                        and FilmesSessao.DiaSemana = @diaSemana";

                var sessaoDicionario = new Dictionary<Guid, Filme>();
                var lista = connection.Query<FilmesDTO, SessaoDTO, Filme>(
                    sql,
                    (filme, sessao) =>
                    {
                        if (sessaoDicionario.TryGetValue(filme.Id, out var filmeExistente))
                        {
                            filmeExistente.AdicionarSessao(sessao.DiaSemana, sessao.HorarioInicial, sessao.QuantidadeLugares, sessao.Preco);
                            return filmeExistente;
                        }
                        else
                        {
                            var novoFilme = new Filme(filme.Id, filme.Titulo, filme.Duracao, filme.Sinopse, new List<Sessao>());
                            return null;
                        }
                    },
                    splitOn: "IdSessaoSplit",
                    param: new { id = id.ToString() });
                return lista.FirstOrDefault();
            }
        }
    }
}
