using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AplicativoCinema.WebApi.Models
{
    public sealed class NovaSessaoInputModel
    {
        public string IdFilme { get; set; }
        public int DiaSemana { get; set; }
        public string Horario { get; set; }
        public int QuantidadeLugares { get; set; }
        public float Preco { get; set; }
        public int TotalIngressos { get; set; }
    }
}
