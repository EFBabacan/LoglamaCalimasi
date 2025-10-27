Geli�mi� Yap�land�rma: Kural Bazl� Filtreleme ve Y�nlendirme
PostaGuvercini.Logging k�t�phanesi, loglar� sadece seviyelerine g�re de�il, ayn� zamanda kaynaklar�na (namespace) ve i�eriklerine g�re filtreleyip farkl� hedeflere (sink'lere) y�nlendirmek i�in g��l� ve esnek bir yap�land�rma sunar. T�m bu kurallar, uygulaman�z�n appsettings.json dosyas� �zerinden, kod de�i�ikli�i yapmadan y�netilebilir.

Temel Yap�
Yap�land�rma, appsettings.json dosyas�ndaki "Serilog" b�l�m� alt�nda yap�l�r. Anahtar bile�enler �unlard�r:

"Using": Serilog'un, standart olmayan sink'leri (bizim BatchedFileSink gibi) veya yap�land�rma metotlar�n� bulabilmesi i�in taranacak assembly'leri (k�t�phaneleri) belirtir. Kendi �zel sink'lerinizi kullan�yorsan�z, ilgili k�t�phaneyi buraya eklemelisiniz.

JSON

"Using": [
  "PostaGuvercini.Logging", 
  "Serilog.Sinks.File",
  "Serilog.Sinks.Seq" 
],
"MinimumLevel": Loglama seviyesini dinamik olarak kontrol etmek i�in kullan�l�r. "ControlledBy" ile bir "LevelSwitches" de�i�kenine ba�lanabilir.

JSON

"LevelSwitches": { "$controlSwitch": "Information" },
"MinimumLevel": {
  "ControlledBy": "$controlSwitch",
  "Override": { // Belirli kaynaklar i�in seviyeyi ezme
    "Microsoft": "Warning",
    "System": "Warning"
  }
},
"WriteTo": Loglar�n g�nderilece�i hedefleri (sink'leri) ve y�nlendirme kurallar�n� tan�mlayan bir dizidir. Her bir eleman bir kural� temsil eder.

Kural Bazl� Y�nlendirme (WriteTo.Logger)
Loglar� farkl� kriterlere g�re farkl� hedeflere g�ndermek i�in "Name": "Logger" yap�s�n� kullan�r�z. Her Logger blo�u kendi filtreleme (Filter), seviye (MinimumLevel) ve hedef (WriteTo) tan�mlar�n� i�erebilir.

�rnek Kurallar:

Acil Durum Loglar� (��eri�e G�re Filtreleme): Log mesaj�nda "�deme Ba�ar�s�z" ge�en veya EventType �zelli�i 'PaymentFailure' olan loglar� Acil_Hatalar.txt dosyas�na yazar. Filtreleme i�in Serilog.Expressions kullan�l�r.

JSON

{
  "Name": "Logger",
  "Args": {
    "configureLogger": {
      "Filter": [
        {
          "Name": "ByIncludingOnly",
          "Args": { "expression": "Contains(RenderedMessage, '�deme Ba�ar�s�z') or EventType = 'PaymentFailure'" }
        }
      ],
      "WriteTo": [
        {
          "Name": "BatchedFileSink", // Kendi �zel sink'imiz
          "Args": {
            "filePath": "logs/Acil_Hatalar.txt",
            "options": { "batchSizeLimit": 10, "period": "0.00:00:02" }
          }
        }
      ]
    }
  }
}
Microsoft Loglar� (Namespace'e G�re Filtreleme): SourceContext'i Microsoft. ile ba�layan loglar�, sadece Warning seviyesinden itibaren microsoft-logs.txt dosyas�na yazar.

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
      "MinimumLevel": "Warning", // Bu kural i�in seviye override
      "WriteTo": [
        {
          "Name": "File", // Standart File sink
          "Args": { "path": "logs/microsoft-logs.txt", "rollingInterval": "Day" }
        }
      ]
    }
  }
}
Uygulama Loglar� (Seq Hedefi): Bizim uygulamalar�m�z�n (PostaGuvercini.Logging, Logging.WebApiTest vb.) �retti�i (ama acil olmayan) loglar� Information seviyesinden itibaren Seq sunucusuna g�nderir.

JSON

{
  "Name": "Logger",
  "Args": {
    "configureLogger": {
      "Filter": [
        { // Acil loglar� hari� tut
          "Name": "ByExcluding", 
          "Args": { "expression": "Contains(RenderedMessage, '�deme Ba�ar�s�z') or EventType = 'PaymentFailure'" }
        },
        { // Bizim uygulamalar� dahil et
          "Name": "ByIncludingOnly", 
          "Args": { "expression": "StartsWith(SourceContext, 'Logging.WebApiTest') or StartsWith(SourceContext, 'PostaGuvercini.Logging')" } 
        }
      ],
      "MinimumLevel": "Information",
      "WriteTo": [ { "Name": "Seq", "Args": { "serverUrl": "http://localhost:5341" } } ]
    }
  }
}
Not: Kurallar WriteTo dizisindeki s�rayla i�lenir. Bir log, bir Logger blo�unun filtresine tak�l�rsa ve o blok logu bir hedefe yazarsa, genellikle sonraki Logger bloklar� taraf�ndan tekrar i�lenmez (e�er filtreler buna g�re ayarland�ysa).

Ortama �zel Yap�land�rma
ASP.NET Core'un standart yap�land�rma modeli sayesinde, farkl� ortamlar (Development, Staging, Production) i�in farkl� Serilog kurallar� tan�mlayabilirsiniz.

appsettings.Development.json: Geli�tirme s�ras�nda daha detayl� loglama (�rn: Debug seviyesi, Konsol ��kt�s� a��k, t�m loglar Seq'e) i�in ayarlar� buraya yaz�n.

appsettings.Production.json: Canl� ortam i�in optimize edilmi� ayarlar� (�rn: Information seviyesi, Konsol kapal�, sadece �nemli loglar Seq'e, g�r�lt�l� loglar ayr� dosyalara) buraya yaz�n.

Uygulama �al���rken, ilgili ortama ait .json dosyas�ndaki "Serilog" b�l�m�, ana appsettings.json'daki b�l�m�n �zerine yaz�l�r. Bu sayede, kod de�i�ikli�i yapmadan ortamlar aras�nda farkl� loglama davran��lar� elde edilebilir.

Kurulum: K�t�phanenin entegrasyonu i�in Program.cs dosyas�nda builder.Host.AddCustomLogging(lb => lb.UseSerilogAdapter()); �a�r�s�n�n yap�lm�� olmas� yeterlidir. T�m bu JSON yap�land�rmas� otomatik olarak okunacakt�r.