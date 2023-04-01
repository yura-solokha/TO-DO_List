using BusinessLogicLayer.Service;
using BusinessLogicLayer.Service.Impl;
using DataAccessLayer.DataContext;
using DataAccessLayer.Model;
using DataAccessLayer.Repository;
using DataAccessLayer.Repository.Impl;
using Microsoft.EntityFrameworkCore;
using TO_DO_List.Mappers;
using Task = DataAccessLayer.Model.Task;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

builder.Services.AddScoped<IEntityRepository<User>, UserRepository>();
builder.Services.AddScoped<IEntityRepository<Task>, TaskRepository>();
builder.Services.AddScoped<IEntityRepository<Category>, CategoryRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();

builder.Services.AddDbContext<TodoListContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")!
        .Replace("SECRET(", "")
        .Replace("DBPassword", builder.Configuration["DBPassword"])
        .Replace(")", "")));


static IHostBuilder CreateHostBuilder(string[] args)=>
    Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder =>
    {
        webBuilder.UseStartup<StartupBase>();
    }).ConfigureLogging(builder => { builder.AddLog4Net("log4net.config"); });

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAutoMapper(typeof(AppMappingProfile));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/User/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=User}/{action=Login}");

app.Run();
