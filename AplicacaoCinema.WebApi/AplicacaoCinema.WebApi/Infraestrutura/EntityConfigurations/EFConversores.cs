using AplicacaoCinema.WebApi.Dominio;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AplicacaoCinema.WebApi.Infraestrutura.EntityConfigurations
{
    public static class EFConversores
    {
        public static readonly ValueConverter<Horario, string> HorarioConverter
            = new ValueConverter<Horario, string>(
                horario => horario.ToString(),
                valorDB => Horario.Criar(valorDB).Value);
    }
}
