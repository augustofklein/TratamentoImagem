using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AplicacaoCinema.WebApi.Dominio
{
    public struct Horario
    {
        public int Hora { get; }
        public int Minuto { get; }

        public Horario(int hora, int minuto)
        {
            Hora = hora;
            Minuto = minuto;
        }

        public override string ToString()
        {
            return $"{Hora:D2}:{Minuto:D2}";
        }

        public static Result<Horario> Criar(string horario)
        {
            var time = horario.Split(":");
            if (time.Length != 2)
                return Result.Failure<Horario>("Hora especificada no banco em formato inválida");

            return new Horario(Int32.Parse(time[0]), Int32.Parse(time[1]));
        }
    }
}
