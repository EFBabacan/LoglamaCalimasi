using PostaGuvercini.Logging; // Kendi k�t�phanemizi ekliyoruz
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// YEN�, ESNEK KURULUM KODUMUZ:
builder.Host.AddCustomLogging(loggingBuilder =>
{
    // Hangi adapt�r� kullanmak istedi�imizi burada se�iyoruz.
    loggingBuilder.UseSerilogAdapter();

    // Yar�n NLog adapt�r� yazarsak:
    // loggingBuilder.UseNLogAdapter();
});


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