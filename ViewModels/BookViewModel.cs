using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace DT191G_Mom3.Models;

public class BookViewModel
{
    public int BookId { get; set; } 
    [Required]
    public string? Title { get; set; }
    public int? Year { get; set; }

    // Author selections
    public List<int> SelectedAuthorIds { get; set; } = new List<int>(); // IDs of selected authors
    public IEnumerable<SelectListItem> AuthorsList { get; set; } = new List<SelectListItem>(); // All authors for the dropdown
}
