using System;
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
	public class BorrowController:BaseController
	{
        private readonly DataContext context;
        public BorrowController(DataContext context)
        {
            this.context = context;
        }

        private Borrow AddToBorrowed(BorrowAddDto borrowDto)
        {
            var isBorrowed =  context.Borrows.Any(s=> s.ExpDate > DateTime.Now.AddDays(-2) && s.BookId == borrowDto.BookId && s.UserId == borrowDto.UserId);

            if (isBorrowed == true)
            {
                return null;
            }

            var borrowedBook = new Borrow()
            {
                UserId = borrowDto.UserId,
                BookId = borrowDto.BookId,
                ExpDate = borrowDto.ExpDate

            };

            var book = context.Books.FirstOrDefault(b => b.Id == borrowDto.BookId);
            book.IsBorrowed = true;

            context.Books.Update(book);
         
            try {
                context.Borrows.Add(borrowedBook);

                context.SaveChanges();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return borrowedBook;
        }

        [HttpPost]
        public IActionResult AddBorrowedBook([FromBody] BorrowAddDto borrowAddDto)
        {
            var count = context.Borrows.Count(r => r.UserId == borrowAddDto.UserId && r.ExpDate > DateTime.Now.AddDays(-2));

            if (count >= 5)
            {
                return BadRequest("You cannot borrow more than 5 books");
            }
            else
            {
                var borrowedBook = this.AddToBorrowed(borrowAddDto);

                if (borrowedBook == null)
                {
                    return BadRequest("Book is already Borrowed");
                }

                BorrowDto borrowDto = new BorrowDto();
                borrowDto.Id = borrowedBook.Id;


                return Ok(borrowDto);
            }
           
        }
  

        [HttpGet("getBorrowedBooks")]
        public async Task<ActionResult<IEnumerable<Borrow>>> GetBorrowedBooks()
        {
            return await context.Borrows.Include(x=>x.Book).Include(x=>x.User).ToListAsync();
        }


        [HttpGet("GetBorrowedBooksById/{id}")]
        public async Task<ActionResult<List<Borrow>>> GetBorrowedBooksById(int id)
        {
            var borrowedBooksList = await context.Borrows.Where(b => b.UserId == id).Include(x => x.Book).Include(x => x.User).ToListAsync();
           
            return borrowedBooksList;
        }

        [HttpGet("GetPrevBorrowedBooksById/{id}")]
        public async Task<ActionResult<List<Borrow>>> GetPrevBooksById(int id)
        {
            var prevBorrowedBooksList = await context.Borrows.Where(b => b.UserId == id && b.ExpDate< DateTime.Now.AddDays(-2)).Include(x => x.Book).Include(x => x.User).ToListAsync();
            foreach (var book in prevBorrowedBooksList)
            {
                book.Book.IsBorrowed = false;
                context.Books.Update(book.Book);
                context.Borrows.Update(book);
            }
            await context.SaveChangesAsync();

            return prevBorrowedBooksList;
        }


        [HttpGet("GetBorrowDate/{id}")]
        public async Task<ActionResult<List<DateTime>>> GetBorrrowDate(int id)
        {
            var prevBorrowedBooksList = await context.Borrows.Where(b => b.UserId == id && b.ExpDate < DateTime.Now.AddDays(-2)).Select(e => e.ExpDate.AddDays(+3).ToLocalTime()).ToListAsync();
            return prevBorrowedBooksList;
        }

        [HttpGet("GetRealExpDate/{id}")]
        public async Task<ActionResult<List<DateTime>>> GetRealExpDate(int id)
        {
            var currBorrowedBooksList = await context.Borrows.Where(b => b.UserId == id && b.ExpDate > DateTime.Now.AddDays(-2)).Select(e => e.ExpDate.AddDays(+7).ToLocalTime()).ToListAsync();
            return currBorrowedBooksList;
        }


        [HttpGet("GetCurrentlyBorrowedBooksById/{id}")]
        public async Task<ActionResult<List<Borrow>>> GetCurrentBooksById(int id)
        {
            var currBorrowedBooksList = await context.Borrows.Where(b => b.UserId == id && b.ExpDate > DateTime.Now.AddDays(-2)).Include(x => x.Book).Include(x => x.User).ToListAsync();
            foreach (var book in currBorrowedBooksList)
            {
                book.Book.IsBorrowed = true;
                context.Books.Update(book.Book);
                context.Borrows.Update(book);
            }
            await context.SaveChangesAsync();

            return currBorrowedBooksList;
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBorrowedBook(int id)
        {
            
                var borrow = await context.Borrows.FirstOrDefaultAsync(b=>b.BookId== id);

               if (borrow == null)
                {
                    return NotFound();
                }


            var book = context.Books.FirstOrDefault(b => b.Id == id);
            book.IsBorrowed = false;
            borrow.ExpDate = DateTime.Now.AddDays(-3);

            context.Books.Update(book);
            context.Borrows.Update(borrow);
                await context.SaveChangesAsync();

                return NoContent();

        }


    }


}





