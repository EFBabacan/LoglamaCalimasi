using PostaGuvercini.Logging; // Kendi k�t�phanemizi ekliyoruz
using Serilog;

var builder = WebApplication.CreateBuilder(args);

//******************************************************************
// DE����KL�K 1: ESK� SERILOG YAPILANDIRMASINI S�L�P BUNU EKL�YORUZ
// builder.Host.UseSerilog(...); sat�rlar� yerine kendi metodumuzu �a��r�yoruz.
builder.Host.UseCustomSerilog();
//******************************************************************


// Add services to the container.
builder.Services.AddControllers();
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

//******************************************************************
// DE����KL�K 2: CORRELATION ID MIDDLEWARE'�N� EKL�YORUZ
// Bu sat�r, her iste�e bir kimlik atayacak.
app.UseCorrelationId();
//******************************************************************

app.UseAuthorization();

app.MapControllers();

app.Run();