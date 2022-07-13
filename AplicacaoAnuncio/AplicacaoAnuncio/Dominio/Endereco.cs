using CSharpFunctionalExtensions;
using System;

namespace AplicacaoAnuncio.Dominio
{
    public class Endereco
    {
        public Guid Id { get; set; }
        public Guid UsuarioId { get; set; }
        public string Cep { get; set; }
        public string Estado { get; set; }
        public string Cidade { get; set; }
        public string Logradouro { get; set; }
        public int Numero { get; set; }
        public string Bairro { get; set; }

        public Endereco(Guid id, Guid usuarioId, string cep, string estado, string cidade, string logradouro, int numero, string bairro)
        {
            Id = id;
            UsuarioId = usuarioId;
            Cep = cep;
            Estado = estado;
            Cidade = cidade;
            Logradouro = logradouro;
            Numero = numero;
            Bairro = bairro;
        }

        public static Result<Endereco> Criar(Guid usuarioId, string cep, string estado, string cidade, string logradouro, int numero, string bairro)
        {
            var endereco = new Endereco(Guid.NewGuid(), usuarioId, cep, estado, cidade, logradouro, numero, bairro);
            return endereco;
        }
    }
}
