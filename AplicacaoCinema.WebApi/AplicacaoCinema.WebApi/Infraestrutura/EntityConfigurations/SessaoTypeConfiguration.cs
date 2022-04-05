using AplicacaoCinema.WebApi.Dominio;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AplicacaoCinema.WebApi.Infraestrutura.EntityConfigurations
{
    public class SessaoTypeConfiguration : IEntityTypeConfiguration<Sessao>
    {
        public void Configure(EntityTypeBuilder<Sessao> builder)
        {
            builder
                .ToTable("Sessoes", "dbo");

            builder
                .HasKey(c => c.Id);

            builder
                .HasOne<Filme>()
                .WithMany()
                .HasForeignKey(c => c.FilmeId);

            builder
                .Property(c => c.DiaSemana)
                .HasConversion(new EnumToStringConverter<EDiaSemana>())
                .HasColumnName("DiaSemana")
                .HasColumnType("varchar(8)");

            builder
                .Property(c => c.HorarioInicial)
                .HasConversion(EFConversores.HorarioConverter)
                .HasColumnName("HorarioInicial")
                .HasColumnType("varchar(5)");

            builder
                .Property(c => c.Preco)
                .HasColumnName("Preco")
                .HasColumnType("decimal");

            builder
                .Property(c => c.TotalIngressos)
                .HasColumnName("TotalIngressos")
                .HasColumnType("int");

            builder
                .Property<DateTime>("DataUltimaAlteracao");

            builder
                .Property<DateTime>("DataCadastro");
        }
    }
}
