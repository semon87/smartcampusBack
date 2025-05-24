namespace Dev.Models
{
	public class MenuItemCreateDto
	{
		
			public string ItemName { get; set; } = null!;
			public string? Description { get; set; }
			public decimal Price { get; set; }
			public bool Availability { get; set; }
		

	}
}
