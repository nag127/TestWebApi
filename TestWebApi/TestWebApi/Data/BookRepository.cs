using TestWebApi.Models;

namespace TestWebApi.Data
{
    public class BookRepository : GenericRepository<Book>, IBookRepository
    {
        public BookRepository(ApplicationContext context) : base(context)
        {

        }
    }
}
