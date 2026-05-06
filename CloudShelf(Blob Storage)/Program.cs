using CloudShelf_Blob_Storage_.Data;
using CloudShelf_Blob_Storage_.Services.Implementation;
using CloudShelf_Blob_Storage_.Services.Interface;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//tell app to use BlobService when IBlobService is requested
//it is dependency injection, and i am using it 2nd type which is scoped
builder.Services.AddScoped<IBlobService, BlobService>();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddHttpClient();

//Setting up the Db Context for the application
builder.Services.AddDbContext<AppDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
