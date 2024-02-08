using System.ComponentModel.DataAnnotations;
namespace DT191G_Mom3.Models;
public class Book
{
    public int BookId { get; set; } // Primary key
    [Required]
    public string? Title { get; set; }
    [Required]
    public int? Year { get; set; }
    public int AuthorId { get; set; } // Foreign key
    public Author? Author { get; set; } // Navigation property
}
