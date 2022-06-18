using AplicacaoAnuncio.Dominio;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AplicacaoAnuncio.Infraestrutura.EntityConfigurations
{
    public class EnderecoTypeConfiguration : IEntityTypeConfiguration<Endereco>
    {
        public void Configure(EntityTypeBuilder<Endereco> builder)
        {
            builder
                .ToTable("Enderecos", "dbo");

            builder
                .HasKey(c => c.Id);

            builder
                .HasOne<Usuario>()
                .WithMany()
                .HasForeignKey(c => c.UsuarioId);

            builder
                .Property(c => c.Cep)
                .HasColumnName("Cep")
                .HasColumnType("char(9)");

            builder
                .Property(c => c.Estado)
                .HasColumnName("Estado")
                .HasColumnType("char(2)");

            builder
                .Property(c => c.Cidade)
                .HasColumnName("Cidade")
                .HasColumnType("varchar(50)");

            builder
                .Property(c => c.Logradouro)
                .HasColumnName("Logradouro")
                .HasColumnType("varchar(50)");

            builder
                .Property(c => c.Numero)
                .HasColumnName("Numero")
                .HasColumnType("int");

            builder
                .Property(c => c.Bairro)
                .HasColumnName("Bairro")
                .HasColumnType("varchar(50)");

            builder
                .Property<DateTime>("DataUltimaAlteracao");

            builder
                .Property<DateTime>("DataCadastro");
        }
    }
}
