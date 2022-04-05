using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AplicacaoCinema.WebApi.Dominio
{
    public sealed class Ingresso
    {
        public Guid Id { get; }
        public Guid SessaoId { get; }
        public int QuantidadeIngressos { get; }

        private Ingresso(Guid id, Guid sessaoId, int quantidadeIngressos)
        {
            Id = id;
            SessaoId = sessaoId;
            QuantidadeIngressos = quantidadeIngressos;
        }

        public static Result<Ingresso> Criar(Guid sessaoId, int quantidadeIngressos)
        {
            var ingresso = new Ingresso(Guid.NewGuid(), sessaoId, quantidadeIngressos);
            return ingresso;
        }

        public bool ExisteIngressoDisponiveLSessao(List<Ingresso> ingresso, Sessao sessao, int quantidadeIngressos)
        {
            int _totalIngressos = 0;

            for (int i = 1; i < ingresso.Count(); i++)
            {
                _totalIngressos += ingresso[i].QuantidadeIngressos;
            }

            _totalIngressos += quantidadeIngressos;

            if (_totalIngressos >= sessao.TotalIngressos)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
