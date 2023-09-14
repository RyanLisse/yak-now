using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YakShop.Data;
using YakShop.Models;
using YakShop.Services;

namespace YakShop.Controllers
{
  [Route("yak-shop")]
  [ApiController]
  public class YakShopController : ControllerBase
  {
    private readonly YakShopContext _context;
    private readonly IYakService _yakService;
    private readonly IOrderService _orderService;
    private readonly IStockService _stockService;

    public YakShopController(YakShopContext context, IYakService yakService, IOrderService orderService, IStockService stockService)
    {
      _context = context;
      _yakService = yakService;
      _orderService = orderService;
      _stockService = stockService;
    }

    [HttpPost("load")]
    public async Task<IActionResult> LoadHerd([FromBody] Yak[] yaks)
    {
      await _yakService.LoadHerdAsync(yaks.ToList());
      return StatusCode(205);
    }

    [HttpGet("herd/{T}")]
    public async Task<IActionResult> GetHerd(int T)
    {
      var herd = await _yakService.GetHerdAsync(T);
      return Ok(herd);
    }

    [HttpGet("stock/{T}")]
    public async Task<IActionResult> GetStock(int T)
    {
      var stock = await _stockService.GetStockByDayAsync(T);
      return Ok(stock);
    }

    [HttpPost("order/{T}")]
    public async Task<IActionResult> CreateOrderAsync(int T, [FromBody] Order order)
    {
      var result = await _orderService.CreateOrderAsync(T, order);

      if (result == null)
      {
        return NotFound();
      }

      if (result.Milk < order.Milk || result.Skins < order.Skins)
      {
        return StatusCode(206, result);
      }

      return StatusCode(201, result);
    }
  }
}
