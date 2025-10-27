Geliþmiþ Yapýlandýrma: Kural Bazlý Filtreleme ve Yönlendirme
PostaGuvercini.Logging kütüphanesi, loglarý sadece seviyelerine göre deðil, ayný zamanda kaynaklarýna (namespace) ve içeriklerine göre filtreleyip farklý hedeflere (sink'lere) yönlendirmek için güçlü ve esnek bir yapýlandýrma sunar. Tüm bu kurallar, uygulamanýzýn appsettings.json dosyasý üzerinden, kod deðiþikliði yapmadan yönetilebilir.

Temel Yapý
Yapýlandýrma, appsettings.json dosyasýndaki "Serilog" bölümü altýnda yapýlýr. Anahtar bileþenler þunlardýr:

"Using": Serilog'un, standart olmayan sink'leri (bizim BatchedFileSink gibi) veya yapýlandýrma metotlarýný bulabilmesi için taranacak assembly'leri (kütüphaneleri) belirtir. Kendi özel sink'lerinizi kullanýyorsanýz, ilgili kütüphaneyi buraya eklemelisiniz.

JSON

"Using": [
  "PostaGuvercini.Logging", 
  "Serilog.Sinks.File",
  "Serilog.Sinks.Seq" 
],
"MinimumLevel": Loglama seviyesini dinamik olarak kontrol etmek için kullanýlýr. "ControlledBy" ile bir "LevelSwitches" deðiþkenine baðlanabilir.

JSON

"LevelSwitches": { "$controlSwitch": "Information" },
"MinimumLevel": {
  "ControlledBy": "$controlSwitch",
  "Override": { // Belirli kaynaklar için seviyeyi ezme
    "Microsoft": "Warning",
    "System": "Warning"
  }
},
"WriteTo": Loglarýn gönderileceði hedefleri (sink'leri) ve yönlendirme kurallarýný tanýmlayan bir dizidir. Her bir eleman bir kuralý temsil eder.

Kural Bazlý Yönlendirme (WriteTo.Logger)
Loglarý farklý kriterlere göre farklý hedeflere göndermek için "Name": "Logger" yapýsýný kullanýrýz. Her Logger bloðu kendi filtreleme (Filter), seviye (MinimumLevel) ve hedef (WriteTo) tanýmlarýný içerebilir.

Örnek Kurallar:

Acil Durum Loglarý (Ýçeriðe Göre Filtreleme): Log mesajýnda "Ödeme Baþarýsýz" geçen veya EventType özelliði 'PaymentFailure' olan loglarý Acil_Hatalar.txt dosyasýna yazar. Filtreleme için Serilog.Expressions kullanýlýr.

JSON

{
  "Name": "Logger",
  "Args": {
    "configureLogger": {
      "Filter": [
        {
          "Name": "ByIncludingOnly",
          "Args": { "expression": "Contains(RenderedMessage, 'Ödeme Baþarýsýz') or EventType = 'PaymentFailure'" }
        }
      ],
      "WriteTo": [
        {
          "Name": "BatchedFileSink", // Kendi özel sink'imiz
          "Args": {
            "filePath": "logs/Acil_Hatalar.txt",
            "options": { "batchSizeLimit": 10, "period": "0.00:00:02" }
          }
        }
      ]
    }
  }
}
Microsoft Loglarý (Namespace'e Göre Filtreleme): SourceContext'i Microsoft. ile baþlayan loglarý, sadece Warning seviyesinden itibaren microsoft-logs.txt dosyasýna yazar.

JSON

{
  "Name": "Logger",
  "Args": {
    "configureLogger": {
      "Filter": [
        {
          "Name": "ByIncludingOnly",
          "Args": { "expression": "StartsWith(SourceContext, 'Microsoft.')" }
        }
      ],
      "MinimumLevel": "Warning", // Bu kural için seviye override
      "WriteTo": [
        {
          "Name": "File", // Standart File sink
          "Args": { "path": "logs/microsoft-logs.txt", "rollingInterval": "Day" }
        }
      ]
    }
  }
}
Uygulama Loglarý (Seq Hedefi): Bizim uygulamalarýmýzýn (PostaGuvercini.Logging, Logging.WebApiTest vb.) ürettiði (ama acil olmayan) loglarý Information seviyesinden itibaren Seq sunucusuna gönderir.

JSON

{
  "Name": "Logger",
  "Args": {
    "configureLogger": {
      "Filter": [
        { // Acil loglarý hariç tut
          "Name": "ByExcluding", 
          "Args": { "expression": "Contains(RenderedMessage, 'Ödeme Baþarýsýz') or EventType = 'PaymentFailure'" }
        },
        { // Bizim uygulamalarý dahil et
          "Name": "ByIncludingOnly", 
          "Args": { "expression": "StartsWith(SourceContext, 'Logging.WebApiTest') or StartsWith(SourceContext, 'PostaGuvercini.Logging')" } 
        }
      ],
      "MinimumLevel": "Information",
      "WriteTo": [ { "Name": "Seq", "Args": { "serverUrl": "http://localhost:5341" } } ]
    }
  }
}
Not: Kurallar WriteTo dizisindeki sýrayla iþlenir. Bir log, bir Logger bloðunun filtresine takýlýrsa ve o blok logu bir hedefe yazarsa, genellikle sonraki Logger bloklarý tarafýndan tekrar iþlenmez (eðer filtreler buna göre ayarlandýysa).

Ortama Özel Yapýlandýrma
ASP.NET Core'un standart yapýlandýrma modeli sayesinde, farklý ortamlar (Development, Staging, Production) için farklý Serilog kurallarý tanýmlayabilirsiniz.

appsettings.Development.json: Geliþtirme sýrasýnda daha detaylý loglama (örn: Debug seviyesi, Konsol çýktýsý açýk, tüm loglar Seq'e) için ayarlarý buraya yazýn.

appsettings.Production.json: Canlý ortam için optimize edilmiþ ayarlarý (örn: Information seviyesi, Konsol kapalý, sadece önemli loglar Seq'e, gürültülü loglar ayrý dosyalara) buraya yazýn.

Uygulama çalýþýrken, ilgili ortama ait .json dosyasýndaki "Serilog" bölümü, ana appsettings.json'daki bölümün üzerine yazýlýr. Bu sayede, kod deðiþikliði yapmadan ortamlar arasýnda farklý loglama davranýþlarý elde edilebilir.

Kurulum: Kütüphanenin entegrasyonu için Program.cs dosyasýnda builder.Host.AddCustomLogging(lb => lb.UseSerilogAdapter()); çaðrýsýnýn yapýlmýþ olmasý yeterlidir. Tüm bu JSON yapýlandýrmasý otomatik olarak okunacaktýr.