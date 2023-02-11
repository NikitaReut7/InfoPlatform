using Info.CommandService.Data;
using Info.CommandService.SyncDataServices.Grps;
using Microsoft.EntityFrameworkCore;
using Info.Common.MassTransit;
using Info.CommandService.Data.CommandRepository;
using Info.CommandService.Data.PlatformRepository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransitWithRabbitMq();

if (builder.Environment.IsProduction())
{
    Console.WriteLine("--> User PostgreSQL Db");

    builder.Services.AddDbContext<AppDbContext>(opt => 
        opt.UseNpgsql(builder.Configuration.GetConnectionString("CommandsConn")));
}
else
{
    Console.WriteLine("--> User InMeM Db");

    builder.Services.AddDbContext<AppDbContext>(opt =>
        opt.UseInMemoryDatabase("InMem"));
}

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<IPlatformRepository, PlatformRepository>();
builder.Services.AddScoped<ICommandRepository, CommandRepository>();
builder.Services.AddScoped<IPlatformDataClient, PlatformDataClient>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

PrepDb.PrepPopulation(app, builder.Environment.IsProduction());


app.Run();
