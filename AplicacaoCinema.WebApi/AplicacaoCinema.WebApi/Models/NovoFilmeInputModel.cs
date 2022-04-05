using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AplicacaoCinema.WebApi.Models
{
    public class NovoFilmeInputModel
    {
        [Required(ErrorMessage = "O título é um campo obrigatório")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "A duração é um campo obrigatório")]
        public int Duracao { get; set; }

        [Required(ErrorMessage = "A sinopse é um campo obrigatório")]
        public string Sinopse { get; set; }
    }
}
