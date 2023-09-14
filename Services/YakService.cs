using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YakShop.Data;
using YakShop.Models;
using Microsoft.EntityFrameworkCore;
namespace YakShop.Services
{
  public class YakService : IYakService
  {
    private readonly YakShopContext _context;

    public YakService(YakShopContext context)
    {
      _context = context;
    }

    public async Task LoadHerdAsync(List<Yak> yaks)
    {
      _context.Yaks.RemoveRange(_context.Yaks);
      await _context.Yaks.AddRangeAsync(yaks);
      await _context.SaveChangesAsync();
    }

    public async Task<List<Yak>> GetHerdAsync(int days)
    {
      var yaks = await _context.Yaks.ToListAsync();
      UpdateYakAges(yaks, days);
      return yaks;
    }

    public async Task UpdateHerdAsync(int days)
    {
      var yaks = await _context.Yaks.ToListAsync();
      UpdateYakAges(yaks, days);
      await _context.SaveChangesAsync();
    }

    public async Task<Stock> GetStockAsync(int days)
    {
      var yaks = await _context.Yaks.ToListAsync();
      return CalculateStock(yaks, days);
    }

    private void UpdateYakAges(List<Yak> yaks, int days)
    {
      foreach (var yak in yaks)
      {
        yak.Age += days / 100.0;
        if (yak.AgeLastShaved + 0.01 * days < yak.Age)
        {
          yak.AgeLastShaved = yak.Age;
        }
      }
    }
    public async Task<OrderResponse> CreateOrderAsyncAsync(int day, Order order)
    {
      var stock = await GetStockAsync(day);
      if (stock.Milk < order.Milk || stock.Skins < order.Skins)
      {
        return new OrderResponse { IsOrderFulfilled = false };
      }
      stock.Milk -= order.Milk;
      stock.Skins -= order.Skins;
      await _context.SaveChangesAsync();
      return new OrderResponse { IsOrderFulfilled = true };
    }
    private Stock CalculateStock(List<Yak> yaks, int days)
    {
      double totalMilk = 0;
      int totalSkins = 0;

      foreach (var yak in yaks)
      {
        totalMilk += (50 - yak.Age * 0.03) * days;
        totalSkins += (int)((days - yak.Age * 0.01) / (8 + yak.Age * 0.01));
      }

      return new Stock
      {
        Milk = totalMilk,
        Skins = totalSkins,
        Day = days
      };
    }
  }
}
