using System.ComponentModel.DataAnnotations;

namespace AplicacaoAnuncio.Models
{
    public class NovaAvaliacaoInputModel
    {
        [Required(ErrorMessage = "O código identificador do servico é obrigatório ")]
        public string ServicoId { get; set; }

        [Required(ErrorMessage = "O valor da avaliação é obrigatório")]
        public int Nota { get; set; }
    }
}
