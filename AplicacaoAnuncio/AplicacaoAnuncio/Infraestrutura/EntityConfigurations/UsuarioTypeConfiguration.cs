using AplicacaoAnuncio.Dominio;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AplicacaoAnuncio.Infraestrutura.EntityConfigurations
{
    public class UsuarioTypeConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder
                .ToTable("Usuarios", "dbo");

            builder
                .HasKey(c => c.Id);

            builder
                .Property(c => c.Cpf)
                .HasColumnName("Cpf")
                .HasColumnType("varchar(18)");

            builder
                .Property(c => c.Nome)
                .HasColumnName("Nome")
                .HasColumnType("varchar(50)");

            builder
                .Property(c => c.DataNascimento)
                .HasColumnName("DataNascimento")
                .HasColumnType("char(10)");

            builder
                .Property(c => c.Sexo)
                .HasColumnName("Sexo")
                .HasColumnType("char(1)");

            builder
                .Property(c => c.TipoUsuario)
                .HasColumnName("TipoUsuario")
                .HasColumnType("int");

            builder
                .Property(c => c.Senha)
                .HasColumnName("Senha")
                .HasColumnType("char(15)");

            builder
                .Property(c => c.Email)
                .HasColumnName("Email")
                .HasColumnType("char(50)");

            builder
                .Property<DateTime>("DataUltimaAlteracao");

            builder
                .Property<DateTime>("DataCadastro");
        }
    }
}
