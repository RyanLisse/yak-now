using System.Threading.Tasks;
using YakShop.Data;
using YakShop.Models;

namespace YakShop.Services
{
    public class OrderService : IOrderService
    {
        private readonly YakShopContext _context;
        private readonly IStockService _stockService;

        public OrderService(YakShopContext context, IStockService stockService)
        {
            _context = context;
            _stockService = stockService;
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
            var stock = await _stockService.GetStockByDayAsync(day);

            // If we don't have enough stock, return null to signify unfulfilled order.
            if (stock.Milk < order.Milk || stock.Skins < order.Skins)
            {
                return null;
            }

            stock.Milk -= order.Milk;
            stock.Skins -= order.Skins;

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return order;
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
