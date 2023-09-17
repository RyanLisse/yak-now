using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using YakShop.Data;
using YakShop.Services;
using Microsoft.OpenApi.Models; // Added for Swagger
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
      // Controllers
      services.AddControllers();

      // Database Context
      services.AddDbContext<YakShopContext>(options =>
          options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

      // Services
      services.AddScoped<IYakService, YakService>();
      services.AddScoped<IOrderService, OrderService>();
      services.AddScoped<IStockService, StockService>();

      // Swagger
      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "YakShop API", Version = "v1" });
      });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      // Environment-specific middlewares
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      else
      {
        app.UseExceptionHandler("/Home/Error");
        app.UseHsts();
      }

      // Basic middlewares
      app.UseHttpsRedirection();
      app.UseStaticFiles();
      app.UseRouting();
      app.UseAuthorization();

      // Swagger
      app.UseSwagger();
      app.UseSwaggerUI(c =>
      {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "YakShop API v1");
        c.RoutePrefix = string.Empty;
      });

      // Endpoints
      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }
  }
}
