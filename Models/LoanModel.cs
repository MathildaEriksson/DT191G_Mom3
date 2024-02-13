namespace DT191G_Mom3.Models;
using System.ComponentModel.DataAnnotations;

public class Loan
{
    public int LoanId { get; set; }
    [Display(Name = "Date borrowed")]
    public DateTime BorrowedDate { get; set; }
    
    [Display(Name = "User")]
    public int UserId { get; set; }
    public User? User { get; set; }
    
    [Display(Name = "Book")]
    public int BookId { get; set; }
    public Book? Book { get; set; }
}
