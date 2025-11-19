var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// CORS pour autoriser le client Blazor pendant le dev
builder.Services.AddCors(options =>
{
    options.AddPolicy("DevCors", p => p
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod());
});

// Register application/infrastructure services
LybraryManagement.Server.StartupExtensions.RegisterApplication(builder.Services, builder.Configuration);

var app = builder.Build();

// Création automatique de la DB en développement
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<LybraryManagement.Server.Infrastructure.Data.LibraryDbContext>();
    
    // Crée la base de données et les tables si elles n'existent pas
    dbContext.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseWebAssemblyDebugging(); // Pour déboguer le client WASM
}

// IMPORTANT : Servir les fichiers statiques du client Blazor
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

// Avant app.Run()
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseCors("DevCors");
app.UseAuthorization();

app.MapControllers();

// CRITIQUE : Rediriger toutes les routes non-API vers index.html
app.MapFallbackToFile("index.html");

app.Run();