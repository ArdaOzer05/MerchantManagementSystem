# VakifBank Uye Isyeri Basvuru ve Yonetim Sistemi

Bu proje, VakıfBank Ödeme Sistemleri Uygulama Geliştirme birimi bünyesinde, üye işyeri (Merchant) başvuru operasyonlarını dijitalleştirmek, POS ve vPOS tahsis süreçlerini otomatize etmek amacıyla geliştirilmiş kurumsal bir yönetim sistemidir. İşletmelerin kurumsal, finansal ve risk verilerini merkezi bir ilişkisel veritabanında konsolide ederek banka personelinin değerlendirme süreçlerini optimize eder.

---

## Proje Mimarisi ve Katman Yapısı

Proje, kurumsal yazılım geliştirme pratiklerine uygun olarak katmanlı mimari (N-Tier Architecture) ve sorumlulukların ayrılması (Separation of Concerns) prensipleri doğrultusunda ASP.NET Core MVC çatısı altında yapılandırılmıştır.

### Sunum Katmanı (Presentation Layer)
* **Teknoloji Altyapısı:** ASP.NET Core Razor Views, Bootstrap 4, JavaScript (ES6+), Chart.js
* **Mimari İşlevi:** Kullanıcı etkileşimlerini ve form verilerini güvenli HTTP istekleri (GET/POST) ile kontrolcü katmanına iletir. Merkezi şablon yönetimi (`_Layout.cshtml`) kullanılarak tüm arayüz bileşenlerinin standartlara uygun, tutarlı bir hiyerarşide sunulması sağlanmıştır.

### İş Mantığı ve Denetim Katmanı (Business & Controller Layer)
* **Teknoloji Altyapısı:** .NET C# Controllers, Action Filters, DataAnnotations
* **Mimari İşlevi:** İstemci tarafında üretilen istekleri (Requests) iş kurallarına tabi tutar. Finansal hesaplama algoritmalarını (komisyon oranları, bloke gün vadeleri) yürütür ve durum geçişlerini kontrol ederek ham verinin doğrudan arayüze sızmasını engeller.

### Veri Erişim Katmanı (Data Access Layer)
* **Teknoloji Altyapısı:** Microsoft SQL Server (MS SQL), Entity Framework Core, T-SQL
* **Mimari İşlevi:** `UyeIsyeriDB` ilişkisel veritabanı şeması üzerindeki CRUD (Create, Read, Update, Delete) operasyonlarını yönetir. Tablolar arası ilişkileri (Primary/Foreign Key) ve veri bütünlüğünü veritabanı kısıtlamaları (Constraints) seviyesinde korur.

---

## Veritabanı Tasarımı (UyeIsyeriDB)

Sistem mimarisi, normalizasyon kurallarına uygun olarak tasarlanmış `UyeIsyeriDB` veritabanı üzerinde çalışmaktadır. Tablolar arası ilişkisel yapı, veri kaybını ve çoklamayı önleyecek şekilde foreign key kısıtlamalarıyla türetilmiştir.

### Veritabanı Tablo Yapıları ve İlişkileri
* **Merchants (Üye İşyerleri):** Ticari Unvan, Vergi Dairesi, Vergi Numarası / TCKN, Sektör Kodu (MCC), İletişim ve Adres verilerini saklar.
* **POSRequests (POS Talepleri):** Üye işyerine bağlı donanım (Masaüstü, Mobil POS) veya yazılım (vPOS) isteklerini, benzersiz Terminal ID (TID) ve Üye İşyeri Numarası (MID) simülasyonu ile eşleştirir.
* **CommercialTerms (Çalışma Şartları):** Banka ile işletme arasındaki komisyon oranlarını, valör sürelerini ve hesap kesim döngülerini parametrik olarak tutar.
* **RiskIntelligence (Kredibilite & İstihbarat):** İşletmelerin merkezi risk skorlarını barındırır ve başvuru onay mekanizmasına veri girdisi sağlar.

---

## Uçtan Uca Süreç Akışı (Business Workflow)

1. **Başvuru ve Ön Doğrulama:** İşletme kurumsal verilerini sisteme girer. Sistem, istemci ve sunucu taraflı validasyon zincirlerini tetikleyerek vergi numarası ve format kontrollerini gerçekleştirir.
2. **Operasyon Havuzuna Aktarım:** Doğrulamadan geçen başvurular veri tabanına kaydedilir ve durumu otomatik olarak "İncelemede" statüsüne çekilerek banka personeli paneline düşer.
3. **Risk ve Komisyon Değerlendirmesi:** Banka personeli, işletmenin risk skorlarını ve talep ettiği POS donanım şartlarını inceler. Komisyon ve bloke gün tanımlamalarını yaparak başvuruyu karara bağlar.
4. **Aktivasyon ve Loglama:** Onaylanan başvurular için Terminal ID üretilir. Başvurunun geçtiği tüm aşamalar, tarih ve işlem yapan kullanıcı bilgisiyle birlikte veritabanında denetim (audit) amacıyla loglanır.

