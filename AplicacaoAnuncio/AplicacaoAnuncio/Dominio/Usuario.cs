using CSharpFunctionalExtensions;
using System;

namespace AplicacaoAnuncio.Dominio
{
    public class Usuario
    {
        public Guid Id { get; set; }
        public string Cpf { get; set; }
        public string Nome { get; set; }
        public string DataNascimento { get; set; }
        public char Sexo { get; set; }
        public int TipoUsuario { get; set; }
        public string Senha { get; set; }
        public string Email { get; set; }

        public Usuario(Guid id, string cpf, string nome, string dataNascimento, char sexo, int tipoUsuario, string senha, string email)
        {
            Id = id;
            Cpf = cpf;
            Nome = nome;
            DataNascimento = dataNascimento;
            Sexo = sexo;
            TipoUsuario = tipoUsuario;
            Senha = senha;
            Email = email;
        }

        public static Result<Usuario> Criar(string cpf, string nome, string dataNascimento, char sexo, int tipoUsuario, string senha, string email)
        {
            var usuario = new Usuario(Guid.NewGuid(), cpf, nome, dataNascimento, sexo, tipoUsuario, senha, email);

            return usuario;
        }
    }
}
