using AplicativoCinema.WebApi.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AplicativoCinema.WebApi.Infraestrutura.Mappers
{
    public class SessoesDTO
    {
        public Guid Id { get; set; }
        public EDiaSemana DiaSemana { get; set; }
        public Horario HorarioInicial { get; set; }
        public int QuantidadeLugares { get; set; }
        public float Preco { get; set; }
        public int TotalIngressos { get; set; }
    }
}
