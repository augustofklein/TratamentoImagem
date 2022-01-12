using AplicativoCinema.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Dapper;

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
            using(SqlConnection connection = new SqlConnection(_configuracao.GetConnectionString("Filmes")))
            {
                var comando = new SqlCommand($"INSERT INTO Filmes (Id, Descricao) VALUES ('{filme.Id}','{filme.Titulo}')", connection);
                connection.Open();
                var resultado = comando.ExecuteNonQuery();
            }
        }

        public IEnumerable<Filme> RecuperarTodos()
        {
            using (SqlConnection connection = new SqlConnection(_configuracao.GetConnectionString("Filmes")))
            {
                /*
                 * O Dapper realiza a abertura da conexão
                 */
                var lista = connection.Query<Filme>(@"SELECT Filmes.Id,
                                                             Filmes.Titulo
                                                        FROM Filmes");
                return lista;
            }
        }
    }
}
