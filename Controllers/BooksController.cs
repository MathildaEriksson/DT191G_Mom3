using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DT191G_Mom3.Data;
using DT191G_Mom3.Models;

namespace DT191G_Mom3.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BooksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Books
        public async Task<IActionResult> Index(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;

            IQueryable<Book> booksQuery = _context.Books
                .Include(b => b.BookAuthors)
                    .ThenInclude(ba => ba.Author);

            if (!String.IsNullOrEmpty(searchString))
            {
                booksQuery = booksQuery.Where(b => EF.Functions.Like(b.Title, $"%{searchString}%"));
            }

            var books = await booksQuery.ToListAsync();
            return View(books);
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(b => b.BookAuthors)
                    .ThenInclude(ba => ba.Author)
                .FirstOrDefaultAsync(m => m.BookId == id);

            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            var model = new BookViewModel
            {
                // Populate AuthorsList with authors from the database
                AuthorsList = _context.Authors.Select(a => new SelectListItem
                {
                    Value = a.AuthorId.ToString(),
                    Text = a.Name
                })
            };
            return View(model);
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookViewModel model)
        {
            if (ModelState.IsValid)
            {
                var book = new Book { Title = model.Title, Year = model.Year };
                _context.Add(book);
                await _context.SaveChangesAsync();

                foreach (var authorId in model.SelectedAuthorIds)
                {
                    var bookAuthor = new BookAuthor { BookId = book.BookId, AuthorId = authorId };
                    _context.BookAuthors.Add(bookAuthor);
                }
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            // Re-populate AuthorsList if returning to form
            model.AuthorsList = _context.Authors.Select(a => new SelectListItem
            {
                Value = a.AuthorId.ToString(),
                Text = a.Name
            });
            return View(model);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var book = await _context.Books
                .Include(b => b.BookAuthors)
                .FirstOrDefaultAsync(m => m.BookId == id);

            if (book == null) return NotFound();

            var selectedAuthors = book.BookAuthors?.Select(ba => ba.AuthorId).ToList() ?? new List<int>();

            var model = new BookViewModel
            {
                BookId = book.BookId,
                Title = book.Title,
                Year = book.Year,
                SelectedAuthorIds = selectedAuthors,
                AuthorsList = _context.Authors.Select(a => new SelectListItem
                {
                    Value = a.AuthorId.ToString(),
                    Text = a.Name
                }).ToList()
            };

            return View(model);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BookViewModel model)
        {
            if (id != model.BookId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var bookToUpdate = await _context.Books
                        .Include(b => b.BookAuthors)
                        .FirstOrDefaultAsync(b => b.BookId == id);

                    if (bookToUpdate == null)
                    {
                        return NotFound();
                    }

                    bookToUpdate.Title = model.Title;
                    bookToUpdate.Year = model.Year;

                    // Update authors
                    var existingAuthors = bookToUpdate.BookAuthors.Select(ba => ba.AuthorId).ToList();
                    var newAuthors = model.SelectedAuthorIds.Except(existingAuthors).ToList();
                    var removedAuthors = existingAuthors.Except(model.SelectedAuthorIds).ToList();

                    foreach (var authorId in newAuthors)
                    {
                        bookToUpdate.BookAuthors.Add(new BookAuthor { BookId = id, AuthorId = authorId });
                    }

                    foreach (var authorId in removedAuthors)
                    {
                        var authorToRemove = bookToUpdate.BookAuthors.FirstOrDefault(ba => ba.AuthorId == authorId);
                        if (authorToRemove != null) bookToUpdate.BookAuthors.Remove(authorToRemove);
                    }

                    _context.Update(bookToUpdate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(model.BookId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            // If we got this far, something failed, re-populate AuthorsList to show the form again
            model.AuthorsList = _context.Authors.Select(a => new SelectListItem
            {
                Value = a.AuthorId.ToString(),
                Text = a.Name
            }).ToList();

            return View(model);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .FirstOrDefaultAsync(m => m.BookId == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.BookId == id);
        }
    }
}
