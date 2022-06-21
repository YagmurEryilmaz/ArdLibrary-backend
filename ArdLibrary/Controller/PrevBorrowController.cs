using System;
using ArdLibrary.Dto;
using ArdLibrary.Entities;
using System;
using ArdLibrary.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ArdLibrary.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrevBorrowController:BaseController
    {
        private readonly DataContext context;
        public PrevBorrowController(DataContext context)
        {
            this.context = context;
        }

        private PrevBorrow AddToPrevBorrowed(BorrowAddDto borrowDto)
        {
            var isBorrowed = context.PrevBorrows.Any(s => s.BookId == borrowDto.BookId);

            if (isBorrowed == true)
            {
                return null;
            }

            var prevBorrowedBook = new PrevBorrow()
            {
                UserId = GetUserId(),
                BookId = borrowDto.BookId,
                ExpDate = borrowDto.ExpDate

            };

            var book = context.Books.FirstOrDefault(b => b.Id == borrowDto.BookId);
            book.IsBorrowed = false;

            context.PrevBorrows.Add(prevBorrowedBook);
            context.SaveChanges();
            return prevBorrowedBook;
        }

        [HttpPost]
        public IActionResult AddPrevBorrowedBook([FromBody] BorrowAddDto borrowAddDto)
        {

            var prevBorrowedBook = this.AddToPrevBorrowed(borrowAddDto);

            if (prevBorrowedBook == null)
            {
                return BadRequest("Book is already in list");
            }

            BorrowDto borrowDto = new BorrowDto();
            borrowDto.Id = prevBorrowedBook.Id;


            return Ok(borrowDto);

        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<PrevBorrow>>> GetPrevBorrowedBooks()
        {
            return await context.PrevBorrows.Include(x => x.Book).Include(x => x.User).ToListAsync();
        }


        [HttpGet("GetPrevBorrowedBooksById/{id}")]
        public async Task<ActionResult<List<PrevBorrow>>> GetPrevBorrowedBooksById(int id)
        {
            var prevBorrowedBooksList = await context.PrevBorrows.Where(b => b.UserId == id).Include(x => x.Book).Include(x => x.User).ToListAsync();


            return prevBorrowedBooksList;
        }

    }
}

