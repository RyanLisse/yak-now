using System.Collections.Generic;
using System.Threading.Tasks;
using YakShop.Models;

namespace YakShop.Services
{
  public interface IYakService
  {
    Task LoadHerdAsync(List<Yak> yaks);
    Task<List<Yak>> GetHerdAsync(int day);
    Task UpdateHerdAsync(int day);
    Task<Stock> GetStockAsync(int day);
    Task<OrderResponse> CreateOrderAsyncAsync(int day, Order order);
  }
}