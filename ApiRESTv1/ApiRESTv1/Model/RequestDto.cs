using System.ComponentModel.DataAnnotations;

namespace ApiRESTv1.Model
{
	public class RequestDto
	{
		[Required]
		public string Title { get; set; } = "";
		[Required]
		public string Description { get; set; }="";
		[Required]
		public string IdType { get; set; } = "";
		[Required]
		
		public string IdUser { get; set; } = "";
		[Required]
		

		public decimal Longitude { get; set; }
		[Required]
		public decimal Latitude { get; set; }
	}


}
