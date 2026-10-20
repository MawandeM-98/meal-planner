var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// The Angular dev server runs on 4200, so the browser needs permission to call us.
builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod());
});

var app = builder.Build();

app.UseCors("Frontend");
app.MapControllers();

app.Run();