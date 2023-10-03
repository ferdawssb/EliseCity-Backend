using ApiRESTv1.Data;
using ApiRESTv1.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using System;

namespace ApiRESTv1.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class RequestTypeController : Controller
	{

		private readonly AppDbContext _context;

		public RequestTypeController(AppDbContext context)
		{
			_context = context;
			
		}


	
		[HttpPost]
		public IActionResult CreateProduct(RequestTypeDto req)
		{
			try
			{
				using (var connection = new MySqlConnection(_context.Database.GetConnectionString()))
				{
					connection.Open();
					String sql = " INSERT INTO request_type (title) VALUES (@title)  ";


					using (var command = new MySqlCommand(sql, connection))
					{
						
						command.Parameters.AddWithValue("@title", req.Title);


						command.ExecuteNonQuery();

					}

				}

			}
			catch (Exception e)
			{
				ModelState.AddModelError("Request", "Désolé, mais nous avons rencontré une exception.");
				return BadRequest(ModelState);
			}

			return Ok();

		}




		[HttpGet()]
		public IActionResult GetAdmins()
		{
			List<RequestType> Requests = new List<RequestType>();
			try
			{
				using (var connection = new MySqlConnection(_context.Database.GetConnectionString()))
				{
					connection.Open();

					string req = "SELECT * FROM request_type ";
					using (var command = new MySqlCommand(req, connection))
					{
						using (var reader = command.ExecuteReader())
						{
							while (reader.Read())
							{
								RequestType request = new RequestType();

								request.Id= reader.GetInt32(0);
								request.title = reader.GetString(1);
								

								Requests.Add(request);
							}
						}
					}
				}
			}
			catch (Exception e)
			{
				ModelState.AddModelError("Request", "Désolé, mais nous avons rencontré une exception.");
				return BadRequest(ModelState);
			}
			return Ok(Requests);
		}




	}
}

