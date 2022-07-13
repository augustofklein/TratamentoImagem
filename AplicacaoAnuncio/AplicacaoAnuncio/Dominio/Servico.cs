using CSharpFunctionalExtensions;
using System;

namespace AplicacaoAnuncio.Dominio
{
    public sealed class Servico
    {
        public Guid Id { get; private set; }
        public Guid UsuarioId {get;set;}
        public string NomeServico { get; set; }
        public string Descricao { get; set; }
        public int Categoria { get; set; }
        public double Valor { get; set; }

        private Servico(Guid id, Guid usuarioId,string nomeServico, string descricao, int categoria, double valor)
        {
            Id = id;
            UsuarioId = usuarioId;
            NomeServico = nomeServico;
            Descricao = descricao;
            Categoria = categoria;
            Valor = valor;
        }

        public static Result<Servico> Criar(Guid usuarioId, string nomeServico, string descricao, int categoria, double valor)
        {
            var servico = new Servico(Guid.NewGuid(), usuarioId, nomeServico, descricao, categoria, valor);
            return servico;
        }
    }
}
