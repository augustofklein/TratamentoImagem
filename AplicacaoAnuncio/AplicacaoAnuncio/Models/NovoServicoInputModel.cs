using System.ComponentModel.DataAnnotations;

namespace AplicacaoAnuncio.Models
{
    public class NovoServicoInputModel
    {
        [Required(ErrorMessage = "O código do usuário é obrigatório")]
        public string UsuarioId { get; set; }

        [Required(ErrorMessage ="O nome do serviço é obrigatório")]
        public string NomeServico { get; set; }

        [Required(ErrorMessage = "A descrição do serviço é obrigatória")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "A categoria do serviço é obrigatória")]
        public int Categoria { get; set; }

        [Required(ErrorMessage = "O valor do serviço é obrigatório")]
        public double Valor { get; set; }

    }
}
