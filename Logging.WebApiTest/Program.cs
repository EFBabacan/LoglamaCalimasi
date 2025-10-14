
using PostaGuvercini.Logging;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseCustomSerilog();
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting(); // <-- Bundan sonra

app.UseCorrelationId(); // <-- BÝZÝM MÝDDLEWARE'ÝMÝZ

app.UseAuthorization(); // <-- Bundan önce

app.MapControllers();

app.Run();
