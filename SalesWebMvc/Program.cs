using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Data;


var builder = WebApplication.CreateBuilder(args);

// Connection String
var connectionString = builder.Configuration.GetConnectionString("SalesWebMvcContext");

// DbContext
builder.Services.AddDbContext<SalesWebMvcContext>(options =>
    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString)
    ));

// Services
builder.Services.AddScoped<SeedingService>();

// MVC
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.UseStaticFiles();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


// Executa o Seed
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var seedingService = services.GetRequiredService<SeedingService>();

    seedingService.Seed();
}

app.Run();