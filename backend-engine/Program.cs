﻿using backend_engine.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Newtonsoft;
using Microsoft.Extensions.Configuration;
using backend_engine.Models;
using backend_engine.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


// Repositories
//builder.Services.AddSingleton<IRepository<Stock>,StockRepository>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();
builder.Services.AddDbContext<BreezeDataContext>(options => options.UseSqlServer("Server=tcp:breeze.database.windows.net,1433;Initial Catalog=BreezeData;Persist Security Info=False;User ID=breezeadmin;Password=!Breeze27;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"));
builder.Services.AddControllers().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

builder.Services.AddAzureClients(builder => {
    builder.AddBlobServiceClient("DefaultEndpointsProtocol=https;AccountName=hermesworkbooks;AccountKey=EaVNOEBoTyhe+S330Ec670TN0WlqHPdBNJ+Yp6eUB0q1XWGrpl/xE5GH0aVciVey42PRn+ZZSP0l+AStM9JbHg==;EndpointSuffix=core.windows.ne");

});

builder.Services.AddTransient<IStorageService, StorageService>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
