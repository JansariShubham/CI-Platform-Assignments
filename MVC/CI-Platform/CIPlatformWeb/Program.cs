
using CIPlatform.entities.DataModels;
using CIPlatform.repository.IRepository;
using CIPlatform.repository.Repository;
using Microsoft.EntityFrameworkCore;
/*using CIPlatform.DataAccess.Data;*/

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddSession();
builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
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
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{area=Users}/{controller=Home}/{action=Login}/{id?}");

app.Run();
