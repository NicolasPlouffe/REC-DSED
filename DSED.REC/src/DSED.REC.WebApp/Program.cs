using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.SqlServer;

using DSED.REC.Application;
using DSED.REC.DataAccesLayer;
using DSED.REC.Entity;
using DSED.REC.Entity.IDepot;
using DSED.REC.WebApp.Hubs;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(option =>
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<ILeadDepot, LeadDepotDepot>();
builder.Services.AddScoped<IValidator<LeadEntity>, LeadValidator>();
builder.Services.AddScoped<LeadServiceBL>();
builder.Services.AddSignalR();
builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy => 
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapHub<DashBoardHub>("/DashBoardHub");
app.MapControllers();
app.Run();