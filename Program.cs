using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WikipediaDAW.ContextModels;
using Microsoft.Extensions.DependencyInjection;
using WikipediaDAW.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<UtilizatorContext>(options =>
  options.UseSqlServer(builder.Configuration.GetConnectionString("db")));

builder.Services.AddIdentity<User, IdentityRole>()
     .AddEntityFrameworkStores<UtilizatorContext>()
     .AddDefaultTokenProviders();

async Task CreateRoles(IServiceProvider serviceProvider)
{
    //initializing custom roles 
    var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var UserManager = serviceProvider.GetRequiredService<UserManager<User>>();
    string[] roleNames = { Roles.User, Roles.Moderator, Roles.Admin };
    IdentityResult roleResult;

    foreach (var roleName in roleNames)
    {
        var roleExist = await RoleManager.RoleExistsAsync(roleName);
        if (!roleExist)
        {
            //create the roles and seed them to the database: Question 1
            roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
        }
    }
}

await CreateRoles(builder.Services.BuildServiceProvider());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html"); ;

app.Run();
