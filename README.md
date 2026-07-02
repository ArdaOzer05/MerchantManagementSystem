# Üye İşyeri Başvuru, Operasyon ve Yönetim Sistemi Entegrasyon Modülü

Bu dökümantasyon; kurumsal bankacılık süreçleri, ödeme sistemleri altyapıları ve üye işyeri (Merchant) operasyonel standartları dikkate alınarak geliştirilen merkezi yönetim modülünün teknik mimarisini, veritabanı ilişkilerini, iş akışlarını ve yerel ortam kurulum prosedürlerini içermektedir.

Sistem; banka ekosistemine dahil olmak isteyen ticari işletmelerin başvuru süreçlerini, POS donanım ve vPOS yazılım tahsis taleplerini, taraflar arasındaki ticari/finansal sözleşme şartlarını ve işletmelerin merkezi kredi risk skorlarını tek bir veri havuzundan konsolide etmek üzere tasarlanmıştır. Proje mimarisi, ölçeklenebilir, genişletilebilir ve test edilebilir katmanlı mimari (N-Tier Architecture) prensiplerine uygun olarak ASP.NET Core MVC çatısı altında yapılandırılmıştır.

---

## 1. Mimari Tasarım ve Sistem Katmanları

Sistem içerisindeki veri akışı, sorumlulukların ayrılması (Separation of Concerns) prensibine göre katmanlandırılmış ve her katmanın sınırları kesin kurallarla belirlenmiştir:

### Sunum Katmanı (Presentation Layer)
* **Teknolojik Altyapı:** Razor View Engine, HTML5, CSS3, JavaScript, Bootstrap 4, Chart.js.
* **İşlevi:** Kullanıcı etkileşimlerini karşılamak, form verilerini backend servislerine asenkron veya senkron istekler (HTTP POST/GET) olarak iletmek ve sunucudan dönen durum kodlarını kullanıcı arayüzünde nihai çıktılara dönüştürmek. Merkezi şablon yönetimi (`_Layout.cshtml`) sayesinde tüm arayüz bileşenleri tek bir çatıdan kontrol edilmektedir.

### İş Mantığı ve Kontrol Katmanı (Business & Controller Layer)
* **Teknolojik Altyapı:** .NET C# Denetleyicileri (Controllers), Aksiyon Filtreleri (Action Filters).
* **İşlevi:** İstemci tarafından gönderilen istekleri (Requests) işlemek, finansal hesaplama algoritmalarını (komisyon hakediş süreleri, bloke gün vadeleri, risk puanı eşikleri) çalıştırmak ve operasyonel iş akışlarını (Workflow) yönetmek. Veritabanı katmanı ile sunum katmanı arasında bir köprü görevi görerek doğrudan ham verinin arayüze sızmasını engeller.

### Veri Erişim ve Kalıcılık Katmanı (Data Access Layer)
* **Teknolojik Altyapı:** Microsoft SQL Server, Entity Framework Core, T-SQL (Transact-SQL).
* **İşlevi:** İlişkisel veritabanı şemasında tanımlanmış olan tabloların (Entities) yaşam döngüsünü yönetmek. CRUD (Create, Read, Update, Delete) işlemlerini optimize edilmiş sorgular vasıtasıyla yürütmek ve veri tutarlılığını (Data Integrity) veritabanı kısıtlamaları (Constraints, Foreign Keys) seviyesinde garanti altına almak.

---

## 2. Fonksiyonel Modül Detayları ve İş Süreçleri

Sistem, birbirleriyle gevşek bağlı (Loosely Coupled) ve ilişkisel veri tabanında yabancı anahtarlar (Foreign Keys) üzerinden konuşan 4 temel alt modülden oluşmaktadır:

### A. Üye İşyeri (Merchant) Kimlik ve Profil Yönetimi
Sisteme dahil olan her işletme, kurumsal bankacılık standartlarında bir Üye İşyeri Numarası (MID - Merchant ID) şemasına tabi tutulur. Modül kapsamında şu veriler ilişkisel şemada işlenir:
* **Yasal Veriler:** Resmi Şirket Unvanı, Ticari Sicil Gazetesi Kayıt Numarası, Bağlı Olunan Vergi Dairesi ve Vergi Numarası / T.C. Kimlik Numarası.
* **Operasyonel Veriler:** Şirket merkezi iletişim bilgileri, operasyonel adres tanımları, yetkili imza sirküleri sorumluları ve e-posta/telefon veri ağaçları.
* **Süreç Akışı:** Başvuru yapan bir işletmenin verileri sisteme girildiğinde statüsü otomatik olarak "İncelemede" olarak işaretlenir ve risk analizi tetiklenene kadar sistem havuzunda kilitlenir.

