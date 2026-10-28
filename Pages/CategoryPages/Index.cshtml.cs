
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Vidican_Miruna_Lab2.Models;
using Vidican_Miruna_Lab2.ViewModels;

namespace Vidican_Miruna_Lab2.Pages.CategoryPages;

public class IndexModel : PageModel
{
    private readonly Vidican_Miruna_Lab2Context _context;

    public IndexModel(Vidican_Miruna_Lab2Context context)
    {
        _context = context;
    }

    public IList<Category> Category { get; set; } = default!;
    public CategoryIndexData CategoryData { get; set; } = default!;
    public int CategoryID { get; set; }

    public async Task OnGetAsync(int? id)
    {
        CategoryData = new CategoryIndexData();

        CategoryData.Categories = await _context.Category
            .Include(c => c.BookCategories)
            .ThenInclude(bc => bc.Book)
            .ThenInclude(b => b.Authors)
            .OrderBy(c => c.CategoryName)
            .ToListAsync();

        Category = CategoryData.Categories.ToList();

        if (id != null)
        {
            CategoryID = id.Value;

            Category? category = CategoryData.Categories
                .SingleOrDefault(c => c.ID == id.Value);

            if (category != null)
            {
                CategoryData.Books = category.BookCategories!
                    .Select(bc => bc.Book)
                    .ToList();
            }
        }
    }
}
