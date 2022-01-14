using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AplicativoCinema.WebApi.Infraestrutura
{
    public class CinemasDbContext:DbContext
    {
        public CinemasDbContext(DbContextOptions options) : base(options)
        { 
            
        }
    }
}
