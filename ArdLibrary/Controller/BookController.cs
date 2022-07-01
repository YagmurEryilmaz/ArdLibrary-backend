using ArdLibrary.Data;
using ArdLibrary.Dto;
using ArdLibrary.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ArdLibrary.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController:BaseController
    {
        private readonly DataContext context;
        public BookController(DataContext context)
        {
            this.context = context;
        }

  
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
            return await context.Books.OrderBy(x=>x.Title).ToListAsync();
        }

        [HttpGet("GetBookById/{id}")]
        public async Task<ActionResult<Book>> GetBookById(int id)
        {
            var book = await context.Books.FindAsync(id);

            return book;
        }


        [HttpGet("Filter")]
        public IQueryable<Book> GetFilteredBooks([FromQuery] FilterDto filteredBook)
        {

            var result = context.Books.AsQueryable();
            if (filteredBook != null)
            {
                if (filteredBook.AuthorName != null)
                    result = result.Where(x => x.AuthorName.Contains(filteredBook.AuthorName));
                if (filteredBook.Genre != null)
                    result = result.Where(x => x.Genre.Contains(filteredBook.Genre));
                if (filteredBook.Language != null)
                    result = result.Where(x => x.Language.Contains(filteredBook.Language));
                if (filteredBook.PublishYear != null)
                    result = result.Where(x => x.PublishYear == filteredBook.PublishYear);

            }
            return result;
        }


        [HttpPost("addBook")]
        public IActionResult AddNewBook([FromBody] BookDto bookDto)
        {

            var isFound = context.Books.Any(s => s.Title == bookDto.Title && s.AuthorName == bookDto.AuthorName);

            if (isFound == true)
            {
                return BadRequest("The book already exists in the system");
            }


            var book = new Book()
            {
                Title = bookDto.Title,
                AuthorName = bookDto.AuthorName,
                IsBorrowed = false,
                ImageUrl = bookDto.ImageUrl,
                Subject = bookDto.Subject,
                PublishYear = bookDto.PublishYear,
                Language = bookDto.Language,
                Genre = bookDto.Genre

            };

            bookDto.Id = book.Id;


            try
            {
                context.Books.Add(book);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return Ok(bookDto);

        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {

            var book = await context.Books.FirstOrDefaultAsync(b => b.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            try
            {
                 context.Books.Remove(book);

                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
        
            return NoContent();

        }


    }

}

