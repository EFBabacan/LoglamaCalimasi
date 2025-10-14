using PostaGuvercini.Logging;
using Serilog;


var builder = WebApplication.CreateBuilder(args);

// 1. Serilog'u yapılandırıyoruz.
// Senin yazdığın .UseCustomSerilog() metodu burada çağrılıyor.
//builder.Host.UseCustomSerilog();

// 2. Servisleri konteynere ekliyoruz. (Eski Startup.ConfigureServices metodunun karşılığı)
// Örneğin, MVC veya Razor Pages servisleri burada eklenir.
builder.Services.AddControllersWithViews();
// builder.Services.AddRazorPages();

var app = builder.Build();

// --- DEĞİŞİKLİK BURADA BAŞLIYOR ---
// Sorunun ne olduğunu anlamak için, GEÇİCİ OLARAK her zaman detaylı hata sayfasını gösterelim.
// Orijinal `if` bloğunu yorum satırına alıyoruz:
/*
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
*/

// Yerine bu satırı ekliyoruz. Bu, hatanın kaynağını bize net olarak gösterecektir.
// Not: Bu satır .NET 6 ve sonrasında normalde örtülü olarak eklenir ama
// sorunu bulmak için açıkça yazmak en garantisidir.
app.UseDeveloperExceptionPage();
// --- DEĞİŞİKLİK BURADA BİTİYOR ---


// Serilog'un gelen istekleri de loglaması için bu ara katmanı eklemek çok faydalıdır.
app.UseSerilogRequestLogging();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();