### B. POS ve vPOS Terminal Tahsis Yönetimi
Üye işyerlerinin fiziksel mağazalarında ya da e-ticaret altyapılarında kullanacakları ödeme alma terminallerinin yönetim döngüsüdür:
* **Fiziki POS Yönetimi:** Masaüstü POS, Mobil POS ve yeni nesil ÖKC (Ödeme Kaydedici Cihaz) yazar kasa donanım taleplerinin adet, marka ve model bazlı toplanması.
* **Sanal POS (vPOS) Yönetimi:** E-ticaret entegrasyonu için gerekli olan API anahtarları, SSL sertifika onay süreçleri ve 3D Secure yetkilendirme isteklerinin konfigürasyon takibi.
* **Teknik Karşılık:** Her onaylanan talep için veritabanında üye işyerine bağlı bir Terminal ID (TID) üretilir ve terminalin durumu (Aktif, Pasif, Blokeli) anlık olarak güncellenir.

### C. Finansal Çalışma Şartları ve Komisyon Matrisi
Banka ile üye işyeri arasında imzalanan ticari sözleşmelerin parametrik olarak sisteme geçirilmesini ve hakediş hesaplamalarını yöneten kritik modüldür:
* **Komisyon Oranları:** Sektör bazlı, ciro endeksli veya kart markası (VakıfBank World, diğer bankalar vb.) özelinde dinamik komisyon baremlerinin tanımlanması.
* **Valör ve Bloke Gün Yönetimi:** Üye işyerinin gerçekleştirdiği işlemlerin üye işyeri hesabına kaç gün sonra (örneğin; Ertesi Gün Geçişi veya 30 Gün Bloke Standartı) aktarılacağının finansal matrisler üzerinden yönetimi.
* **Sözleşme Takibi:** Geçerlilik tarihi dolan veya revizyon gerektiren ticari şartların otomatik olarak uyarı mekanizmasına bildirilmesi.

### D. Kredibilite ve Risk İstihbarat Modülü
Başvuru sürecindeki risklerin minimize edilmesi amacıyla, harici bankacılık ve istihbarat sistemlerinin iş kurallarını simüle eden karar destek mekanizmasıdır:
* **Risk Skorlaması:** İşletmelerin geçmiş ticari performansları, protestolu senet/çek geçmişleri ve merkezi bankacılık risk puanları (Risk Score) üzerinden bir istatistik tablosu tutulur.
* **Karar Ağacı:** Belirlenen kritik eşik değerinin (Threshold) altında kalan başvurular otomatik olarak "Reddedildi" veya "Ek Revizyon Bekliyor" statüsüne çekilerek operasyon ekiplerinin paneline düşürülür.

---

## 3. İleri Düzey Teknik Uygulamalar ve Altyapı Geliştirmeleri

Projenin kararlılığını, güvenliğini ve kullanıcı deneyimini artırmak amacıyla arka planda uygulanan mühendislik çözümleri:

### Merkezi Durum ve Bildirim Pipeline'ı (State Management)
* **Problem:** HTTP protokolünün durumsuz (Stateless) yapısı ve MVC mimarisindeki sayfa yönlendirmeleri (RedirectToAction) esnasında, kullanıcının yaptığı işlemlerin sonuçlarının (Başarı, Hata, Sistem Kısıtı) bir sonraki sayfaya güvenli ve kayıpsız aktarılamaması.
* **Çözüm:** Sistemin ana iskeletini oluşturan ana şablon (`_Layout.cshtml`) içerisine entegre edilmiş, arka plandaki Controller aksiyonlarından fırlatılan `TempData` ve `ViewBag` sözlük nesnelerini asenkron olarak dinleyen bir bildirim yakalama katmanı kurulmuştur.
* **Uygulama:** Veritabanına yazılan bir kaydın ardından tetiklenen HTTP 302 (Redirect) yönlendirmesinde, veri kaybı yaşanmadan durum kodları çözümlenir ve tarayıcı ekranında kullanıcıyı rahatsız etmeyecek şekilde Bootstrap alert bileşenleri aracılığıyla dinamik mesajlar basılır.

