using System.ComponentModel.DataAnnotations;

namespace AplicacaoAnuncio.Models
{
    public class NovoPagamentoInputModel
    {
        [Required(ErrorMessage = "O código identificador do servico é obrigatório")]
        public string ServicoId { get; set; }

        [Required(ErrorMessage = "O tipo do pagamento é obrigatório")]
        public int TipoPagamento { get; set; }

        [Required(ErrorMessage = "A quantidade de parcelas é obrigatório")]
        public int QuantidadeParcelas { get; set; }

        [Required(ErrorMessage = "O valor da parcela é obrigatório")]
        public decimal ValorParcela { get; set; }
    }
}
