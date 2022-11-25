using Microsoft.Extensions.DependencyInjection;
using TestWebApi.Controllers;
using TestWebApi.Data;
using Microsoft.EntityFrameworkCore;
using TestWebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace IntegrationTests
{
    [TestClass]
    public class BooksTests
    {
        IServiceCollection services = new ServiceCollection();
        private readonly IUnitOfWork _unitOfWork;

        public BooksTests()
        {
            services.AddDbContext<ApplicationContext>(options => options.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=BooksDB;Trusted_Connection=true"));

            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddTransient<IBookRepository, BookRepository>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            ServiceProvider serviceProvider = services.BuildServiceProvider();

            _unitOfWork = serviceProvider.GetRequiredService<IUnitOfWork>();

        }

        [TestMethod]
        public void BooksController_AddBook_ShouldAddTheBookWithoutErrors()
        {
            //Arrange
            var booksController = new BooksController(_unitOfWork);
            var addBooksRequest = new AddBooksRequest
            {
                Name="Test" + DateTime.Now.ToString(),
                AuthorName="Test"
            };

            //Act
            var result = booksController.AddBook(addBooksRequest);
            var okResult = result as OkObjectResult;

            //Assert
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

        }

        [TestMethod]
        public void BooksController_AddBook_DuplicateBook_ShouldGetBadRequest()
        {
            //Arrange
            var booksController = new BooksController(_unitOfWork);
            var addBooksRequest = new AddBooksRequest
            {
                Name = "Test" + DateTime.Now.ToString(),
                AuthorName = "Test"
            };

            //Act
            var result = booksController.AddBook(addBooksRequest);
            result = booksController.AddBook(addBooksRequest);
            var badRequest = result as BadRequestObjectResult;

            //Assert
            Assert.IsNotNull(badRequest);
            Assert.AreEqual(400, badRequest.StatusCode);

        }


        [TestMethod]
        public void BooksController_GetAllBooks_ShouldGetAllBooks()
        {
            //Arrange
            var booksController = new BooksController(_unitOfWork);
            var addBooksRequest = new AddBooksRequest
            {
                Name = "Test" + DateTime.Now.ToString(),
                AuthorName = "Test"
            };
             booksController.AddBook(addBooksRequest);
            //Act
            var result=booksController.GetBooks();

            var request = result as OkObjectResult;

            //Assert
            Assert.IsNotNull(request);
            Assert.AreEqual(200, request.StatusCode);

        }

        [TestMethod]
        public void BooksController_GetBook_ShouldGetTheBook()
        {
            //Arrange
            var booksController = new BooksController(_unitOfWork);
            var addBooksRequest = new AddBooksRequest
            {
                Name = "Test" + DateTime.Now.ToString(),
                AuthorName = "Test"
            };
            var result1=booksController.AddBook(addBooksRequest);
            var resultObj = result1 as OkObjectResult;
            var book = resultObj.Value as Book;

            //Act
            var result = booksController.GetBook(book.Id);
            var response = result as OkObjectResult;

            //Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(200, response.StatusCode);

        }

    }
}