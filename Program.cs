var builder = WebApplication.CreateBuilder(args);

// Verificar que existe la configuración necesaria
var baseUrl = builder.Configuration["ApiSettings:BaseUrl"];
if (string.IsNullOrEmpty(baseUrl))
{
    throw new Exception("ApiSettings:BaseUrl no está configurado");
}

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();

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

// Ruta específica para el callback de Google
app.MapControllerRoute(
    name: "callback",
    pattern: "callback",
    defaults: new { controller = "Callback", action = "Index" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
