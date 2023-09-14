using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using YakShop.Data;
using YakShop.Models;

namespace YakShop.Services
{
  public class OrderService : IOrderService
  {
    private readonly YakShopContext _context;

    public OrderService(YakShopContext context)
    {
      _context = context;
    }
    public async Task<Order> GetOrderByIdAsync(int id)
    {
      return await _context.Orders.FindAsync(id);
    }

    public async Task<bool> UpdateOrderAsync(Order order)
    {
      _context.Orders.Update(order);
      await _context.SaveChangesAsync();
      return true;
    }

    public async Task<Order> CreateOrderAsync(int day, Order order)
    {
      var stock = await _context.Stocks
          .OrderByDescending(s => s.Day)
          .FirstOrDefaultAsync();

      if (stock == null || stock.Milk < order.Milk || stock.Skins < order.Skins)
      {
        return null;
      }

      stock.Milk -= order.Milk;
      stock.Skins -= order.Skins;

      _context.Orders.Add(order);
      await _context.SaveChangesAsync();

      return order;
    }

    public async Task<Order> GetOrderAsync(int id)
    {
      return await _context.Orders.FindAsync(id);
    }

    public async Task<bool> DeleteOrderAsync(int id)
    {
      var order = await _context.Orders.FindAsync(id);
      if (order == null)
      {
        return false;
      }

      _context.Orders.Remove(order);
      await _context.SaveChangesAsync();

      return true;
    }
  }
}
