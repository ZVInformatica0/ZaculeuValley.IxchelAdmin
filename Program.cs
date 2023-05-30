using Microsoft.EntityFrameworkCore;
using ZaculeuValley.IxchelAdmin.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
//injeccion de dependencias con dbcontext servicio
var connection = builder.Configuration.GetConnectionString("connectionDB");
builder.Services.AddDbContext<IxchelWebpruebasContext>(options => options.UseSqlServer(connection));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Institutions}/{action=Index}/{id?}");

app.Run();
