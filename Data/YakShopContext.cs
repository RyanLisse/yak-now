using Microsoft.EntityFrameworkCore;
using YakShop.Models;

namespace YakShop.Data
{
  public class YakShopContext : DbContext
  {
    public YakShopContext(DbContextOptions<YakShopContext> options) : base(options)
    {
    }

    public DbSet<Yak> Yaks { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Stock> Stocks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<Yak>().ToTable("Yak");
      modelBuilder.Entity<Order>().ToTable("Order");
      modelBuilder.Entity<Stock>().ToTable("Stock");
    }
  }
}
