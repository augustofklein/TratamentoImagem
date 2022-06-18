using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AplicacaoAnuncio.Models
{
    public class NovoEnderecoInputModel
    {
        [Required(ErrorMessage = "O código do usuário é obrigatório")]
        public string UsuarioId { get; set; }

        [Required(ErrorMessage = "O código do CEP é obrigatório")]
        public string Cep { get; set; }

        [Required(ErrorMessage = "A informação do estado é obrigatória")]
        public string Estado { get; set; }

        [Required(ErrorMessage = "A informação da cidade é obrigatória")]
        public string Cidade { get; set; }

        [Required(ErrorMessage = "A informação do logradouro é obrigatória")]
        public string Logradouro { get; set; }

        [Required(ErrorMessage = "A informação do número é obrigatória")]
        public int Numero { get; set; }

        [Required(ErrorMessage = "A informação do bairro é obrigatória")]
        public string Bairro { get; set; }
    }
}
