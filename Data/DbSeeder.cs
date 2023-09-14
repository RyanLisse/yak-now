using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using System.Threading;
using YakShop.Data;
using YakShop.Models;

namespace YakShop.Seeding
{
    public class DbSeeder : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public DbSeeder(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<YakShopContext>();

                if (context.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
                {
                    // Ensure the database is created
                    context.Database.EnsureCreated();

                    // Check if there are already yaks in the database, and only seed if not
                    if (!context.Yaks.Any())
                    {
                        // Add your dummy data here
                        var yaks = new List<Yak>
                        {
                            new Yak { Name = "Yak-1", Age = 4, Sex = "FEMALE" },
                            new Yak { Name = "Yak-2", Age = 8, Sex = "FEMALE" },
                            new Yak { Name = "Yak-3", Age = 9.5, Sex = "FEMALE" },
                            // Add more yaks as needed
                        };

                        context.Yaks.AddRange(yaks);
                        await context.SaveChangesAsync();
                    }
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
