using AplicativoCinema.WebApi.Dominio;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace AplicativoCinema.WebApi.Infraestrutura.Mappers
{
    public class HorarioTypeHandler: SqlMapper.TypeHandler<Horario>
    {
        public override void SetValue(IDbDataParameter parameter, Horario value)
        {
            parameter.Value = value.ToString();
        }

        public override Horario Parse(object value)
        {
            var time = ((string)value).Split(":");
            if (time.Length != 2)
                throw new FormatException("Hora especificada no banco em formato inválida");
            return new Horario(Int32.Parse(time[0]), Int32.Parse(time[1]));
        }
    }
}
