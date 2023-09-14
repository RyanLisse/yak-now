using System.ComponentModel.DataAnnotations;

namespace YakShop.Models
{
  public class Stock
  {
    [Key]
    public int Id { get; set; }

    public double Milk { get; set; }

    public int Skins { get; set; }

    public int Day { get; set; }
  }
}
