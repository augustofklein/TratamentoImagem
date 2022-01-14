using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AplicativoCinema.WebApi.Infraestrutura.Mappers;
using Dapper;
using Microsoft.Extensions.DependencyInjection;

namespace AplicativoCinema.WebApi.Hosting.Extensions
{
    public static class DapperExtensions
    {
        public static IServiceCollection AddDapper(this IServiceCollection serviceCollection)
        {
            SqlMapper.AddTypeHandler(new GuidTypeHandler());
            SqlMapper.AddTypeHandler(new HorarioTypeHandler());
            return serviceCollection;
        }
    }
}
