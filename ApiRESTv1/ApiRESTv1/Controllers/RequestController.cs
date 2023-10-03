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
		public async Task<IActionResult> CreateRequest(RequestDto req)
		{
			try
			{
				// Recherchez l'ID du type de demande en fonction du titre fourni dans la DTO
				var typeRequest = await _context.request_type.FirstOrDefaultAsync(tr => tr.title == req.IdType);
				if (typeRequest == null)
				{
					ModelState.AddModelError("Request", "Le type de demande spécifié n'existe pas.");
					return BadRequest(ModelState);
				}

				// Recherchez l'ID de l'utilisateur en fonction du prénom fourni dans la DTO
				var user = await _context.users.FirstOrDefaultAsync(u => u.Firstname == req.IdUser);
				if (user == null)
				{
					ModelState.AddModelError("Request", "L'utilisateur spécifié n'existe pas.");
					return BadRequest(ModelState);
				}

				// Vous pouvez créer une instance de la demande à partir de la DTO
				var request = new Request
				{
					Title = req.Title,
					Description = req.Description,
					IdType = typeRequest.Id, // Utilisez l'ID du type de demande trouvé dans la table type_request
					IdUser = user.Id, // Utilisez l'ID de l'utilisateur trouvé dans la table users
					Longitude = req.Longitude,
					Latitude = req.Latitude
				};

				// Ajoutez la demande à votre contexte de base de données et sauvegardez les modifications
				_context.Requests.Add(request);
				await _context.SaveChangesAsync();

				return Ok();
			}
			catch (Exception e)
			{
				ModelState.AddModelError("Request", "Désolé, mais nous avons rencontré une exception.");
				return BadRequest(ModelState);
			}
		}






















2
		/*[HttpPost]
		public IActionResult CreateProduct(RequestDto req)
		{
			try
			{
				using (var connection = new MySqlConnection(_context.Database.GetConnectionString()))
				{
					connection.Open();

					// Récupérer l'ID du type de demande en utilisant le titre
					string typeIdSql = "SELECT id FROM request_type WHERE title = @title";
					int typeId;

					using (var getTypeIdCommand = new MySqlCommand(typeIdSql, connection))
					{
						getTypeIdCommand.Parameters.AddWithValue("@title", req.IdType); // Utilisez req.Id_type au lieu de typeIdSql
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

					// Récupérer l'ID de l'utilisateur en utilisant le nom d'utilisateur
					string userIdSql = "SELECT Id FROM users WHERE Firstname = @Firstname";
				
					using (var getUserIdCommand = new MySqlCommand(userIdSql, connection))
					{
						getUserIdCommand.Parameters.AddWithValue("@Firstname", req.IdUser); // Utilisez req.Id_utilisateur au lieu de userIdSql
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

					// Maintenant que vous avez les IDs correspondants, vous pouvez insérer la demande
					string insertSql = "INSERT INTO request(title, description, id_type, id_user, Longitude, Latitude) " +
									   "VALUES (@title, @description, @id_type, @id_user, @log, @lat)";

					using (var command = new MySqlCommand(insertSql, connection))
					{
						command.Parameters.AddWithValue("@title", req.Title); // Utilisez req.Titre au lieu de req.Title
						command.Parameters.AddWithValue("@description", req.Description);
						command.Parameters.AddWithValue("@id_type", typeId);
						command.Parameters.AddWithValue("@id_user", userId);
						command.Parameters.AddWithValue("@log", req.Longitude); // Utilisez req.longitude au lieu de req.Longitude
						command.Parameters.AddWithValue("@lat", req.Latitude); // Utilisez req.latitude au lieu de req.Latitude

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
		}*/



	}
}
