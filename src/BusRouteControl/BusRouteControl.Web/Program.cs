using BusRouteControl.Application.Common.Interfaces;
using BusRouteControl.Application.Services;
using BusRouteControl.Application.Services.BusRouteControl.Application.Services;
using BusRouteControl.Infrastructure.Data;
using BusRouteControl.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<BusRouteControlDbContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("StrConnection")));
builder.Services.AddScoped<IRouteRepository, RouteRepository>();
builder.Services.AddScoped<IBusRouteService, BusRouteService>();
builder.Services.AddScoped<IScheduleRepository, ScheduleRepository>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

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
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
