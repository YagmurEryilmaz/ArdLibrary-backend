using System.ComponentModel.DataAnnotations.Schema;

namespace ArdLibrary.Entities
{
    public class Borrow
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
        public DateTime ExpDate { get; set; }
        public int AuthorName { get; set; } // bir kere foreignkey verirsen digerlerine erişebilirsin.

        public int Title { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; },,,,,,,

        [ForeignKey("BookId")]
        public virtual Book Book { get; set; }

    }
}
