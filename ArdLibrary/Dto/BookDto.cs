namespace ArdLibrary.Dto
{
    public class BookDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string AuthorName { get; set; }
        public bool IsBorrowed { get; set; }
        public int PublishYear { get; set; }
        public string ImageUrl { get; set; }
        public string Subject { get; set; }
        public string Genre { get; set; }
        public string Language { get; set; }
    }
}
