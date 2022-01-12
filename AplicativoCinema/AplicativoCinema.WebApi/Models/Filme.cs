using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AplicativoCinema.WebApi.Models
{
    /*
     * O comando SEALED não permite mais a classe ser herdada. Ninguém mais abaixo
     * de turma poderá existir.
     */
    public sealed class Filme
    {
        public string Id { get; set; }

        [Required]
        public string Titulo { get; set; }

        [Required]
        public int Duracao { get; set; }

        [Required]
        public string Sinopse { get; set; }

        public List<Sessao> Sessao { get; set; }
    }

    public sealed class Sessao
    {
        public EDiaSemana DiasSemana { get; set; }

        public Horario HorarioInicio { get; set; }

        public int QuantidadeLugares { get; set; }

        public float Preco { get; set; }

        public enum EDiaSemana
        {
            Domingo = 1,
            Segunda = 2,
            Terca   = 3,
            Quarta  = 4,
            Quinta  = 5,
            Sexta   = 6,
            Sabado  = 7
        }
    }

    public struct Horario
    {
        public int Hora { get; set; }

        public int Minuto { get; set; }

        public override string ToString()
        {
            return $"{Hora}:{Minuto}";
        }
    }
}
