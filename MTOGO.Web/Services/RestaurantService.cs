namespace MTOGO.Web.Services
{
	public class RestaurantService(HttpClient client)
	{
		
	}
}

public record RestaurantPage(int RestaurantCount, IEnumerable<Restaurant> Data);

public record Restaurant
{
	public Guid RestaurantId { get; init; }
	public required string Name { get; init; }
	public string Address { get; init; }
}
