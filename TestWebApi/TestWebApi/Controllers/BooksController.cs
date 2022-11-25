using Microsoft.AspNetCore.Mvc;
using TestWebApi.Data;
using TestWebApi.Models;

namespace TestWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public BooksController(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("{id:guid}")]
        public IActionResult GetBook([FromRoute] Guid id)
        {
            var book= _unitOfWork.Books.GetById(id);
            if (book != null)
            {
                return Ok(book);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        public IActionResult GetBooks()
        {
            return Ok(_unitOfWork.Books.GetAll());
        }

        [HttpPost]
        public IActionResult AddBook(AddBooksRequest addBooksRequest)
        {
            var duplicateData= _unitOfWork.Books.Find(x => x.Name == addBooksRequest.Name && x.AuthorName == addBooksRequest.AuthorName).ToList();
            if (duplicateData.Any())
            {
                return BadRequest("Duplicate Book");
            }

            var book = new Book()
            {
                Id = Guid.NewGuid(),
                Name = addBooksRequest.Name,
                AuthorName = addBooksRequest.AuthorName
            };
            _unitOfWork.Books.Add(book);
          
            
            return Ok(book);
        }
    }
}
