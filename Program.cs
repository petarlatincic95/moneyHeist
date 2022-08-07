global using MoneyHeistAPI.Data;
global using Microsoft.EntityFrameworkCore;
global using System.Text.Json.Serialization;
global using Microsoft.AspNetCore.Mvc;
global using MoneyHeistAPI.DTO_Helper_classes;
global using MoneyHeistAPI.Helper_Methodes;
global using MoneyHeistAPI.Model;
global using System.Net.Mail;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<HeistDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("HeistDbConnectionString"));
});
builder.Services.AddMvc().AddXmlSerializerFormatters();
builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles); //Had to add it to be able to send all skills of particular HeistMember


var app = builder.Build();




//// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
