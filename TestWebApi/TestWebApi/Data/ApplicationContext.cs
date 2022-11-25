using Microsoft.EntityFrameworkCore;
using TestWebApi.Models;

namespace TestWebApi.Data
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions options) : base(options)
        {}

        public DbSet<Book> Books { get; set; }

    }
}
