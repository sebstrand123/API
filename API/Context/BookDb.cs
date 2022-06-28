using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Context
{
    public class BookDb : DbContext
    {
        public BookDb(DbContextOptions<BookDb> options)
            : base(options) { }

        public DbSet<Book> Books => Set<Book>();
    }
}
