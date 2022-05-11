using Flunt.Notifications;
using LojaAPI.Dominio.Produtos;
using Microsoft.EntityFrameworkCore;

namespace LojaAPI.Infra.Database; //namespace: organiza programa em módulos

public class ApplicationDbContext : DbContext
{
    public DbSet<Produto> Produtos { get; set; }
    public DbSet<Categoria> Categorias { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {
    }
    protected override void OnModelCreating(ModelBuilder builder) {
        builder.Ignore<Notification>(); //não salvar dados da entidade notification do flunt
        builder.Entity<Produto>().Property(p => p.Nome).IsRequired();
        builder.Entity<Produto>().Property(p => p.Descricao).HasMaxLength(255); //adicionando exceção para convenção abaixo, nesse caso descricaõ terá 255 caracteres
        builder.Entity<Categoria>().Property(c => c.Nome).IsRequired();
    }
    protected override void ConfigureConventions(ModelConfigurationBuilder configuration) {
        configuration.Properties<string>().HaveMaxLength(120); //conveção para dizer que todos os atributos do tipo string deva ter somente 120 caracteres
    }
}
 