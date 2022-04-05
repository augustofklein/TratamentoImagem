using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AplicacaoCinema.WebApi.Dominio
{
    public sealed class Sessao
    {
        public Guid Id { get; }
        public Guid FilmeId { get; }
        public EDiaSemana DiaSemana { get; }
        public Horario HorarioInicial { get; }
        public double Preco { get; }
        public int TotalIngressos { get; }

        private Sessao(Guid id, Guid filmeId, EDiaSemana diaSemana, Horario horarioInicial, double preco, int totalIngressos)
        {
            Id = id;
            FilmeId = filmeId;
            DiaSemana = diaSemana;
            HorarioInicial = horarioInicial;
            Preco = preco;
            TotalIngressos = totalIngressos;
        }

        public static Result<Sessao> Criar(Guid filmeId, EDiaSemana diaSemana, Horario horario, double preco, int totalIngressos)
        {
            var sessao = new Sessao(Guid.NewGuid(), filmeId, diaSemana, horario, preco, totalIngressos);
            return sessao;
        }
    }
}
