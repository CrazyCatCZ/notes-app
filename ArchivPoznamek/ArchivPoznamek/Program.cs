using Microsoft.EntityFrameworkCore;
using ArchivPoznamek.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();

// Datab�ze
builder.Services.AddDbContext<ArchivPoznamekData>(options =>
               options.UseSqlServer(builder.Configuration.GetConnectionString("ArchivPoznamek")));

// Session
builder.Services.AddSession(options => {
    options.Cookie.Name = ".ArchivPoznamek";
    options.Cookie.IsEssential = true;
    options.IdleTimeout = TimeSpan.FromMinutes(30);
});
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

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
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Poznamky}/{action=Vypsat}/{id?}");

app.Run();
