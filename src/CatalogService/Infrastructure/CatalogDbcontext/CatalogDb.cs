using Microsoft.EntityFrameworkCore;
using CatalogService.Domain.Entity;

namespace CatalogService.Infrastructure.CatalogDbcontext;

public class CatalogDb : DbContext

{
    public CatalogDb(DbContextOptions<CatalogDb> options) : base(options)
    {

    }
    public DbSet<Pizza> Pizzas { get; set; }
    public DbSet<Topping> Toppings { get; set; }
    public DbSet<PizzaTopping> PizzaToppings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PizzaTopping>().HasKey(pt => new { pt.PizzaId, pt.ToppingId });

        modelBuilder.Entity<PizzaTopping>()
        .HasOne(pt => pt.Pizza) // PizzaTopping *ссылаетс€ на* одну Pizza
        .WithMany(p => p.PizzaToppings) // у Pizza есть много PizzaTopping
        .HasForeignKey(pt => pt.PizzaId); // внешний ключ в PizzaTopping Ч PizzaId

        modelBuilder.Entity<PizzaTopping>()
            .HasOne(pt => pt.Topping)
            .WithMany(t => t.PizzaToppings)
            .HasForeignKey(pt => pt.ToppingId);

    }



}