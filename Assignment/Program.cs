using Serilog;
using Assignment.Application;
using Assignment.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.RegisterEfCoreDependencies();
builder.Services.RegisterApplicationDependencies();
var app = builder.Build();

app.UseCors("All");

#region Serilog
Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger();
#endregion

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthorization();

app.MapControllers();

app.Run();
