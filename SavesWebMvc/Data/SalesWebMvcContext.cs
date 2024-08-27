using Microsoft.EntityFrameworkCore;
using salesWebMvc.Models;
using SalesWebMvc.Models;

namespace SalesWebMvc.Models
{
    public class SalesWebMvcContext : DbContext
    {
        public SalesWebMvcContext(DbContextOptions<SalesWebMvcContext> options)
            : base(options)
        {
        }

        public DbSet<Department> Department { get; set; }
        public DbSet<Seller> Seller { get; set; }
        public DbSet<SalesRecord> SalesRecord { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Venda> Vendas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurações de relacionamento, se necessário
            modelBuilder.Entity<Produto>()
                .HasOne(p => p.Departamento)
                .WithMany(d => d.Produtos)
                .HasForeignKey(p => p.DepartamentoId);

            modelBuilder.Entity<Venda>()
                .HasOne(v => v.Produto)
                .WithMany(p => p.Vendas)
                .HasForeignKey(v => v.ProdutoId);

            modelBuilder.Entity<Venda>()
                .HasOne(v => v.Vendedor)
                .WithMany(v => v.Vendas)
                .HasForeignKey(v => v.VendedorId);

            modelBuilder.Entity<Venda>()
                .HasOne(v => v.Departamento)
                .WithMany(d => d.Vendas)
                .HasForeignKey(v => v.DepartamentoId);
        }
    }
}
