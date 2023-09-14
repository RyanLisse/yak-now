using System.ComponentModel.DataAnnotations;

namespace YakShop.Models
{
  public class Order
  {
    [Key]
    public int Id { get; set; }

    public string Customer { get; set; }

    public double Milk { get; set; }

    public int Skins { get; set; }

    public int Day { get; set; }
  }
  public class OrderResponse
  {
    public bool IsOrderFulfilled { get; set; }
  }
}