---

## Öne Çıkan Teknik Geliştirmeler

### Merkezi Durum Yönetimi ve Bildirim Pipeline'ı
* **Problem Tanımı:** HTTP protokolünün stateless yapısı nedeniyle, sayfa yönlendirmelerinde (RedirectToAction) işlem sonuçlarının (başarı, hata, validasyon kaybı) veri kaybı yaşanmadan bir sonraki sayfaya aktarılamaması.
* **Çözüm Mekanizması:** `_Layout.cshtml` mimarisine entegre edilen ve backend katmanından fırlatılan `TempData` ile `ViewBag` nesnelerini asenkron dinleyen bir pipeline kurulmuştur. Sayfa yönlendirilse dahi işlem durumu çözümlenerek Bootstrap alert bileşenleri üzerinden kullanıcıya dinamik olarak gösterilir.

### Çift Aşamalı Veri Doğrulama (Data Validation)
* **Client-Side:** Kullanıcı deneyimini optimize etmek ve sunucuya geçersiz istek yükü bindirmemek adına regex desenleri ile tarayıcı seviyesinde format denetimi sağlanmıştır.
* **Server-Side:** Güvenlik amacıyla `ModelState.IsValid` kontrolleri uygulanmış, geçersiz istekler veritabanı katmanına ulaşmadan kesilerek hata detayları arayüze geri basılmıştır.

---

## Ekran Görüntüleri ve Modül Detayları

Sistem, üye işyeri başvuru, doğrulama, finansal parametre yönetimi ve operasyonel takip süreçlerini uçtan uca yöneten 12 temel modülden oluşmaktadır.

### 1. Ana Panel (Dashboard)
Sistemdeki toplam başvuru hacminin, onay/red oranlarının, POS türü dağılımlarının (Fiziki/Sanal) ve illere göre başvuru yoğunluklarının Chart.js grafik kütüphanesiyle dinamik olarak görselleştirildiği analitik izleme ekranıdır.

<img width="1895" height="917" alt="Ana Panel" src="https://github.com/user-attachments/assets/74509bd8-45e0-41c1-b7c3-7795bed441a7" />

### 2. Başvuru Listesi (CRM)
Banka operasyon ekibinin merkezi çalışma havuzudur. Sisteme düşen tüm talepleri benzersiz başvuru numaralarıyla listeler; istihbarat temizlik durumunu, güncel değerlendirme statüsünü ("İncelemede", "Onaylandı") gösterir ve detaylı inceleme aksiyonlarını tetikler.

<img width="1919" height="913" alt="Başvuru Listesi (CRM)" src="https://github.com/user-attachments/assets/162e286c-8c7c-4b8b-8cb5-ddb288318b1d" />

### 3. Başvuru Giriş
Yeni bir üye işyeri süreci başlatmak için kullanılan temel başvuru kayıt ekranıdır. Talep numarası, müşteri tipi (Tüzel, Şahıs, Yabancı), başvuru tarihi ve statü yönetimi bu modül üzerinden asenkron doğrulama protokolleriyle gerçekleştirilir.

<img width="1919" height="911" alt="Başvuru Giriş" src="https://github.com/user-attachments/assets/382fdde5-35a0-4acc-97eb-2e14e027a304" />

### 4. Firma Bilgi Giriş
İşletmelerin kurumsal kimlik verilerinin toplandığı alandır. Firma Ticari Ünvanı, Vergi Numarası, Vergi Dairesi, MERSİS Numarası, faaliyet konusu, iletişim bilgileri ve açık adres verileri çift katmanlı validasyon kontrollerine tabi tutularak bu ekrandan veritabanına işlenir.

<img width="1891" height="858" alt="Firma Bilgi Giriş" src="https://github.com/user-attachments/assets/8388f0ad-4abd-4635-8df2-7b9255c9d271" />

### 5. Yetkili Giriş
Başvuruda bulunan firmanın yasal temsilcilerine ait verilerin (Ad, Soyad, T.C. Kimlik No, Cep Telefonu, E-Posta ve Şirket İçi Yetki Tipi) ilişkisel olarak kaydedildiği, MERSİS ve kimlik formatı doğrulama kurallarının işletildiği modüldür.

<img width="1913" height="909" alt="Yetkili Giriş" src="https://github.com/user-attachments/assets/5942ae5f-09f0-409b-b4bb-87d63937e80c" />

