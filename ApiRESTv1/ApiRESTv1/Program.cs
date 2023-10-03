using ApiRESTv1.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddCors(options =>
{
	options.AddDefaultPolicy(
		Policy =>
		{
			Policy.AllowAnyOrigin() // Replace with your Vue.js app's URL
					   .AllowAnyHeader()
					   .AllowAnyMethod();
			
		});
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("MySqlCon");
builder.Services.AddDbContext<AppDbContext>(options =>
{
	options.UseMySql(connectionString,ServerVersion.AutoDetect(connectionString));	

});
var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}
app.UseCors();
app.MapControllers();



app.UseHttpsRedirection();

app.UseAuthorization();



app.Run();
