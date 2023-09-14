using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using YakShop.Data;
using YakShop.Models;

namespace YakShop.Services
{
  public class StockService : IStockService
  {
    private readonly YakShopContext _context;

    public StockService(YakShopContext context)
    {
      _context = context;
    }

    public async Task<Stock> GetStockByDayAsync(int day)
    {
      var stock = await _context.Stocks.FirstOrDefaultAsync(s => s.Day == day);

      if (stock == null)
      {
        stock = new Stock { Day = day };
        _context.Stocks.Add(stock);
        await _context.SaveChangesAsync();
      }

      return stock;
    }

    public async Task UpdateStockAsync(Stock stock)
    {
      var yaks = await _context.Yaks.ToListAsync();

      foreach (var yak in yaks)
      {
        var daysSinceLastShaved = stock.Day - yak.AgeLastShaved * 100;
        if (daysSinceLastShaved >= 8 + yak.Age * 100 * 0.01)
        {
          stock.Skins++;
          yak.AgeLastShaved = stock.Day / 100.0;
        }

        if (yak.Age * 100 < stock.Day)
        {
          stock.Milk += (50 - yak.Age * 100 * 0.03) * (stock.Day - yak.Age * 100);
        }
        else
        {
          stock.Milk += (50 - yak.Age * 100 * 0.03) * (stock.Day - yak.AgeLastShaved * 100);
        }
      }

      _context.Stocks.Update(stock);
      await _context.SaveChangesAsync();
    }
  }
}
