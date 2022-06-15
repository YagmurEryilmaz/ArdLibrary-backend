namespace ArdLibrary.Dto
{
    public class BookDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string AuthorName { get; set; }
        public bool IsBorrowed { get; set; }
        public string ImageUrl { get; set; }
        public string Subject { get; set; }
    }
}
