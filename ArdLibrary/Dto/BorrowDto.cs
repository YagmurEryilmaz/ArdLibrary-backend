namespace ArdLibrary.Dto
{
    public class BorrowDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
        public DateTime ExpDate { get; set; }
    }
}
