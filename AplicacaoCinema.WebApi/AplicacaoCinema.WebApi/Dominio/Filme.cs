using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AplicacaoCinema.WebApi.Dominio
{
    public sealed class Filme
    {
        public Guid Id { get; }
        public string Titulo { get; }
        public int Duracao { get; }
        public string Sinopse { get; }

        public Filme(Guid id, string titulo, int duracao, string sinopse)
        {
            Id = id;
            Titulo = titulo;
            Duracao = duracao;
            Sinopse = sinopse;
        }

        public static Result<Filme> Criar(string titulo, int duracao, string sinopse)
        {
            var filme = new Filme(Guid.NewGuid(), titulo, duracao, sinopse);
            return filme;
        }
    }
}
