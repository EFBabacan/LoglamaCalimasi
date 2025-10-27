# 📝 Loglama Çalışması

> .NET Core Web API ile kapsamlı loglama ve hata yönetimi uygulaması

[![.NET Version](https://img.shields.io/badge/.NET-8.0-512BD4?style=flat&logo=dotnet)](https://dotnet.microsoft.com/)
[![License](https://img.shields.io/badge/license-MIT-green.svg)](LICENSE)

## 🎯 Proje Hakkında

Bu proje, modern bir .NET Core Web API uygulamasında **loglama**, **hata yönetimi** ve **middleware** kullanımını göstermektedir. Serilog ve özel middleware implementasyonları ile production-ready bir yapı sunmaktadır.

## ✨ Özellikler

- 🔍 **Kapsamlı Loglama**: Serilog ile detaylı log kaydı
- 🛡️ **Global Exception Handling**: Merkezi hata yönetimi
- 📊 **Structured Logging**: JSON formatında yapılandırılmış loglar
- 🔄 **Request/Response Logging**: HTTP isteklerinin otomatik kaydı
- 📁 **Dosya Bazlı Loglama**: Günlük log dosyaları
- 🎨 **Temiz Mimari**: SOLID prensiplere uygun kod yapısı

## 🚀 Kurulum

### Gereksinimler

- .NET 8.0 SDK veya üzeri
- Visual Studio 2022 / VS Code / Rider

### Adımlar

1. **Projeyi klonlayın**
```bash
git clone https://github.com/EFBabacan/LoglamaCalimasi.git
cd LoglamaCalimasi
```

2. **Bağımlılıkları yükleyin**
```bash
dotnet restore
```

3. **Projeyi çalıştırın**
```bash
dotnet run
```

4. **Tarayıcıda açın**
```
https://localhost:5001
```

## 📦 Kullanılan Teknolojiler

| Teknoloji | Versiyon | Açıklama |
|-----------|----------|----------|
| .NET Core | 8.0 | Web API Framework |
| Serilog | 3.x | Loglama kütüphanesi |
| Serilog.Sinks.File | 5.x | Dosyaya loglama |
| Serilog.Sinks.Console | 5.x | Console'a loglama |

## 🏗️ Proje Yapısı

```
LoglamaCalimasi/
│
├── Controllers/          # API Controller'ları
│   └── WeatherForecastController.cs
│
├── Middleware/          # Özel Middleware'ler
│   ├── ExceptionHandlingMiddleware.cs
│   └── RequestLoggingMiddleware.cs
│
├── Models/              # Data modelleri
│   └── WeatherForecast.cs
│
├── Logs/                # Log dosyaları (otomatik oluşur)
│   └── log-20240127.txt
│
├── Program.cs           # Uygulama giriş noktası
└── appsettings.json     # Yapılandırma ayarları
```

## 💡 Kullanım Örnekleri

### API Endpoint'lerini Test Etme

```bash
# WeatherForecast endpoint'ini çağır
curl -X GET "https://localhost:5001/weatherforecast"

# Hata durumunu test et
curl -X GET "https://localhost:5001/weatherforecast/error"
```

### Log Dosyalarını İnceleme

Log dosyaları `Logs` klasöründe günlük olarak oluşturulur:

```
Logs/
├── log-20240127.txt
├── log-20240128.txt
└── log-20240129.txt
```

## 🔧 Yapılandırma

`appsettings.json` dosyasında loglama seviyesini ayarlayabilirsiniz:

```json
{
  "Serilog": {
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Warning",
        "System": "Warning"
      }
    }
  }
}
```

## 📝 Loglama Seviyeleri

- **Verbose**: En detaylı loglar
- **Debug**: Geliştirme aşaması için
- **Information**: Genel bilgi logları
- **Warning**: Uyarı mesajları
- **Error**: Hata logları
- **Fatal**: Kritik hatalar

## 🤝 Katkıda Bulunma

1. Bu repository'yi fork edin
2. Feature branch oluşturun (`git checkout -b feature/YeniOzellik`)
3. Değişikliklerinizi commit edin (`git commit -m 'Yeni özellik eklendi'`)
4. Branch'inizi push edin (`git push origin feature/YeniOzellik`)
5. Pull Request oluşturun

## 📄 Lisans

Bu proje MIT lisansı altında lisanslanmıştır. Detaylar için [LICENSE](LICENSE) dosyasına bakınız.

## 👤 Geliştirici

**Emre Furkan Babacan**

- GitHub: [@EFBabacan](https://github.com/EFBabacan)

## 🌟 Teşekkürler

Bu projeyi beğendiyseniz ⭐ vermeyi unutmayın!

---

<div align="center">
Made with ❤️ by Emre Furkan Babacan
</div>