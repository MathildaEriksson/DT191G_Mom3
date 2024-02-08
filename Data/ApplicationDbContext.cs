using Microsoft.EntityFrameworkCore;
using DT191G_Mom3.Models;

namespace DT191G_Mom3.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Author> Authors { get; set; }
    public DbSet<Book> Books { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Relationship between Author and Book
        modelBuilder.Entity<Book>()
            .HasOne(b => b.Author) // Each book has one author.
            .WithMany(a => a.Books) // An author can have many books.
            .HasForeignKey(b => b.AuthorId); // Specify the foreign key in the 'Book' table.

    }
}