### 6. POS Talep Giriş
Firmaların ticari faaliyetlerinde kullanacakları donanım ve altyapı planlama formudur. Talep edilen POS Tipi (Mobil, Masaüstü, Sanal), cihaz adeti, cihaz markası, modeli ve fiziki kurulum/entegrasyon adres bilgileri bu ekrandan toplanır.

<img width="1909" height="912" alt="POS Talep Giriş" src="https://github.com/user-attachments/assets/55909b5f-5aab-419d-8e4f-0ceefd8005fc" />

### 7. Banka Bilgi Giriş
Üye işyerinin hak ediş ödemelerinin ve finansal takas süreçlerinin yürütüleceği banka hesap tanımlama ekranıdır. Banka adı, IBAN numarası, hesap numarası ve hesap sahibi bilgileri uluslararası IBAN standartlarına uygun şekilde doğrulanarak kaydedilir.

<img width="1911" height="912" alt="Banka Bilgi Giriş" src="https://github.com/user-attachments/assets/c3f0541b-1a55-4891-a950-563057c48995" />

### 8. Doküman Giriş
Kurumsal üye işyeri başvurusu için zorunlu olan yasal evrakların (Vergi Levhası, İmza Sirküleri, Ticaret Sicil Gazetesi vb.) dijitalleştirildiği alandır. Doküman tipi seçimi, dosya adı eşleştirmesi ve güvenli dosya depolama yolu (Upload Path) yönetimi sağlanır.

<img width="1919" height="914" alt="Doküman Giriş" src="https://github.com/user-attachments/assets/6e5176ee-40aa-479f-81c8-82b09f705037" />

### 9. Durum Log Giriş (Audit Trail)
Başvurunun geçtiği tüm aşamaların (Örn: İncelemede → Onaylandı) geçmişe dönük olarak kayıt altına alındığı denetim izi modülüdür. Durum değişiklik gerekçeleri, işlemi gerçekleştiren personel bilgisi ve işlem zamanı geri döndürülemez şekilde veritabanında loglanır.

<img width="1893" height="920" alt="Durum Log Giriş" src="https://github.com/user-attachments/assets/e34b0f08-5538-4a7f-9cce-aef6847796e4" />

### 10. POS İşlemleri
Onaylanan başvurular için spesifik parametre ve özellik yapılandırmalarının yapıldığı entegrasyon ekranıdır. Ödeme Kaydedici Cihaz (ÖKC) entegrasyonu, dövizli işlem yetkileri, banka tarım/üye kart kampanyaları ve ortak POS kullanımı gibi ileri düzey bankacılık parametreleri set edilerek VakıfBank çekirdek sistemine iletilir.

<img width="1919" height="904" alt="POS İşlemleri" src="https://github.com/user-attachments/assets/350e22dc-0bf7-4710-a708-244c12665010" />

### 11. Çalışma Şartı İşlemleri
Banka ile üye işyeri arasında uygulanacak finansal modellerin tanımlandığı modüldür. Sektörel MCC kodlarına göre komisyon oranları, bloke gün vadeleri, hesap kesim döngüleri ve parametrik fiyatlandırma kuralları bu ekran üzerinden yönetilir.

<img width="1918" height="913" alt="Çalışma Şartı İşlemleri" src="https://github.com/user-attachments/assets/731a476d-106a-4498-844c-3b077b9f0d69" />

### 12. Kredibilite İşlemleri
Başvuru sahibi firmanın finansal istihbarat ve risk skorlarının konsolide edildiği modüldür. Merkezi risk yönetim sistemlerinden gelen veriler doğrultusunda firmanın kredibilite notu analiz edilir ve otomatik onay/red karar mekanizmasına veri girdisi sağlar.

<img width="1919" height="916" alt="Kredibilite İşlemleri" src="https://github.com/user-attachments/assets/5e8cd44e-1786-42e7-ab89-5d4879fcca9c" />

---

## Yerel Ortamda Kurulum (Setup Instructions)

### Ön Koşullar
* .NET SDK 8.0 veya üzeri
* Microsoft SQL Server (LocalDB / Express)
* Visual Studio 2022

### Kurulum Adımları

1. Projeyi yerel dizine klonlayın:
   ```bash
   git clone [https://github.com/ArdaOzer05/MerchantManagementSystem.git](https://github.com/ArdaOzer05/MerchantManagementSystem.git)

2. Kök dizinde yer alan appsettings.Example.json dosyasının adını appsettings.json olarak güncelleyin.
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=UyeIsyeriDB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;"
}

3. Visual Studio üzerinden UyelyeriBasvuru.slnx çözüm dosyasını açın, bağımlılıkların yüklenmesinin ardından projeyi derleyerek yerel sunucu (Kestrel / IIS Express) üzerinde çalıştırın.
