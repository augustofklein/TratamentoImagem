using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AplicativoCinema.WebApi.Dominio
{
    public sealed class Sessao
    {
        private Sessao() { }

        public Sessao(Guid id, Guid idFilme, EDiaSemana diaSemana, Horario horario, int quantidadeLugares, double preco, int totalIngressos)
        {
            Id = id;
            IdFilme = idFilme;
            DiaSemana = diaSemana;
            Horario = horario;
            QuantidadeLugares = quantidadeLugares;
            Preco = preco;
            TotalIngressos = totalIngressos;
        }

        public Guid Id { get; }
        public Guid IdFilme { get; }
        public EDiaSemana DiaSemana { get; }
        public Horario Horario { get; }
        public int QuantidadeLugares { get; }
        public double Preco { get; }
        public int TotalIngressos { get; }

        public static Result<Sessao> Criar(Guid idFilme, EDiaSemana diaSemana, Horario horario, int quantidadeLugares, double preco, int totalIngressos)
        {
            var sessao = new Sessao(Guid.NewGuid(), idFilme, diaSemana, horario, quantidadeLugares, preco, totalIngressos);
            return sessao;
        }
    }
}
