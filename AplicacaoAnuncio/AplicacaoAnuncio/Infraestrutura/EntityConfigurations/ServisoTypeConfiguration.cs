using AplicacaoAnuncio.Dominio;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AplicacaoAnuncio.Infraestrutura.EntityConfigurations
{
    public class ServicoTypeConfiguration : IEntityTypeConfiguration<Servico>
    {
        public void Configure(EntityTypeBuilder<Servico> builder)
        {
            builder
                .ToTable("Servicos", "dbo");

            builder
                .HasKey(c => c.Id);

            builder
                .HasOne<Usuario>()
                .WithMany()
                .HasForeignKey(c => c.UsuarioId);

            builder
                .Property(c => c.NomeServico)
                .HasColumnName("NomeServico")
                .HasColumnType("varchar(50)");

            builder
                .Property(c => c.Descricao)
                .HasColumnName("Descricao")
                .HasColumnType("varchar(50)");

            builder
                .Property(c => c.Categoria)
                .HasColumnName("Categoria")
                .HasColumnType("int");

            builder
                .Property(c => c.Valor)
                .HasColumnName("Valor")
                .HasColumnType("decimal");

            builder
                .Property<DateTime>("DataUltimaAlteracao");

            builder
                .Property<DateTime>("DataCadastro");

        }
    }
}
