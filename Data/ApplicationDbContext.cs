using Gerenciador_de_Produtos.Models;
using Microsoft.EntityFrameworkCore;

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
        public DbSet<RevisaoItemERP> RevisaoItemERPs { get; set; }
        public DbSet<PerfilItemERP> PerfilItemERPs { get; set; }
        public DbSet<RevisaoPerfilItemERP> RevisaoPerfilItemERPs { get; set; }
        public DbSet<VariavelProduto> VariaveisProdutos { get; set; }
        public DbSet<VariavelAgrupador> VariaveisAgrupadores { get; set; }
        public DbSet<VariavelComponente> VariaveisComponentes { get; set; }
        public DbSet<Tag> Tags { get; set; }

        public DbSet<Desenho> Desenhos { get; set; }
        public DbSet<DesenhoItemERP> DesenhoItemERPs { get; set; }
        public DbSet<Perfil> Perfis { get; set; }
        public DbSet<ItemERP> ItensERP { get; set; }
        public DbSet<Secao> Secao { get; set; }

        public DbSet<Secao> Secoes { get; set; }

        public DbSet<Equacao> Equacao { get; set; }
        public DbSet<Norma> Norma { get; set; }

        // Novo DbSet para itens relacionados
        public DbSet<ItemERPRelacionado> ItemERPRelacionados { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Agrupador ⇄ ItemERP
            modelBuilder.Entity<AgrupadorItemERP>()
                .HasOne(a => a.Agrupador)
                .WithMany(g => g.AgrupadorItensERP)
                .HasForeignKey(a => a.AgrupadorId);

            modelBuilder.Entity<AgrupadorItemERP>()
                .HasOne(a => a.ItemERP)
                .WithMany(e => e.AgrupadorItensERP)
                .HasForeignKey(a => a.ItemERPId);

            // === AgrupadorComponente ===
            modelBuilder.Entity<AgrupadorComponente>(entity =>
            {
                // mantém o Id como PK
                entity.HasKey(ac => ac.Id);

                // FK para Agrupador
                entity.HasOne(ac => ac.Agrupador)
                      .WithMany(a => a.AgrupadorComponentes)
                      .HasForeignKey(ac => ac.AgrupadorId)
                      .OnDelete(DeleteBehavior.Cascade);

                // FK para Componente
                entity.HasOne(ac => ac.Componente)
                      .WithMany(c => c.AgrupadorComponentes)
                      .HasForeignKey(ac => ac.ComponenteId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Componente ⇄ ItemERP
            modelBuilder.Entity<ComponenteItemERP>()
                .HasOne(ci => ci.Componente)
                .WithMany(c => c.ComponenteItensERP)
                .HasForeignKey(ci => ci.ComponenteId);

            modelBuilder.Entity<ComponenteItemERP>()
                .HasOne(ci => ci.ItemERP)
                .WithMany(i => i.ComponenteItemERPs)
                .HasForeignKey(ci => ci.ItemERPId);

            // ItemERP ⇄ Desenhos
            modelBuilder.Entity<DesenhoItemERP>()
                .HasKey(di => new { di.DesenhoId, di.ItemERPId });

            modelBuilder.Entity<DesenhoItemERP>()
                .HasOne(di => di.Desenho)
                .WithMany(d => d.DesenhoItemERPs)
                .HasForeignKey(di => di.DesenhoId);

            modelBuilder.Entity<DesenhoItemERP>()
                .HasOne(di => di.ItemERP)
                .WithMany(e => e.DesenhoItemERPs)
                .HasForeignKey(di => di.ItemERPId);


            // ItemERP ⇄ Revisões do Item
            modelBuilder.Entity<RevisaoItemERP>()
                .HasOne(r => r.ItemERP)
                .WithMany(i => i.Revisoes)
                .HasForeignKey(r => r.ItemERPId)
                .OnDelete(DeleteBehavior.Cascade);

            // ItemERP ⇄ PerfilItemERP (cascade delete)
            modelBuilder.Entity<PerfilItemERP>()
                .HasOne(pi => pi.ItemERP)
                .WithMany(i => i.PerfilItemERPs)
                .HasForeignKey(pi => pi.ItemERPId)
                .OnDelete(DeleteBehavior.Cascade);

            // PerfilItemERP ⇄ Perfil (sem cascade delete para evitar caminhos múltiplos)
            modelBuilder.Entity<PerfilItemERP>()
                .HasOne(pi => pi.Perfil)
                .WithMany(p => p.PerfilItemERPs)
                .HasForeignKey(pi => pi.PerfilId)
                .OnDelete(DeleteBehavior.Restrict);

            // PerfilItemERP ⇄ RevisaoPerfilItemERP (cascade delete)
            modelBuilder.Entity<RevisaoPerfilItemERP>()
                .HasOne(rp => rp.PerfilItemERP)
                .WithMany(pi => pi.Revisoes)
                .HasForeignKey(rp => rp.PerfilItemERPId)
                .OnDelete(DeleteBehavior.Cascade);

            // ItemERPRelacionado ⇄ ItemERP (pai)
            modelBuilder.Entity<ItemERPRelacionado>()
                .HasOne(r => r.ItemERP)
                .WithMany(i => i.ItensRelacionados)
                .HasForeignKey(r => r.ItemERPId)
                .OnDelete(DeleteBehavior.Cascade);

            // ItemERPRelacionado ⇄ ItemERP (relacionado)
            modelBuilder.Entity<ItemERPRelacionado>()
                .HasOne(r => r.Relacionado)
                .WithMany()  // sem navegação inversa
                .HasForeignKey(r => r.RelacionadoId)
                .OnDelete(DeleteBehavior.Restrict);

            // ItemERPRelacionado ⇄ Desenho
            modelBuilder.Entity<ItemERPRelacionado>()
                .HasOne(r => r.Desenho)
                .WithMany()
                .HasForeignKey(r => r.DesenhoId)
                .OnDelete(DeleteBehavior.Restrict);

            // Perfil ⇄ Desenho (muitos-para-muitos via DesenhoPerfil)
            modelBuilder.Entity<DesenhoPerfil>()
                .HasKey(dp => new { dp.PerfilId, dp.DesenhoId });

            modelBuilder.Entity<DesenhoPerfil>()
                .HasOne(dp => dp.Perfil)
                .WithMany(p => p.DesenhosPerfil)
                .HasForeignKey(dp => dp.PerfilId);

            modelBuilder.Entity<DesenhoPerfil>()
                .HasOne(dp => dp.Desenho)
                .WithMany()
                .HasForeignKey(dp => dp.DesenhoId);



        }
    }
}
