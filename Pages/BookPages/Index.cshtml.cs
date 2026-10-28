
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Vidican_Miruna_Lab2.Models;

namespace Vidican_Miruna_Lab2.Pages.BookPages
{
    public class IndexModel : PageModel
    {
        private readonly Vidican_Miruna_Lab2Context _context;

        public IndexModel(Vidican_Miruna_Lab2Context context)
        {
            _context = context;
        }

        public IList<Book> Book
        {
            get; set;
        }
        public BookData BookD
        {
            get; set;
        }
        public int BookID
        {
            get; set;
        }
        public int CategoryID
        {
            get; set;
        }

        public string TitleSort { get; set; }
        public string AuthorSort { get; set; }

        public string CurrentFilter { get; set; } = string.Empty;



        public async Task OnGetAsync(int? id, int? categoryID, string sortOrder, string searchString)
        {


            TitleSort = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            AuthorSort = sortOrder == "author" ? "author_desc" : "author";
            CurrentFilter = searchString ?? string.Empty;

            BookD = new BookData();


            IQueryable<Book> books = _context.Book
                .Include(b => b.Authors)
                .Include(b => b.Publisher)
                .Include(b => b.BookCategories)
                    .ThenInclude(bc => bc.Category)
                .AsNoTracking();


            if (!String.IsNullOrEmpty(searchString))
            {
                books = books.Where(b =>
                    b.Title.Contains(searchString) ||
                    (b.Authors != null &&
                     (b.Authors.LastName.Contains(searchString) ||
                      b.Authors.FirstName.Contains(searchString))));
            }


            switch (sortOrder)
            {
                case "title_desc":
                    books = books.OrderByDescending(b => b.Title);
                    break;

                case "author":
                    books = books.OrderBy(b => b.Authors.LastName);
                    break;

                case "author_desc":
                    books = books.OrderByDescending(b => b.Authors.LastName);
                    break;

                default:
                    books = books.OrderBy(b => b.Title);
                    break;
            }

            BookD.Books = await books.ToListAsync();

            if (id != null)
            {
                BookID = id.Value;

                Book book = BookD.Books
                    .Where(i => i.ID == id.Value).Single();

                BookD.Categories = book.BookCategories.Select(s => s.Category);
            }
        }
    }
}
