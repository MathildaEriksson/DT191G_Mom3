using System.ComponentModel.DataAnnotations;
namespace DT191G_Mom3.Models;

public class User
{
    public int UserId { get; set; }
    [Required]
    public string? Name { get; set; }
}
