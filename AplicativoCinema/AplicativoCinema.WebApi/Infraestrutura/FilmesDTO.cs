using AplicativoCinema.WebApi.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AplicativoCinema.WebApi.Infraestrutura
{
    public class FilmesDTO
    {
        public Guid Id { get; set; }
        public string Titulo { get; set; }
        public int Duracao { get; set; }
        public string Sinopse { get; set; }

        public class SessaoDTO
        {
            public Guid Id { get; set; }
            public EDiaSemana DiaSemana { get; set; }
            public Horario HorarioInicial { get; set; }
            public int QuantidadeLugares { get; set; }
            public float Preco { get; set; }
        }
    }
}
