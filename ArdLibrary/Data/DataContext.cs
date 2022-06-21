using System;
using ArdLibrary.Entities;
using Microsoft.EntityFrameworkCore;

namespace ArdLibrary.Data
{
	public class DataContext:DbContext
	{

        public DataContext()
        {
        }
        public DataContext(DbContextOptions<DataContext> options): base(options) { }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (!options.IsConfigured)
            {
                options.UseSqlServer("Data Source=164.92.243.254;Initial Catalog= ArdLibrary;Integrated Security=False;Persist Security Info=True;User ID= stj;Password=165d252b89;MultipleActiveResultSets=true");
            }
        }
        public DbSet<Book> Books { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<Borrow> Borrows { get; set; }
        public DbSet<PrevBorrow> PrevBorrows { get; set; }

    }
}

