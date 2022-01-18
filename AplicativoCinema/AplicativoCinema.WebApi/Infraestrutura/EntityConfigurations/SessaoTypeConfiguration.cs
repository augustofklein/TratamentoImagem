using AplicativoCinema.WebApi.Dominio;
using AplicativoCinema.WebApi.Infraestrutura.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AplicativoCinema.WebApi.Infraestrutura
{
    public sealed class SessaoTypeConfiguration : IEntityTypeConfiguration<Sessao>
    {
        public void Configure(EntityTypeBuilder<Sessao> builder)
        {
            builder.ToTable("Sessao", "dbo");
            builder.HasKey(c => c.Id);

            builder
                .Property(c => c.DiaSemana)
                .HasConversion(new EnumToStringConverter<EDiaSemana>())
                .HasColumnType("varchar(20)")
                .HasColumnName("DiaSemana");

            builder
                .Property(c => c.Horario)
                .HasConversion(EFConversores.HorarioConverter)
                .HasColumnType("varchar(5)")
                .HasColumnName("Horario");

            builder
                .HasOne<Filme>()
                .WithMany()
                .HasForeignKey(c => c.Id);

            builder
                .Property(c => c.TotalIngressos)
                .HasColumnType("int")
                .HasColumnName("TotalIngressos");

            builder
                .Property(c => c.Preco)
                .HasColumnType("decimal(12,2)")
                .HasColumnName("Preco");

            builder
                .Property<DateTime>("DataUltimaAlteracao");

            builder
                .Property<DateTime>("DataCadastro");
        }
    }
}
