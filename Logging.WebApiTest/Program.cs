using PostaGuvercini.Logging; // Kendi kütüphanemizi ekliyoruz
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// YENÝ, ESNEK KURULUM KODUMUZ:
builder.Host.AddCustomLogging(loggingBuilder =>
{
    // Hangi adaptörü kullanmak istediðimizi burada seçiyoruz.
    loggingBuilder.UseSerilogAdapter();

    // Yarýn NLog adaptörü yazarsak:
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
// DEÐÝÞÝKLÝK 2: CORRELATION ID MIDDLEWARE'ÝNÝ EKLÝYORUZ
// Bu satýr, her isteðe bir kimlik atayacak.
app.UseCorrelationId();
//******************************************************************

app.UseAuthorization();

app.MapControllers();

app.Run();