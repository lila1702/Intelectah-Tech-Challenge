using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using CarDealershipManager.Core.Models;
using CarDealershipManager.Infrastructure.Identity;

namespace CarDealershipManager.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Fabricante> Fabricantes { get; set; }
        public DbSet<Veiculo> Veiculos { get; set; }
        public DbSet<Concessionaria> Concessionarias { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Venda> Vendas { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configurações de Fabricante
            builder.Entity<Fabricante>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Nome).IsUnique();
                entity.Property(e => e.Nome).IsRequired().HasMaxLength(100);
                entity.Property(e => e.PaisOrigem).HasMaxLength(50);
                entity.Property(e => e.Website).HasMaxLength(255);

                // Filtro de Soft Delete Global
                entity.HasQueryFilter(e => !e.IsDeleted);
            });

            // Configurações de Veículo
            builder.Entity<Veiculo>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Modelo);
                entity.Property(e => e.Modelo).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Preco).HasPrecision(10, 2);
                entity.Property(e => e.Descricao).HasMaxLength(500);

                entity.HasOne(e => e.Fabricante)
                    .WithMany(f => f.Veiculos)
                    .HasForeignKey(e => e.FabricanteId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasQueryFilter(e => !e.IsDeleted);
            });

            // Configurações de Concessionária
            builder.Entity<Concessionaria>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Nome).IsUnique();
                entity.Property(e => e.Nome).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Endereco).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Cidade).HasMaxLength(50).IsRequired();
                entity.Property(e => e.Estado).HasMaxLength(50).IsRequired();
                entity.Property(e => e.CEP).HasMaxLength(10).IsRequired();
                entity.Property(e => e.Telefone).HasMaxLength(15).IsRequired();
                entity.Property(e => e.Email).HasMaxLength(100).IsRequired();

                entity.HasQueryFilter(e => !e.IsDeleted);
            });

            // Configurações de Cliente
            builder.Entity<Cliente>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.CPF).IsUnique();
                entity.Property(e => e.Nome).IsRequired().HasMaxLength(100);
                entity.Property(e => e.CPF).IsRequired().HasMaxLength(11);
                entity.Property(e => e.Telefone).HasMaxLength(15);

                entity.HasQueryFilter(e => !e.IsDeleted);
            });

            // Configurações de Venda
            builder.Entity<Venda>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.PrecoVenda).HasPrecision(10, 2);
                entity.HasIndex(e => e.ProtocoloVenda).IsUnique();
                entity.Property(e => e.ProtocoloVenda).IsRequired().HasMaxLength(20);

                entity.HasOne(e => e.Veiculo)
                    .WithMany(v => v.Vendas)
                    .HasForeignKey(e => e.VeiculoId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Concessionaria)
                    .WithMany(c => c.Vendas)
                    .HasForeignKey(e => e.ConcessionariaId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Cliente)
                    .WithMany(c => c.Vendas)
                    .HasForeignKey(e => e.ClienteId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasQueryFilter(e => !e.IsDeleted);
            });
        }

        public override int SaveChanges()
        {
            UpdateTimeStamps();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimeStamps();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateTimeStamps()
        {
            var entries = ChangeTracker.Entries().Where(e => e.Entity is BaseModel && (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entry in entries)
            {
                var entity = (BaseModel)entry.Entity;

                if (entry.State == EntityState.Added)
                {
                    entity.CreatedAt = DateTime.UtcNow;
                }

                else if (entry.State == EntityState.Modified)
                {
                    entity.UpdatedAt = DateTime.UtcNow;
                }
            }
        }
    }
}
