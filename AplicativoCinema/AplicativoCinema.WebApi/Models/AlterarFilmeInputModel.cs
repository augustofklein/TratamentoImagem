using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AplicativoCinema.WebApi.Models
{
    public class AlterarFilmeInputModel
    {
        [Required]
        public string Titulo { get; set; }
        public int Duracao { get; set; }
        public string Sinopse { get; set; }
        public List<SessaoInputModel> Sessao { get; set; }

        public sealed class SessaoInputModel
        {
            public string Id { get; set; }
            public int DiaSemana { get; set; }
            public string HorarioInicial { get; set; }
            public int QuantidadeLugares { get; set; }
            public float Preco { get; set; }
        }
    }
}
