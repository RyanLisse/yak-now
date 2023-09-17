using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using YakShop.Models;
using YakShop.Models.DTOs;
using YakShop.Services;

namespace YakShop.Controllers
{
    [Route("yak-shop")]
    [ApiController]
    public class YakShopController : ControllerBase
    {
        private readonly IYakService _yakService;
        private readonly IOrderService _orderService;
        private readonly IStockService _stockService;

        public YakShopController(IYakService yakService, IOrderService orderService, IStockService stockService)
        {
            _yakService = yakService;
            _orderService = orderService;
            _stockService = stockService;
        }

        [HttpPost("load")]
        public async Task<IActionResult> LoadHerd([FromBody] HerdRequest herdRequest)
        {
            if (herdRequest == null || herdRequest.Herd == null)
            {
                return BadRequest("Invalid herd data.");
            }

            await _yakService.LoadHerdAsync(herdRequest.Herd);
            return StatusCode(205);
        }

        [HttpGet("herd/{T}")]
        public async Task<IActionResult> GetHerd(int T)
        {
            var yaks = await _yakService.GetHerdAsync(T);
            var herdResponse = yaks.Select(yak => new HerdResponseDTO
            {
                Name = yak.Name,
                Age = yak.Age,
                AgeLastShaved = yak.AgeLastShaved
            }).ToList();

            return Ok(new { herd = herdResponse });
        }


        [HttpGet("stock/{T}")]
        public async Task<IActionResult> GetStock(int T)
        {
            if (T < 0)
            {
                return BadRequest("Invalid day number.");
            }

            var stock = await _stockService.GetStockByDayAsync(T);
            return Ok(stock);
        }

        [HttpPost("order/{T}")]
        public async Task<IActionResult> CreateOrderAsync(int T, [FromBody] Order order)
        {
            if (T < 0 || order == null)
            {
                return BadRequest("Invalid input.");
            }

            var result = await _orderService.CreateOrderAsync(T, order);

            if (result == null)
            {
                return NotFound("Order could not be completed.");
            }

            if (result.Milk < order.Milk || result.Skins < order.Skins)
            {
                return StatusCode(206, new { skins = result.Skins, milk = result.Milk });
            }

            return StatusCode(201, new { skins = result.Skins, milk = result.Milk });
        }
    }
}
