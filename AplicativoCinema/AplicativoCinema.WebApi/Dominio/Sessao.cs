using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AplicativoCinema.WebApi.Dominio
{
    public sealed class Sessao
    {
        private IList<Sessao> _sessao;
        public Sessao(Guid id, EDiaSemana diaSemana, Horario horarioInicial, int quantidadeLugares, float preco)
        {
            Id = id;
            DiaSemana = diaSemana;
            HorarioInicial = horarioInicial;
            QuantidadeLugares = quantidadeLugares;
            Preco = preco;
        }

        public Guid Id { get; }
        public EDiaSemana DiaSemana { get; }
        public Horario HorarioInicial { get; set; }
        public int QuantidadeLugares { get; set; }
        public float Preco { get; set; }
    }
}
