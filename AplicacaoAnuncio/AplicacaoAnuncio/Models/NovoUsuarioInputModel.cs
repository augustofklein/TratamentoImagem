using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AplicacaoAnuncio.Models
{
    public class NovoUsuarioInputModel
    {
        [Required(ErrorMessage = "O CPF é obrigatório")]
        public string Cpf { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "A data de nascimento é obrigatória")]
        public string DataNascimento { get; set; }

        [Required(ErrorMessage = "O sexo é obrigatório")]
        public char Sexo { get; set; }
    }
}
