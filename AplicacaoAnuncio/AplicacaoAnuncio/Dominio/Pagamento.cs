using CSharpFunctionalExtensions;
using System;

namespace AplicacaoAnuncio.Dominio
{
    public class Pagamento
    {
        public Guid Id { get; set; }
        public Guid ServicoId { get; set; }
        public int TipoPagamento { get; set; }
        public int QuantidadeParcelas { get; set; }
        public decimal ValorParcela { get; set; }

        public Pagamento(Guid id, Guid servicoId, int tipoPagamento, int quantidadeParcelas, decimal valorParcela)
        {
            Id = id;
            ServicoId = servicoId;
            TipoPagamento = tipoPagamento;
            QuantidadeParcelas = quantidadeParcelas;
            ValorParcela = valorParcela;
        }

        public static Result<Pagamento> Criar(Guid servicoId, int tipoPagamento, int quantidadeParcelas, decimal valorParcela)
        {
            var pagamento = new Pagamento(Guid.NewGuid(), servicoId, tipoPagamento, quantidadeParcelas, valorParcela);

            return pagamento;
        }
    }
}
