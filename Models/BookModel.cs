using System.ComponentModel.DataAnnotations;
namespace DT191G_Mom3.Models;
public class Book
{
    public int BookId { get; set; } // Primary key
    [Required]
    public string? Title { get; set; }
    public int? Year { get; set; }
    public List<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();
}
