using System;
using ArdLibrary.Entities;
using Microsoft.EntityFrameworkCore;

namespace ArdLibrary.Data
{
	public class DataContext:DbContext
	{
		public DataContext(DbContextOptions<DataContext> options) : base(options) { }
		public DbSet<Book> Books { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<Borrow> Borrows { get; set; }

	}
}

