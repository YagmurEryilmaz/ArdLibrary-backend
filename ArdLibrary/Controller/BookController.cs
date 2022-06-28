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

        private async Task<Book> AddBook(BookDto bookDto)
        {
            /*var isFound = await context.Books.AnyAsync(s => s.Title== bookDto.Title);

            if (isFound == true)
            {
                return null;
            }*/

            var book = new Book()
            {
                Title = bookDto.Title,
                AuthorName = bookDto.AuthorName,
                IsBorrowed = bookDto.IsBorrowed,
                ImageUrl = bookDto.ImageUrl,
                Subject = bookDto.Subject,
                PublishYear =bookDto.PublishYear
            };

            try
            {

                context.Borrows.Add(new Borrow { UserId = 2, BookId = 51, ExpDate = DateTime.Now });

                //context.Books.Add(book);
                //context.Books.Update(book);

                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
   

            return book;
        }

        [HttpPost("addBook")]
        public IActionResult AddNewBook([FromBody] BookDto bookDto)
        {

            if (bookDto == null)
            {
                return BadRequest("Error");
            }

            //var book = this.AddBook(bookDto);
            var book = new Book()
            {
                Title = bookDto.Title,
                AuthorName = bookDto.AuthorName,
                IsBorrowed = bookDto.IsBorrowed,
                ImageUrl = bookDto.ImageUrl,
                Subject = bookDto.Subject,
                PublishYear = bookDto.PublishYear
            };
            var borrow = new Borrow()
            {
               UserId=2,
               BookId=51,
               ExpDate=DateTime.Now
            };

            bookDto.Id = book.Id;
            //BorrowDto borrowDto = new BorrowDto();
            //borrow.Id = borrowDto.Id;


            try
            { 
            //context.Books.Add(book);
             context.Borrows.Add(borrow);
                context.SaveChangesAsync();
            }
            catch(Exception ex)
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

