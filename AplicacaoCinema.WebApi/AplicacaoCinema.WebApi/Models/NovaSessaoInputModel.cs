using AplicacaoCinema.WebApi.Dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AplicacaoCinema.WebApi.Models
{
    public class NovaSessaoInputModel
    {
        [Required(ErrorMessage = "O filme é um campo obrigatório")]
        public string FilmeId { get; set; }

        [Required(ErrorMessage = "O dia da semana é um campo obrigatório")]
        public int DiaSemana { get; set; }

        [Required(ErrorMessage = "O horario inicial é um campo obrigatório")]
        public string HorarioInicial { get; set; }
        
        [Required(ErrorMessage = "O preço é um campo obrigatório")]
        public double Preco { get; set; }

        [Required(ErrorMessage = "A quantidade de ingresos é obrigatório")]
        public int TotalIngressos { get; set; }
    }
}
