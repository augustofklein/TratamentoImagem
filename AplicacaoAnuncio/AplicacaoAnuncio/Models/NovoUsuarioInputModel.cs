using System.ComponentModel.DataAnnotations;

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

        [Required(ErrorMessage = "O tipo do usuário é obrigatório")]
        public int TipoUsuario { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória")]
        public string Senha { get; set; }

        [Required(ErrorMessage = "O email é obrigatório")]
        public string Email { get; set; }
    }
}