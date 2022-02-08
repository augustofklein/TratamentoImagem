using AplicacaoCinema.WebApi.Dominio;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AplicacaoCinema.WebApi.Infraestrutura.EntityConfigurations
{
    public class FilmeTypeConfiguration : IEntityTypeConfiguration<Filme>
    {
        public void Configure(EntityTypeBuilder<Filme> builder)
        {
            builder
                .ToTable("Filmes", "dbo");

            builder
                .HasKey(c => c.Id);

            builder
                .Property(c => c.Titulo)
                .HasColumnName("Titulo")
                .HasColumnType("varchar(50)");

            builder
                .Property(c => c.Duracao)
                .HasColumnName("Duracao")
                .HasColumnType("int");

            builder
                .Property(c => c.Sinopse)
                .HasColumnName("Sinopse")
                .HasColumnType("varchar(50)");

            builder
                .Property<DateTime>("DataUltimaAlteracao");

            builder
                .Property<DateTime>("DataCadastro");
        }
    }
}
