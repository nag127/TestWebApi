namespace TestWebApi.Data
{
    public interface IUnitOfWork : IDisposable
    {
        IBookRepository Books { get; }
       
        int Complete();
    }
}
