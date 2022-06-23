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
            var isBorrowed =  context.Borrows.Any(s => s.BookId == borrowDto.BookId);

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
            context.Borrows.Update(borrowedBook);
            context.Borrows.Add(borrowedBook);
            
            context.SaveChanges();
            return borrowedBook;
        }

        [HttpPost]
        public IActionResult AddBorrowedBook([FromBody] BorrowAddDto borrowAddDto)
        {

            var count = context.Borrows.Count(r => r.UserId == borrowAddDto.UserId && r.ExpDate > DateTime.Today);
      
            if(count >= 5)
            {
                return BadRequest("You cannot borrow more than 5 books");
            }
            var borrowedBook = this.AddToBorrowed(borrowAddDto);

            if (borrowedBook==null)
            {
                return BadRequest("Book is already Borrowed");
            }

            BorrowDto borrowDto = new BorrowDto();
            borrowDto.Id = borrowedBook.Id;

           
            return Ok(borrowDto);
           
        }
  

        [HttpGet]
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





        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBorrowedBook(int id)
        {
            
                var borrow = await context.Borrows.FirstOrDefaultAsync(b=>b.BookId== id);

               if (borrow == null)
                {
                    return NotFound();
                }

            var prevBorrowedBook = new PrevBorrow()
            {
                UserId = borrow.UserId,
                BookId = id,
                ExpDate = borrow.ExpDate

            };

            var book = context.Books.FirstOrDefault(b => b.Id == id);
            book.IsBorrowed = false;
            borrow.ExpDate = DateTime.Now.AddDays(-3);
            prevBorrowedBook.ExpDate = DateTime.Now.AddDays(-3);


            context.Books.Update(book);
            context.Borrows.Update(borrow);
            context.PrevBorrows.Add(prevBorrowedBook);
            context.Borrows.Remove(borrow);
          
                await context.SaveChangesAsync();

                return NoContent();

        }


    }


}





