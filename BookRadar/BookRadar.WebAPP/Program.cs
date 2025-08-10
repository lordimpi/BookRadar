using BookRadar.Common.Configurations;
using BookRadar.Configurations.ServiceCollection;
using BookRadar.DataAccess.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Registrar soporte para IGenericOptionsService<>
builder.Services.AddBookRadarOptionsCore();

//Vincular clases Configuration con la sección del appsettings.json
builder.Services.BindOptions<OpenLibraryOptions>(builder.Configuration, "APIs");
builder.Services.BindOptions<DbOptions>(builder.Configuration, "ConnectionStrings");

var app = builder.Build();

//Crear BD y aplicar migraciones si es necesario
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

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
    pattern: "{controller=Search}/{action=Index}/{id?}");

app.Run();
