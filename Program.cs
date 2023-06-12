using Microsoft.EntityFrameworkCore;
using ZaculeuValley.IxchelAdmin.Models;
using Microsoft.AspNetCore.Identity;
using ZaculeuValley.IxchelAdmin.Data;
using ZaculeuValley.IxchelAdmin.Areas.Identity.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Injeccion de dependencias con dbcontext servicio
var connection = builder.Configuration.GetConnectionString("connectionDB");
builder.Services.AddDbContext<IxchelWebpruebasContext>(options => options.UseSqlServer(connection));
builder.Services.AddDbContext<AuthContext>(options => options.UseSqlServer(connection));
builder.Services.AddDefaultIdentity<AdminUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<AuthContext>();


builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

builder.Services.AddRazorPages();

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

app.UseSession(); // Add this line to enable session state

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Institutions}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
