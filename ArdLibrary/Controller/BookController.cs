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
    public class BookController
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

    }


    //[HttpGet("getBorrowedBooks/{id}")]
    //public async Task<ActionResult<List<Borrow>>> getBorrowedBooks(int id)
    //{
    //    var borrowedBooks = await context.Books
    //    //    .Where(e => e.Id == id)
    //    //    .Include(x => x.User)
    //    //    .Select(x => x.Title)
    //    //    .OrderBy(x => x.Order).ToListAsync();
    //    //return titles;
    //}


}

