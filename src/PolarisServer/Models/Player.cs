namespace Polaris.Server.Models
{
	public class Player
	{
		public int ID { get; set; }
		public string Name { get; set; }

		Character Character { get; set; }
	}
}
