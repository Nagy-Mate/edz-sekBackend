using Edzések;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<EdzesekDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using var serviceScope = (app as IApplicationBuilder).ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
using var dbContext = serviceScope.ServiceProvider.GetService<EdzesekDbContext>();

if(!dbContext.Edzesek.Any())
{
    var lines = File.ReadAllLines("Edzesek.csv");
    dbContext.Database.OpenConnection();
    dbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Edzesek ON");
    foreach (var line in lines.Skip(1))
    {
        var data = line.Split(",");
        dbContext.Edzesek.Add(new Edzesek
        {
            Azon = int.Parse(data[0]),
            Nev = data[1],
            Tipus = data[2],
            Ido = int.Parse(data[3]),
            Kaloria = int.Parse(data[4]),
            Datum = DateTime.Parse(data[5]),
        });
        dbContext.SaveChanges();
    }
}
app.Run();
