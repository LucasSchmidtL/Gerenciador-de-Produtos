using Gerenciador_de_Produtos.Models;
using Microsoft.EntityFrameworkCore;
using Gerenciador_de_Produtos.Models;    

namespace Gerenciador_de_Produtos.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSets das entidades
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<AgrupadorProduto> ProdutoAgrupadores { get; set; }
        public DbSet<Agrupador> Agrupadores { get; set; }
        public DbSet<AgrupadorComponente> AgrupadorComponentes { get; set; }
        public DbSet<AgrupadorItemERP> AgrupadorItemERPs { get; set; }
        public DbSet<Componente> Componentes { get; set; }
        public DbSet<ComponenteItemERP> ComponenteItemERPs { get; set; }
        public DbSet<VariavelProduto> VariaveisProdutos { get; set; }
        public DbSet<VariavelAgrupador> VariaveisAgrupadores { get; set; }
        public DbSet<VariavelComponente> VariaveisComponentes { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Desenho> Desenhos { get; set; }
        public DbSet<Perfil> Perfis { get; set; }
        public DbSet<ItemERP> ItensERP { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurações adicionais, se precisar no futuro:
            // Exemplo de relacionamento customizado
            // modelBuilder.Entity<Produto>()
            //     .HasMany(p => p.ProdutoAgrupadores)
            //     .WithOne(pa => pa.Produto)
            //     .HasForeignKey(pa => pa.ProdutoId);

            // Por enquanto, o EF vai deduzir certinho só com os atributos
        }
        public DbSet<Gerenciador_de_Produtos.Models.Secao> Secao { get; set; } = default!;
        public DbSet<Gerenciador_de_Produtos.Models.Equacao> Equacao { get; set; } = default!;
        public DbSet<Gerenciador_de_Produtos.Models.Norma> Norma { get; set; } = default!;
    }
}
