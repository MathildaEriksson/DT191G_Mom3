namespace DT191G_Mom3.Models;

public class Loan
{
    public int LoanId { get; set; }
    public DateTime BorrowedDate { get; set; }
    
    // Foreign key and navigation property for User
    public int UserId { get; set; }
    public User? User { get; set; }
    
    // Foreign key and navigation property for Book
    public int BookId { get; set; }
    public Book? Book { get; set; }
}
