using ApiRESTv1.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiRESTv1.Data
{
	public class AppDbContext : DbContext
	{

		public AppDbContext (DbContextOptions <AppDbContext>options ) : base (options)	
		{





		}
	
		public DbSet<User> users { get; set; }


		public DbSet<RequestType> request_type { get; set; }
		public DbSet<Request> request { get; set; }
























	}
}
