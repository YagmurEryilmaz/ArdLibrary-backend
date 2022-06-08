using System.ComponentModel.DataAnnotations;

namespace ArdLibrary.Entities
{
    public class Book
    {
        [Key] // otomatik olarak identity yapmak için
        public int Id { get; set; }
        public string Title { get; set; }
        public string AuthorName { get; set; }
        public int PublishYear { get; set; }
        public string Genre { get; set; }
        public string Language { get; set; }
        public bool IsBorrowed { get; set; }
        public int something { get; set; }

    }
}
