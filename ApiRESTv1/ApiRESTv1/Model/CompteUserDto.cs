using System.ComponentModel.DataAnnotations;

namespace ApiRESTv1.Model
{
	public class CompteUserDto
	{

		[Required]
		public string Firstname { get; set; } = "";
		[Required]
		public string Lastname { get; set; } = "";
		[Required]
		public string Mobile { get; set; }= "";
		[Required]
		public string login { get; set; } = "";

		[Required]
		public string Password { get; set; } = "";


	}
}
