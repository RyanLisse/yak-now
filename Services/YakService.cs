using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using YakShop.Data;
using YakShop.Models;

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
      var stock = await _context.Stocks.FirstOrDefaultAsync(s => s.Day == days);
      if (stock == null)
      {
        var yaks = await _context.Yaks.ToListAsync();
        stock = CalculateStock(yaks, days);
        _context.Stocks.Add(stock);
        await _context.SaveChangesAsync();
      }

      return stock;
    }

    private void UpdateYakAges(List<Yak> yaks, int days)
    {
      foreach (var yak in yaks)
      {
        // Calculate age
        double ageInDays = days + yak.Age * 100;
        double ageInYears = ageInDays / 100;
        yak.Age = Math.Round(ageInYears, 2);  // Round to 2 decimal places

        // Update last shaved age
        double daysSinceLastShave = ageInDays - yak.AgeLastShaved * 100;
        double nextShaveDay = 8 + ageInDays * 0.01;

        if (daysSinceLastShave >= nextShaveDay)
        {
          // Yak is shaven today
          double previousShaveInterval = nextShaveDay - (8 + (yak.AgeLastShaved * 100 * 0.01));
          double daysWhenLastShaved = ageInDays - previousShaveInterval;
          double ageWhenLastShaved = daysWhenLastShaved / 100.0;
          yak.AgeLastShaved = Math.Floor(ageWhenLastShaved * 10) / 10.0; // Update the AgeLastShaved based on the day the yak was last shaven
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
