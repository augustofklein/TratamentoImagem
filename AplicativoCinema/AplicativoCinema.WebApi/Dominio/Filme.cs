using System;
using System.Collections.Generic;
using CSharpFunctionalExtensions;

namespace AplicativoCinema.WebApi.Dominio
{
    public sealed class Filme
    {
        private IList<Sessao> _sessao;
        public Filme(Guid id, string titulo, int duracao, string sinopse, List<Sessao> sessao) {
            Id = id;
            Titulo = titulo;
            Duracao = duracao;
            Sinopse = sinopse;
            _sessao = sessao;
        }

        public Guid Id { get; }
        public string Titulo { get; }
        public int Duracao { get; }
        public string Sinopse { get; }
        public IEnumerable<Sessao> Sessao => _sessao;

        public void AdicionarSessao(Sessao sessao)
        {
            _sessao.Add(sessao);
        }

        public void AdicionarSessao(EDiaSemana diaSemana, Horario horarioInicial, int quantidadeLugares, float preco)
        {
            _sessao.Add(new Sessao(Guid.NewGuid(), diaSemana, horarioInicial, quantidadeLugares, preco));
        }

        public void LimparSessoes()
        {
            _sessao.Clear();
        }

        public static Result<Filme> Criar(string titulo, int duracao, string sinopse)
        {
            return new Filme(Guid.NewGuid(), titulo, duracao, sinopse, new List<Sessao>());
        }
    }
}
