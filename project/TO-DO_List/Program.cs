using BusinessLogicLayer.Service;
using BusinessLogicLayer.Service.Impl;
using DataAccessLayer.DataContext;
using DataAccessLayer.Model;
using DataAccessLayer.Repository;
using DataAccessLayer.Repository.Impl;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Task = DataAccessLayer.Model.Task;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITaskService, TaskService>();

builder.Services.AddScoped<IEntityRepository<User>, UserRepository>();
builder.Services.AddScoped<IEntityRepository<Task>, TaskRepository>();

builder.Services.AddDbContext<TodoListContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
static IHostBuilder CreateHostBuilder(string[] args)=>
    Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuider =>
    {
        webBuider.UseStartup<StartupBase>();
    }).ConfigureLogging(builder => { builder.AddLog4Net("log4net.config"); });



// Add services to the container.
var logger = new LoggerConfiguration()
  .ReadFrom.Configuration(builder.Configuration)
  .Enrich.FromLogContext()
  .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

builder.Services.AddControllersWithViews();

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