### Çift Aşamalı Veri Doğrulama Mimarisi (Data Validation)
* **İstemci Taraflı (Client-Side) Doğrulama:** Form girişlerinde kullanıcı deneyimini maksimumda tutmak ve sunucuya gereksiz geçersiz istek (Invalid Request) yükü bindirmemek adına, tarayıcı seviyesinde JavaScript ve regex desenleri (Regular Expressions) kullanılarak format denetimleri yapılmıştır. (Örn: Vergi numarası karakter sınırı, e-posta şablon doğrulaması).
* **Sunucu Taraflı (Server-Side) Doğrulama:** Güvenlik amacıyla istemci tarafındaki doğrulamaların bypass edilme ihtimaline karşı, Controller katmanında `ModelState.IsValid` kontrolleri ve backend validasyon zincirleri işletilir. Hatalı veri tespit edildiğinde istek veritabanı katmanına ulaşmadan kesilir, hata detayları state korunarak arayüze geri basılır.

### Analitik Yönetici Dashboard Paneli
* **Veri Konsolidasyonu:** Sistem mimarisindeki dağınık ilişkisel tablolar, JOIN sorguları ve EF Core LINQ optimizasyonları ile tek bir analitik modele indirgenmiştir.
* **Görselleştirme Pipelines'ı:** Toplanan başvuru havuzu verileri, durum parametrelerine göre gruplanarak **Chart.js** kütüphanesinin anlayacağı veri dizilerine (Arrays) dönüştürülür. Arayüzde yer alan dinamik grafikler sayesinde operasyon yöneticileri sistemin genel yükünü ve onay/ret oranlarını anlık olarak izleyebilir.

---

## 4. Kullanılan Teknolojik Yığın (Tech Stack)

* **Çekirdek Çatı (Framework):** ASP.NET Core MVC (.NET 8.0 / C#)
* **Veri Kalıcılık Katmanı (ORM):** Entity Framework Core & LINQ Queries
* **Veritabanı Yönetim Sistemi:** Microsoft SQL Server (MS SQL) & T-SQL Compiler
* **Arayüz Teknolojileri (Frontend):** HTML5, CSS3, JavaScript (ES6+), Bootstrap 4 Framework
* **Grafik ve Veri Analitiği:** Chart.js Library

---

## 5. Yerel Geliştirme Ortamı Kurulumu (Setup Instructions)

Projenin yerel bir geliştirme makinesinde derlenmesi, çalıştırılması ve test edilmesi için aşağıdaki adımların sırasıyla uygulanması gerekmektedir:

### Ön Koşullar (Prerequisites)
* .NET SDK (Version 8.0 veya üzeri)
* Visual Studio 2022 (ASP.NET ve web geliştirme iş yükü yüklenmiş olmalıdır)
* Microsoft SQL Server (LocalDB veya Standalone Express instance)

### Kurulum Adımları
1. **Kaynak Kodların Klonlanması:**
   Terminal veya komut satırını açarak projeyi yerel dizininize indirin:
   ```bash
   git clone [https://github.com/ArdaOzer05/MerchantManagementSystem.git](https://github.com/ArdaOzer05/MerchantManagementSystem.git)

2. Güvenli Konfigürasyon Dosyasının Hazırlanması:
Kurumsal güvenlik standartları gereği hassas veritabanı şifrelerini barındıran gerçek appsettings.json dosyası repoya dahil edilmemiştir. Ana dizinde bulunan güvenli şablon dosyasının (appsettings.Example.json) adını appsettings.json olarak değiştirin.

3. Bağlantı Metninin (Connection String) Düzenlenmesi:
Yeni oluşturduğunuz appsettings.json dosyasını bir metin editörüyle açın ve ConnectionStrings altındaki DefaultConnection değerini kendi yerel SQL Server altyapınıza uygun şekilde güncelleyin:

"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_LOCAL_SERVER_NAME;Database=MerchantManagementDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;"
}


4. Çalıştırma ve Dağıtım:
Visual Studio 2022 üzerinden UyelyeriBasvuru.slnx çözüm dosyasını açın. Bağımlılıkların (NuGet paketleri) otomatik olarak yüklenmesini bekleyin. Ardından Ctrl + F5 veya Build -> Run komutu ile projeyi derleyerek yerel web sunucusu (Kestrel / IIS Express) üzerinde tetikleyin. Sistem, yapılandırma mimarisine uygun olarak çalışacaktır.
