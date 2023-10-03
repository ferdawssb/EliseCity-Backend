using System.ComponentModel.DataAnnotations;

namespace ApiRESTv1.Model
{
	public class RequestTypeDto
	{

		[Required]
		public string Title { get; set; } = "";
	}
}
