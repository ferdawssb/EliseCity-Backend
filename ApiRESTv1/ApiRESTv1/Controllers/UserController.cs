using ApiRESTv1.Data;
using ApiRESTv1.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

namespace ApiRESTv1.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly AppDbContext _context;

		public UserController(AppDbContext context)
		{
			_context = context;
		}


		[HttpGet()]
		public IActionResult GetAdmins()
		{
			List<User> Users = new List<User>();
			try
			{
				using (var connection = new MySqlConnection(_context.Database.GetConnectionString()))
				{
					connection.Open();

					string req = "SELECT * FROM users";
					using (var command = new MySqlCommand(req, connection))
					{
						using (var reader = command.ExecuteReader())
						{
							while (reader.Read())
							{
								var admin = new User
								{
									Id = reader.GetInt32(0),
									Firstname = reader.GetString(1),
									Lastname = reader.GetString(2),
									Mobile = reader.GetString(3),
									login = reader.GetString(4),
									Password = reader.GetString(5),
									CreateAt = reader.GetDateTime(6),
									UpdateAt = reader.GetDateTime(7)
								};

								Users.Add(admin);
							}
						}
					}
				}
			}
			catch (Exception e)
			{
				ModelState.AddModelError("UsersGetError", "Désolé, mais nous avons rencontré une exception.");
				return BadRequest(ModelState);
			}
			return Ok(Users);
		}

		[HttpGet("{id}")]
		public IActionResult GetOneUser(int id)
		{

			try
			{

				using (var connection = new MySqlConnection(_context.Database.GetConnectionString()))
				{
					connection.Open();

					string req = "SELECT * FROM users where id = @id";
					using (var command = new MySqlCommand(req, connection))
					{
						command.Parameters.AddWithValue("@id", id);
						using (var reader = command.ExecuteReader())
						{
							reader.Read();
							return Ok(new User
							{
								Id = reader.GetInt32(0),
								Firstname = reader.GetString(1),
								Lastname = reader.GetString(2),
								Mobile = reader.GetString(3),
								login = reader.GetString(4),
								Password = reader.GetString(5),
								CreateAt = reader.GetDateTime(6),
								UpdateAt = reader.GetDateTime(7)
							});

						}
					}
				}

			}
			catch (Exception ex)
			{
				ModelState.AddModelError("UserGetError", "Sorry, but we have an exception");
				return BadRequest(ModelState);

			}

		}
		[HttpPost]
		public IActionResult CreateProduct(CompteUserDto CompteAd)
		{
			try
			{
				using (var connection = new MySqlConnection(_context.Database.GetConnectionString()))
				{
					connection.Open();
					String sql = "INSERT INTO users" +
						"(Firstname,Lastname,Mobile,Login,Password) VALUES " +
						"(@firstname, @lastname, @mobile, @login, @password)";


					using (var command = new MySqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("@firstname", CompteAd.Firstname);
						command.Parameters.AddWithValue("@lastname", CompteAd.Lastname);
						command.Parameters.AddWithValue("@Mobile", CompteAd.Mobile);
						command.Parameters.AddWithValue("@login", CompteAd.login);
						command.Parameters.AddWithValue("@password", CompteAd.Password);


						command.ExecuteNonQuery();

					}

				}

			}
			catch (Exception e)
			{
				ModelState.AddModelError("UserPostError", "Désolé, mais nous avons rencontré une exception.");
				return BadRequest(ModelState);
			}

			return Ok();

		}

		[HttpPut("{id}")]
		public IActionResult UpdateCompteAd(int id, CompteUserDto Compte)
		{

			try
			{

				using (var connection = new MySqlConnection(_context.Database.GetConnectionString()))
				{

					connection.Open();

					string sql = "UPDATE users SET Firstname=@firstname, Lastname=@lastname, Mobile=@mobile, Login=@login, Password=@password, UpdateAt=NOW()  " +
						" WHERE Id=@id";

					using (var command = new MySqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("@firstname", Compte.Firstname);
						command.Parameters.AddWithValue("@lastname", Compte.Lastname);
						command.Parameters.AddWithValue("@Mobile", Compte.Mobile);
						command.Parameters.AddWithValue("@login", Compte.login);
						command.Parameters.AddWithValue("@password", Compte.Password);
						command.Parameters.AddWithValue("@id", id);
						command.ExecuteNonQuery();

					}


				}

			}
			catch (Exception ex)
			{
				ModelState.AddModelError("UserUpdateError", "Sorry, but we have an exception");
				return BadRequest(ModelState);

			}
			return Ok();

		}


		[HttpDelete]
		public IActionResult DeleteProduct(int id)
		{
			try
			{
				using (var connection = new MySqlConnection(_context.Database.GetConnectionString()))
				{
					connection.Open();
					string sql = "DELETE FROM users WHERE Id=@id";
					using (var command = new MySqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("@id", id);
						command.ExecuteNonQuery();
					}
				}
			}
			catch (Exception ex)
			{
				if (ex.Message.Contains("foreign key"))
				{
					ModelState.AddModelError("UserCantBeDeleted", "L'utilisateur que vous voulez supprimer a une demande en cours.");
				}
				else
				{
					ModelState.AddModelError("UserDeleteError", "Sorry, but we have an exception");
				}
				return BadRequest(ModelState);
			}
			return Ok();
		}
	}
}

