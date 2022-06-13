using System.ComponentModel.DataAnnotations;

namespace ArdLibrary.Entities
{
    public class User
    {
        public User()
        {
            Borrows = new List<Borrow>();
        }
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public virtual IEnumerable<Borrow> Borrows { get; set; }

    }
}
