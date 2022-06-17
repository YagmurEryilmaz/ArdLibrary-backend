using System;
namespace ArdLibrary.Dto
{
	public class BorrowAddDto
	{
		public int UserId { get; set; }
		public int BookId { get; set; }
		public DateTime ExpDate { get; set; }
	}
}

