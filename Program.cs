//using healt_Center.Models;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Options;

//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.
//builder.Services.AddControllersWithViews();


////add session



////add new

//var provider=builder.Services.BuildServiceProvider();
//var confing=provider.GetService<IConfiguration>();
//builder.Services.AddDbContext<Patient_DbContext>(item => item.UseSqlServer(confing.GetConnectionString("con")));
////add  end new

//builder.Services.AddSession(option =>
//{
//    option.IdleTimeout = TimeSpan.FromMinutes(20);
//});

//var app = builder.Build();
//app.UseHttpsRedirection();
//app.Use(async (context, next) =>
//{
//    context.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
//    context.Response.Headers["Pragma"] = "no-cache";
//    context.Response.Headers["Expires"] = "-1";
//    await next();
//});

//// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//}

//app.UseSession();


//app.UseStaticFiles();

//app.UseRouting();

//app.UseAuthorization();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Daseboard}/{action=Index}/{id?}");

//app.Run();


using healt_Center.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 🔥 Swagger services
builder.Services.AddEndpointsApiExplorer(); // ⭐️
builder.Services.AddSwaggerGen();           // ⭐️

builder.Services.AddControllersWithViews(); // 👈 still keeping this for MVC + API mix

// 🔥 DbContext
var provider = builder.Services.BuildServiceProvider();
var confing = provider.GetService<IConfiguration>();
builder.Services.AddDbContext<Patient_DbContext>(item =>
    item.UseSqlServer(confing.GetConnectionString("con")));

// 🔥 Session
builder.Services.AddSession(option =>
{
    option.IdleTimeout = TimeSpan.FromMinutes(20);
});

var app = builder.Build();

// 🔥 Swagger middleware (must come before other middlewares)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();       // ⭐️
    app.UseSwaggerUI();     // ⭐️
}

app.UseHttpsRedirection();

app.Use(async (context, next) =>
{
    context.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
    context.Response.Headers["Pragma"] = "no-cache";
    context.Response.Headers["Expires"] = "-1";
    await next();
});

app.UseSession();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// 🔥 Important: allow API endpoints too
app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Daseboard}/{action=Index}/{id?}");

app.Run();

