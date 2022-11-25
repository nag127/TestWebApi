using System.Linq.Expressions;
using TestWebApi.Models;

namespace TestWebApi.Data
{
    public interface IGenericRepository<T> where T : class
    {
        T GetById(Guid id);
        IEnumerable<T> GetAll();
        void Add(T entity);

        IEnumerable<T> Find(Expression<Func<T, bool>> expression);
    }
}
