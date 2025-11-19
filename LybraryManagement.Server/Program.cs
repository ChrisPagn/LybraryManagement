//program.cs cote server

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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Avant app.Run()
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}
app.UseCors("DevCors");

app.UseAuthorization();

app.MapControllers();

app.Run();
