
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Vidican_Miruna_Lab2.Models;

namespace Vidican_Miruna_Lab2.Pages.BookPages
{
    public class CreateModel : BookCategoriesPageModel
    {
        private readonly Vidican_Miruna_Lab2Context _context;

        public CreateModel(Vidican_Miruna_Lab2Context context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            var authorList = _context.Authors.Select(x => new
            {
                x.ID,
                FullName = x.LastName + " " + x.FirstName
            });

            ViewData["AuthorsID"] = new SelectList(authorList, "ID", "FullName");
            ViewData["PublisherID"] = new SelectList(_context.Publisher, "ID", "PublisherName");

            var book = new Book();
            book.BookCategories = new List<BookCategory>();

            PopulateAssignedCategoryData(_context, book);

            return Page();
        }

        [BindProperty]
        public Book Book
        {
            get; set;
        }

        public async Task<IActionResult> OnPostAsync(string[] selectedCategories)
        {
            var newBook = new Book();

            if (selectedCategories != null)
            {
                newBook.BookCategories = new List<BookCategory>();

                foreach (var cat in selectedCategories)
                {
                    var catToAdd = new BookCategory
                    {
                        CategoryID = int.Parse(cat)
                    };

                    newBook.BookCategories.Add(catToAdd);
                }
            }

            Book.BookCategories = newBook.BookCategories;
            _context.Book.Add(Book);
            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}
