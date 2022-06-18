using AplicacaoAnuncio.Dominio;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AplicacaoAnuncio.Infraestrutura.EntityConfigurations
{
    public class AvaliacaoTypeConfiguration : IEntityTypeConfiguration<Avaliacao>
    {
        public void Configure(EntityTypeBuilder<Avaliacao> builder)
        {
            builder
                .ToTable("Avaliacoes", "dbo");

            builder
                .HasKey(c => c.Id);

            builder
                .HasOne<Servico>()
                .WithMany()
                .HasForeignKey(c => c.ServicoId);

            builder
                .Property(c => c.Nota)
                .HasColumnName("Nota")
                .HasColumnType("int");

            builder
                .Property<DateTime>("DataUltimaAlteracao");

            builder
                .Property<DateTime>("DataCadastro");
        }
    }
}
