using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AplicativoCinema.WebApi.Infraestrutura
{
    public class SessoesRepositorio
    {
        private readonly CinemasDbContext _cinemasDbContext;
        private readonly IConfiguration _configuracao;

        public SessoesRepositorio(CinemasDbContext cinemasDbContext, IConfiguration configuracao)
        {
            _cinemasDbContext = cinemasDbContext;
            _configuracao = configuracao;
        }
    }
}
