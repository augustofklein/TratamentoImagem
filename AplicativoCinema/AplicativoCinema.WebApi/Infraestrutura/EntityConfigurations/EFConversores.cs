using AplicativoCinema.WebApi.Dominio;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AplicativoCinema.WebApi.Infraestrutura.EntityConfigurations
{
    public static class EFConversores
    {
        public static readonly ValueConverter<Horario, string> HorarioConverter
            = new ValueConverter<Horario, string>(
            horario => horario.ToString(),
            valorBD => Horario.Criar(valorBD).Value);
    }
}
