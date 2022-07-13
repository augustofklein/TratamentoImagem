using AplicacaoAnuncio.Dominio;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace AplicacaoAnuncio.Infraestrutura.EntityConfigurations
{
    public class PagamentoTypeConfiguration : IEntityTypeConfiguration<Pagamento>
    {
        public void Configure(EntityTypeBuilder<Pagamento> builder)
        {
            builder
                .ToTable("Pagamentos", "dbo");

            builder
                .HasKey(c => c.Id);

            builder
                .HasOne<Servico>()
                .WithMany()
                .HasForeignKey(c => c.ServicoId);

            builder
                .Property(c => c.TipoPagamento)
                .HasColumnName("TipoPagamento")
                .HasColumnType("int");

            builder
                .Property(c => c.QuantidadeParcelas)
                .HasColumnName("QuantidadeParcelas")
                .HasColumnType("int");

            builder
                .Property(c => c.ValorParcela)
                .HasColumnName("ValorParcela")
                .HasColumnType("decimal");

            builder
                .Property<DateTime>("DataUltimaAlteracao");

            builder
                .Property<DateTime>("DataCadastro");
        }
    }
}
