using ApiRESTv1.Data;
using ApiRESTv1.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using System.Security.Cryptography.X509Certificates;

namespace ApiRESTv1.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class RequestController : Controller
	{
		private readonly AppDbContext _context;

		public RequestController(AppDbContext context)
		{
			_context = context; 

		}


		[HttpPost]
		public IActionResult CreateProduct( RequestDto req)
		{
			try
			{
				using (var connection = new MySqlConnection(_context.Database.GetConnectionString()))
				{
					connection.Open();
					// Récupérer l'ID de l'utilisateur en utilisant le nom d'utilisateur
					string userIdSql = "SELECT Id FROM users WHERE Firstname = @Firstname";
					int userId;

					using (var getUserIdCommand = new MySqlCommand(userIdSql, connection))
					{
						getUserIdCommand.Parameters.AddWithValue("@Firstname", req.IdUser); 
						var userIdResult = getUserIdCommand.ExecuteScalar();
						if (userIdResult != null)
						{
							userId = Convert.ToInt32(userIdResult);
						}
						else
						{
							// L'utilisateur n'existe pas, renvoyez une erreur
							ModelState.AddModelError("Request", "L'utilisateur spécifié n'existe pas.");
							return BadRequest(ModelState);
						}
					}

					// Récupérer l'ID du type de demande en utilisant le titre
					string typeIdSql = "SELECT id FROM request_type WHERE title = @title";
					int typeId;

					using (var getTypeIdCommand = new MySqlCommand(typeIdSql, connection))
					{
						getTypeIdCommand.Parameters.AddWithValue("@title", req.IdType); 
						var typeIdResult = getTypeIdCommand.ExecuteScalar();
						if (typeIdResult != null)
						{
							typeId = Convert.ToInt32(typeIdResult);
						}
						else
						{
							// Le type de demande n'existe pas, renvoyez une erreur
							ModelState.AddModelError("Request", "Le type de demande spécifié n'existe pas.");
							return BadRequest(ModelState);
						}
					}

					

					// Maintenant que vous avez les IDs correspondants, vous pouvez insérer la demande
					string insertSql = "INSERT INTO request (title, description, id_user, id_type, Longitude, Latitude) " +
									   "VALUES (@title, @description, @id_user, @id_type, @log, @lat)";

					using (var command = new MySqlCommand(insertSql, connection))
					{
						command.Parameters.AddWithValue("@title", req.Title); 
						command.Parameters.AddWithValue("@description", req.Description);
						command.Parameters.AddWithValue("@id_user", userId);
						command.Parameters.AddWithValue("@id_type", typeId);
						command.Parameters.AddWithValue("@log", req.Longitude);
						command.Parameters.AddWithValue("@lat", req.Latitude); 

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
			List<Request> Requests = new List <Request>();
			try
			{
				using (var connection = new MySqlConnection(_context.Database.GetConnectionString()))
				{
					connection.Open();

					string req = "SELECT * FROM request ";
					using (var command = new MySqlCommand(req, connection))
					{
						using (var reader = command.ExecuteReader())
						{
							while (reader.Read())
							{
								Request request = new Request();

								request.Id = reader.GetInt32(0);
								request.Title = reader.GetString(1);
								request.Description = reader.GetString(2);
								request.IdType = reader.GetInt32(3);
								request.IdUser = reader.GetInt32(4);
								request.Longitude = reader.GetInt32(5);
								request.Latitude = reader.GetInt32(6);
							


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

		[HttpDelete]
		public IActionResult DeleteProduct(int id)
		{

			try
			{

				using (var connection = new MySqlConnection(_context.Database.GetConnectionString()))
				{

					connection.Open();

					string sql = "DELETE FROM request WHERE id=@id";

					using (var command = new MySqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("@id", id);



						command.ExecuteNonQuery();

					}


				}



			}
			catch (Exception ex)
			{
				ModelState.AddModelError("Product", "Sorry, but we have an exception");
				return BadRequest(ModelState);


			}
			return Ok();
		}


	}
}
