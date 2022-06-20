using System;
namespace ArdLibrary.Dto
{
	public class FilteredBook
	{
		public string? AuthorName { get; set; }
		public int? PublishYear { get; set; }
		public string? Genre { get; set; }
		public string? Language { get; set; }
	}
}

