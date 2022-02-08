using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AplicacaoCinema.WebApi.Models
{
    public class NovoFilmeInputModel
    {
        public string Titulo { get; set; }
        public int Duracao { get; set; }
        public string Sinopse { get; set; }
    }
}
