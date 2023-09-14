using System.Threading.Tasks;
using YakShop.Models;

namespace YakShop.Services
{
  public interface IStockService
  {
    Task<Stock> GetStockByDayAsync(int day);
    Task UpdateStockAsync(Stock stock);
  }
}
