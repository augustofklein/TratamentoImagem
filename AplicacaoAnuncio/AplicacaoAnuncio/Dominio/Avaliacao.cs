using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AplicacaoAnuncio.Dominio
{
    public class Avaliacao
    {
        public Guid Id { get; set; }
        public Guid ServicoId { get; set; }
        public int Nota { get; set; }

        public Avaliacao(Guid id, Guid servicoId, int nota)
        {
            Id = id;
            ServicoId = servicoId;
            Nota = nota;
        }

        public static Result<Avaliacao> Criar(Guid servicoId, int nota)
        {
            var avaliacao = new Avaliacao(Guid.NewGuid(), servicoId, nota);

            return avaliacao;
        }
    }
}
