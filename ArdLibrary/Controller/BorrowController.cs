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
                UserId = GetUserId(),
                BookId = borrowDto.BookId,
                ExpDate = borrowDto.ExpDate

            };

            var book = context.Books.FirstOrDefault(b => b.Id == borrowDto.BookId);
            book.IsBorrowed = true;
 
            context.Books.Update(book);
            context.Borrows.Add(borrowedBook);
            context.SaveChanges();
            return borrowedBook;
        }

        [HttpPost]
        public IActionResult AddBorrowedBook([FromBody] BorrowAddDto borrowAddDto)
        {
      
            var borrowedBook = this.AddToBorrowed(borrowAddDto);

            if(borrowedBook==null)
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


            var book = context.Books.FirstOrDefault(b => b.Id == id);
    
            book.IsBorrowed = false;
            borrow.ExpDate = DateTime.Now.AddDays(-7);

            context.Books.Update(book);
            context.Borrows.Update(borrow);
                await context.SaveChangesAsync();

                return NoContent();

        }


    }


}





