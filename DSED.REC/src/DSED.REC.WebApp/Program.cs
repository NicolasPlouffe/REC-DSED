using DSED.REC.Application;
using Microsoft.EntityFrameworkCore.SqlServer;
using DSED.REC.DataAccesLayer;
using DSED.REC.Entity;
using DSED.REC.Entity.IDepot;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(option =>
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<ILeadDepot, LeadDepotDepot>();
builder.Services.AddScoped<IValidator<LeadEntity>, LeadValidator>();
builder.Services.AddScoped<LeadServiceBL>();
/*
builder.Services.AddScoped<SignalR>();
*/

builder.Services.AddControllers();
// Pour Postman
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy => 
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
// pour permettre 
app.UseCors();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
// app.MapHub<LeadStatsHub>("/LeadHub"); 

app.Run();