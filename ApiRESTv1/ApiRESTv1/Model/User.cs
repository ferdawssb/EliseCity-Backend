namespace ApiRESTv1.Model
{
	public class User
	{
		public int Id { get; set; }
		public string Firstname { get; set; }
		public string Lastname { get; set; }
		public string Mobile { get; set; }
		public string login { get; set; }
		public string Password { get; set; }
		public DateTime CreateAt { get; set; }

		public DateTime UpdateAt { get; set; }

	}
}
