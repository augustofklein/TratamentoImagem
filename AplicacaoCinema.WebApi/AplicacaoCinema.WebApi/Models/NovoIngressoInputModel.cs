using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AplicacaoCinema.WebApi.Models
{
    public class NovoIngressoInputModel
    {
        [Required(ErrorMessage = "A sessão é obrigatória")]
        public string SessaoId { get; set; }

        [Required(ErrorMessage = "A quantidade de ingressos é obrigatória")]
        public int QuantidadeIngressos { get; set; }
    }
}
