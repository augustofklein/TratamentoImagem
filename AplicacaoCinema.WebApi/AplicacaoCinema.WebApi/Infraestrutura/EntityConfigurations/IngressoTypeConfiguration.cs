using AplicacaoCinema.WebApi.Dominio;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AplicacaoCinema.WebApi.Infraestrutura.EntityConfigurations
{
    public class IngressoTypeConfiguration : IEntityTypeConfiguration<Ingresso>
    {
        public void Configure(EntityTypeBuilder<Ingresso> builder)
        {
            builder
                .ToTable("Ingressos", "dbo");

            builder
                .HasKey(c => c.Id);

            builder
                .HasOne<Sessao>()
                .WithMany()
                .HasForeignKey(c => c.SessaoId);

            builder
                .Property(c => c.QuantidadeIngressos)
                .HasColumnName("QuantidadeIngressos")
                .HasColumnType("int");

            builder
                .Property<DateTime>("DataUltimaAlteracao");

            builder
                .Property<DateTime>("DataCadastro");
        }
    }
}
