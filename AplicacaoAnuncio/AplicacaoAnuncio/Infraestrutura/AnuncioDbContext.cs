using AplicacaoAnuncio.Dominio;
using AplicacaoAnuncio.Infraestrutura.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AplicacaoAnuncio.Infraestrutura
{
    public class AnuncioDbContext : DbContext
    {
        public DbSet<Servico> Servicos { get; set; }
        public DbSet<Avaliacao> Avaliacoes { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Endereco> Enderecos { get; set; }

        public AnuncioDbContext(DbContextOptions options) : base(options)
        {
        
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                foreach (var item in ChangeTracker.Entries())
                {
                    if (item.State is Microsoft.EntityFrameworkCore.EntityState.Modified or Microsoft.EntityFrameworkCore.EntityState.Added
                        && item.Properties.Any(c => c.Metadata.Name == "DataUltimaAlteracao"))
                        item.Property("DataUltimaAlteracao").CurrentValue = DateTime.UtcNow;

                    if (item.State == EntityState.Added
                        && item.Properties.Any(c => c.Metadata.Name == "DataCadastro"))
                        item.Property("DataCadastro").CurrentValue = DateTime.UtcNow;
                }
                return await base.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException e)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ServicoTypeConfiguration());
            modelBuilder.ApplyConfiguration(new AvaliacaoTypeConfiguration());
            modelBuilder.ApplyConfiguration(new UsuarioTypeConfiguration());
            modelBuilder.ApplyConfiguration(new EnderecoTypeConfiguration());
        }
    }
}
