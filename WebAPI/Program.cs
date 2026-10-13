using Scalar.AspNetCore;
using ClassLibraryDel;
using ClassLibraryDel.DataAccess;
using UserInterface.Components;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Register the product repository implementation for dependency injection
builder.Services.AddSingleton<ClassLibraryDel.DataAccess.Interfaces.IProductRepository, DataAccessLayer.SupabaseProductRepo>();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddRazorComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseAuthorization();

app.MapControllers();
app.MapRazorComponents<App>()
.AddInteractiveServerRenderMode();
app.Run();
