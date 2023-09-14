using System.ComponentModel.DataAnnotations;

namespace YakShop.Models
{
  public class Yak
  {
    [Key]
    public string Name { get; set; }

    public double Age { get; set; }

    public string Sex { get; set; }

    public double AgeLastShaved { get; set; }
  }
}
