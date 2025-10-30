using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using PakrutiApp.Models;
using System;
using PakrutiApp.Data;

var builder = WebApplication.CreateBuilder(args);

// 🧩 Add services to the container.
builder.Services.AddControllersWithViews();

// 🧩 Add Database Context (SQL Server)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 🧩 Add Session support
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // 30 min session timeout
    options.Cookie.HttpOnly = true;                 // Prevent client-side access
    options.Cookie.IsEssential = true;              // Required for GDPR compliance
});

// 🧩 Build the app
var app = builder.Build();

// 🧩 Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// 🧩 Enable Session Middleware
app.UseSession();

app.UseAuthorization();

// 🧩 Default Routing
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
