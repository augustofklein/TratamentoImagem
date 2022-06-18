using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AplicacaoAnuncio.Dominio
{
    public class Usuario
    {
        public Guid Id { get; set; }
        public string Cpf { get; set; }
        public string Nome { get; set; }
        public string DataNascimento { get; set; }
        public char Sexo { get; set; }

        public Usuario(Guid id, string cpf, string nome, string dataNascimento, char sexo)
        {
            Id = id;
            Cpf = cpf;
            Nome = nome;
            DataNascimento = dataNascimento;
            Sexo = sexo;
        }

        public static Result<Usuario> Criar(string cpf, string nome, string dataNascimento, char sexo)
        {
            var usuario = new Usuario(Guid.NewGuid(), cpf, nome, dataNascimento, sexo);

            return usuario;
        }
    }
}
