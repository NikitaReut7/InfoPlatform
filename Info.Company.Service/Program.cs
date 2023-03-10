using Info.Common.MassTransit;
using Info.CompanyService.Data;
using Info.CompanyService.Models;
using Info.CompanyService.SyncDataServices.Http;
using Microsoft.EntityFrameworkCore;
using Info.Common;
using Info.Common.Repository;
using Info.CompanyService.Data.CompanyRepository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

if (builder.Environment.IsProduction())
{
    Console.WriteLine("--> User SqlServer Db");

    builder.Services
        .AddDbContext<AppDbContext>(opt =>
        opt.UseSqlServer(builder.Configuration.GetConnectionString("CompaniesConn")));
}
else
{
    Console.WriteLine("--> User InMeM Db");

    builder.Services.AddDbContext<AppDbContext>(opt =>
        opt.UseInMemoryDatabase("InMem"));
}

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddHttpClient<IPlatformDataClient, HttpPlatformDataClient>();


builder.Services.AddMassTransitWithRabbitMq();


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
