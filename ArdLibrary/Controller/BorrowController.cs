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
  

        // GET: api/BorrowedBooks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Borrow>>> GetBorrowedBooks()
        {
            return await context.Borrows.Include(x=>x.Book).Include(x=>x.User).ToListAsync();
        }

    }


}





