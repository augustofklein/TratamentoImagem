using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AplicativoCinema.WebApi.Models
{
    public class NovoFilmeInputModel
    {
        [Required]
        public string Titulo { get; set; }
        public int Duracao { get; set; }
        public string Sinopse { get; set; }
    }
}
