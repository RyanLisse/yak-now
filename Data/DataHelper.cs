using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;


namespace YakShop.Data
{
    public static class DataHelper
    {
        public static async Task ManageDataAsync(IServiceProvider svcProvider)
        {
            await MigrateDatabaseAsync(svcProvider);
        }

        private static async Task MigrateDatabaseAsync(IServiceProvider svcProvider)
        {
            var dbContextSvc = svcProvider.GetRequiredService<YakShopContext>();
            await dbContextSvc.Database.MigrateAsync();
        }
    }
}

