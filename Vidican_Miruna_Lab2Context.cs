using Microsoft.EntityFrameworkCore;
using Vidican_Miruna_Lab2.Models;

public class Vidican_Miruna_Lab2Context(DbContextOptions<Vidican_Miruna_Lab2Context> options) : DbContext(options)
{
    public DbSet<Book> Book { get; set; } = default!;

    public DbSet<Publisher> Publisher { get; set; } = default!;

    public DbSet<Authors> Authors { get; set; } = default!;
}
