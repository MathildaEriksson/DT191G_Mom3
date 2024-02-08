using System.ComponentModel.DataAnnotations;
namespace DT191G_Mom3.Models;
public class Author
{
    public int AuthorId { get; set; } // Primary key
    [Required]
    public string? Name { get; set; }
    public List<Book>? Books { get; set; } // Navigation property
}
