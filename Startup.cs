using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using YakShop.Seeding;
using YakShop.Data;
using YakShop.Services;

namespace YakShop
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
      services.AddControllers();

      services.AddDbContext<YakShopContext>(options =>
          options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));
      Console.WriteLine(Configuration.GetConnectionString("DefaultConnection"));

      services.AddScoped<IYakService, YakService>();
      services.AddScoped<IOrderService, OrderService>();
      services.AddScoped<IStockService, StockService>();
      services.AddHostedService<DbSeeder>();

    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      else
      {
        app.UseExceptionHandler("/Home/Error");
        app.UseHsts();
      }

      app.UseHttpsRedirection();
      app.UseStaticFiles();

      app.UseRouting();

      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }
  }
}
