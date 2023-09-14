using System.Threading.Tasks;
using YakShop.Models;

namespace YakShop.Services
{
  public interface IOrderService
  {
    Task<Order> CreateOrderAsync(int day, Order order);
    Task<Order> GetOrderByIdAsync(int id);
    Task<bool> UpdateOrderAsync(Order order);
    Task<bool> DeleteOrderAsync(int id);
  }

}
