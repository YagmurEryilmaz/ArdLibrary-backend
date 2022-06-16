using ArdLibrary.Data;
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

        // GET: api/Books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
            return await context.Books.ToListAsync();
        }

        [HttpGet("GetBookById/{id}")]
        public async Task<ActionResult<Book>> GetBookById(int id)
        {
            var book = await context.Books.FindAsync(id);


            return book;
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
}
