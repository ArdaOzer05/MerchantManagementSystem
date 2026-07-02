# VakifBank World Üye İşyeri Başvuru ve Yönetim Modülü

Bu proje; bankacılık ve ödeme sistemleri standartlarına uygun olarak tasarlanmış, üye işyeri (Merchant) başvuru süreçlerini, POS/vPOS donanım taleplerini ve finansal risk analizlerini tek bir merkezden yönetmeyi amaçlayan kurumsal bir CRM ve entegrasyon modülüdür. Proje, VakıfBank Ödeme Sistemleri Uygulama Geliştirme ekosistemindeki iş akışları ve veri modelleri (Merchant ID, POS terminal yönetim yapıları, komisyon şemaları) simüle edilerek, katmanlı mimari prensiplerine uygun olarak geliştirilmiştir.

---

## Mimari Yapı ve Teknik Özellikler

### 1. Merkezi Durum ve Bildirim Yönetimi (State Management)
* **Mesaj İşleme Mekanizması:** Sistemin ana iskeletini oluşturan _Layout.cshtml şablonu üzerinde, asenkron süreçlerden dönen sonuçları yakalayan merkezi bir bildirim yapısı kurulmuştur.
* **Veri Taşıma ve Takip:** TempData ve ViewBag nesneleri üzerinden taşınan işlem sonuçları (başarı, hata, yetki yetersizliği vb.), HTTP yönlendirmelerinde (Redirect) veri kaybı yaşanmadan dinamik Bootstrap alert bileşenlerine dönüştürülerek kullanıcıya sunulur.

### 2. Gelişmiş Veri Doğrulama (Data Validation)
* **Veri Tutarlılığı:** Üye işyeri kayıt ve güncelleme ekranlarında, veritabanı katmanına hatalı istek (Request) gönderilmesini engellemek amacıyla sunucu (Server-Side) ve istemci (Client-Side) taraflı validasyon zinciri oluşturulmuştur.
* **Format Kontrolleri:** Vergi numarası, T.C. Kimlik Numarası, telefon ve e-posta gibi alanlar için veri formatı denetimleri entegre edilmiş; eksik veya hatalı girişlerde kullanıcıyı bloke eden ve hata detaylarını gösteren dinamik uyarı yapısı kurulmuştur.

### 3. Veri Görselleştirme ve İzleme Paneli (Dashboard)
* **Chart.js Entegrasyonu:** Sistemdeki başvuru havuzunun durum dağılımı (Onaylanan, Reddedilen, İncelemede), Chart.js kütüphanesi kullanılarak optimize edilmiş grafikler üzerinden gerçek zamanlı olarak izlenebilmektedir.
* **Metrik Konsolidasyonu:** POS talepleri, bekleyen revizyonlar ve finansal limitler üst veri (Metadata) kartları halinde panelin üst kısmında konsolide edilmiştir.

---

## Modül Detayları

| Modül Adı | İşlevi / Teknik Karşılığı |
| :--- | :--- |
| **Üye İşyeri Yönetimi** | Firma unvanı, vergi dairesi, ticari sicil no ve operasyonel adres verilerinin ilişkisel şemada tutulması. |
| **POS & vPOS Talepleri** | Fiziki terminal (Masaüstü, Mobil POS) veya Sanal POS yetkilendirme isteklerinin durum bazlı takibi. |
| **Çalışma Şartları** | Banka ve üye işyeri arasındaki komisyon oranları, bloke gün süreleri ve hakediş hesaplama kurallarının yönetimi. |
| **Kredibilite & İstihbarat** | Başvuru yapan işletmelerin merkezi risk/istatistik puanlarının izlenmesi ve onay sürecine girdi oluşturması. |

---

## Kullanılan Teknolojik Yığın (Tech Stack)

* **Framework:** ASP.NET Core MVC (C#)
* **Veritabanı Katmanı:** Microsoft SQL Server & Entity Framework Core / T-SQL
* **Arayüz Teknolojileri:** Bootstrap 4, JavaScript, HTML5, CSS3
* **İstatistik & Grafik:** Chart.js

---

## Yerel Ortamda Kurulum (Setup)

1. Projeyi yerel bilgisayarınıza klonlayın:
   ```bash
   git clone [https://github.com/ArdaOzer05/MerchantManagementSystem.git](https://github.com/ArdaOzer05/MerchantManagementSystem.git)


2. Ana dizinde bulunan ve güvenli şablon olarak bırakılan appsettings.Example.json dosyasının adını appsettings.json olarak güncelleyin.

3. appsettings.json içerisindeki DefaultConnection string değerini kendi yerel SQL Server örneğinize (Instance) göre düzenleyin.

4. Visual Studio üzerinden projeyi derleyerek yerel sunucu (Kestrel / IIS Express) üzerinde çalıştırabilirsiniz.
