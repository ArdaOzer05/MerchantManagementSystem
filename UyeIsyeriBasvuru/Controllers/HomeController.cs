using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UyeIsyeriBasvuru.Data;
using UyeIsyeriBasvuru.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace UyeIsyeriBasvuru.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        #region DASHBOARD (INDEX)
        public IActionResult Index()
        {
            var sonAltiAy = DateTime.Now.AddMonths(-6);

            var basvuruVeri = _context.Basvurular
                .Where(x => x.BasvuruTarihi >= sonAltiAy)
                .GroupBy(x => new { x.BasvuruTarihi.Year, x.BasvuruTarihi.Month })
                .Select(g => new { g.Key.Year, g.Key.Month, Sayi = g.Count() })
                .AsEnumerable()
                .Select(g => new { Tarih = new DateTime(g.Year, g.Month, 1), Sayi = g.Sayi })
                .ToList();

            var grafikSonuc = new List<object>();
            for (int i = 6; i >= 0; i--)
            {
                var ay = DateTime.Now.AddMonths(-i);
                var baslangic = new DateTime(ay.Year, ay.Month, 1);
                var veri = basvuruVeri.FirstOrDefault(x => x.Tarih == baslangic);

                grafikSonuc.Add(new
                {
                    Tarih = baslangic.ToString("MM/yyyy"),
                    Sayi = veri != null ? veri.Sayi : 0
                });
            }

            ViewBag.BasvuruLabels = JsonConvert.SerializeObject(grafikSonuc.Select(x => ((dynamic)x).Tarih));
            ViewBag.BasvuruData = JsonConvert.SerializeObject(grafikSonuc.Select(x => ((dynamic)x).Sayi));

            var tumPoslar = _context.PosIslemleri.ToList();
            var fiziksel = tumPoslar.Where(x => x.PosTipi == "Fiziki POS").Sum(x => x.MasaustuPosAdet + x.MobilPosGsmAdet);
            var sanal = tumPoslar.Where(x => x.PosTipi == "Sanal POS").Sum(x => x.MasaustuPosAdet + x.MobilPosGsmAdet);

            ViewBag.FizikselPos = fiziksel;
            ViewBag.SanalPos = sanal;

            var ilVeri = _context.FirmaBilgileri
                .Where(x => !string.IsNullOrEmpty(x.Il) && x.Il.Trim() != "")
                .GroupBy(x => x.Il.Trim())
                .Select(g => new { Il = g.Key, Sayi = g.Count() })
                .ToList();

            ViewBag.IlLabels = JsonConvert.SerializeObject(ilVeri.Select(x => x.Il));
            ViewBag.IlData = JsonConvert.SerializeObject(ilVeri.Select(x => x.Sayi));

            return View();
        }
        #endregion

        private object GetBasvuruSelectList() => _context.Basvurular
            .Select(x => new { x.BasvuruId, Aciklama = $"ID:{x.BasvuruId} | No:{x.BasvuruNO}" }).ToList();

        /// <summary>
        /// EF Core'un arkada üretebileceği gölge (Shadow) foreign key kolonlarını otomatik dolduran güvenlik mekanizması.
        /// </summary>
        private void ShadowKeyKorumaEkle(object model, long basvuruId)
        {
            var entry = _context.Entry(model);
            var golgeAlanlar = entry.Properties
                .Where(p => p.Metadata.Name.StartsWith("BasvuruId") && p.Metadata.Name != "BasvuruId")
                .ToList();

            foreach (var alan in golgeAlanlar)
            {
                alan.CurrentValue = basvuruId;
            }
        }

        #region CRM BAŞVURU SÜREÇLERİ (LİSTE, DETAY, SİL)

        public IActionResult BasvuruListesi()
        {
            var list = _context.Basvurular
                .Include(x => x.FirmaBilgileri)
                .Include(x => x.KredibiliteIslemleri)
                .OrderByDescending(x => x.BasvuruId)
                .ToList();

            return View(list);
        }

        public IActionResult BasvuruDetay(int id)
        {
            // EKSİKLER DÜZELTİLDİ: Tüm alt tablolar, dokümanlar ve her iki POS tablosu da Include edildi.
            var basvuru = _context.Basvurular
                .Include(x => x.FirmaBilgileri)
                .Include(x => x.Yetkililer)
                .Include(x => x.BankaBilgileri)
                .Include(x => x.PosIslemleri)      // Yeni eklendi
                .Include(x => x.PosTalepleri)
                .Include(x => x.Dokumanlar)        // Yeni eklendi (Doküman listeleme sorunu çözümü)
                .Include(x => x.CalismaSartiIslemleri) // Yeni eklendi
                .Include(x => x.KredibiliteIslemleri)
                .FirstOrDefault(x => x.BasvuruId == id);

            if (basvuru == null) return NotFound();

            ViewBag.FirmaBilgisi = basvuru.FirmaBilgileri?.FirstOrDefault();
            ViewBag.YetkiliBilgisi = basvuru.Yetkililer?.FirstOrDefault();
            ViewBag.BankaBilgisi = basvuru.BankaBilgileri?.FirstOrDefault();

            // POS verisinin yanlış gelme ihtimaline karşı ikisini de güvenli bir şekilde kontrol edip bağlıyoruz.
            ViewBag.PosBilgisi = (object)basvuru.PosIslemleri?.FirstOrDefault() ?? basvuru.PosTalepleri?.FirstOrDefault();

            ViewBag.CalismaSartiBilgisi = basvuru.CalismaSartiIslemleri?.FirstOrDefault();
            ViewBag.KredibiliteBilgisi = basvuru.KredibiliteIslemleri?.FirstOrDefault();

            // Detay ekranındaki döngü için dokümanları ViewBag'e aktarıyoruz.
            ViewBag.Dokumanlar = basvuru.Dokumanlar?.ToList() ?? new List<UyeIsyeriDokuman>();

            return View(basvuru);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult BasvuruSil(long id)
        {
            try
            {
                var basvuru = _context.Basvurular.FirstOrDefault(x => x.BasvuruId == id);
                if (basvuru != null)
                {
                    _context.Basvurular.Remove(basvuru);
                    _context.SaveChanges();
                    TempData["SuccessMessage"] = "Başvuru ve bağlı tüm istihbarat, POS, firma verileri güvenli bir şekilde silindi.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Silinmek istenen başvuru kaydı bulunamadı.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Mali veri bütünlüğü koruma hatası: " + ex.Message;
            }
            return RedirectToAction("BasvuruListesi");
        }
        #endregion

        #region POST METODLARI
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UyeIsyeriBasvuru(UyeIsyeriBasvuruModel model)
        {
            ModelState.Remove("FirmaBilgileri");
            ModelState.Remove("Yetkililer");
            ModelState.Remove("BankaBilgileri");
            ModelState.Remove("PosIslemleri");
            ModelState.Remove("PosTalepleri");
            ModelState.Remove("Dokumanlar");
            ModelState.Remove("DurumLoglari");
            ModelState.Remove("CalismaSartiIslemleri");
            ModelState.Remove("KredibiliteIslemleri");

            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Lütfen formdaki zorunlu alanları kontrol ediniz.";
                return View(model);
            }
            try
            {
                _context.Basvurular.Add(model);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Başvuru başarıyla oluşturuldu.";
                return RedirectToAction("UyeIsyeriBasvuru");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Başvuru kaydedilemedi: " + ex.Message;
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UyeIsyeriFirmaBilgi(UyeIsyeriFirmaBilgi model)
        {
            ModelState.Remove("Basvuru");
            ModelState.Remove("BasvuruId1");

            if (!ModelState.IsValid)
            {
                ViewBag.Basvurular = GetBasvuruSelectList();
                TempData["ErrorMessage"] = "Form verileri geçerli değil, lütfen alanları kontrol edin.";
                return View(model);
            }
            try
            {
                _context.FirmaBilgileri.Add(model);
                ShadowKeyKorumaEkle(model, model.BasvuruId);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Firma bilgisi başarıyla kaydedildi.";
                return RedirectToAction("UyeIsyeriFirmaBilgi");
            }
            catch (Exception ex)
            {
                ViewBag.Basvurular = GetBasvuruSelectList();
                var innerMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                TempData["ErrorMessage"] = "Veritabanı Hatası: " + innerMessage;
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UyeIsyeriYetkili(UyeIsyeriYetkili model)
        {
            ModelState.Remove("Basvuru");
            ModelState.Remove("BasvuruId1");

            if (!ModelState.IsValid)
            {
                ViewBag.Basvurular = GetBasvuruSelectList();
                TempData["ErrorMessage"] = "Form verileri geçerli değil, lütfen alanları kontrol edin.";
                return View(model);
            }
            try
            {
                _context.Yetkililer.Add(model);
                ShadowKeyKorumaEkle(model, model.BasvuruId);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Yetkili bilgileri sisteme işlendi.";
                return RedirectToAction("UyeIsyeriYetkili");
            }
            catch (Exception ex)
            {
                ViewBag.Basvurular = GetBasvuruSelectList();
                var innerMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                TempData["ErrorMessage"] = "Veritabanı Hatası: " + innerMessage;
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UyeIsyeriBankaBilgi(UyeIsyeriBankaBilgi model)
        {
            if (!string.IsNullOrEmpty(model.Iban))
            {
                model.Iban = model.Iban.Replace(" ", "").Trim();
            }

            ModelState.Remove("Basvuru");
            ModelState.Remove("BasvuruId1");
            ModelState.Remove("Iban");

            if (!ModelState.IsValid)
            {
                ViewBag.Basvurular = GetBasvuruSelectList();
                TempData["ErrorMessage"] = "Form verileri geçerli değil, lütfen alanları kontrol edin.";
                return View(model);
            }
            try
            {
                _context.BankaBilgileri.Add(model);
                ShadowKeyKorumaEkle(model, model.BasvuruId);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Banka hesap bilgileri kaydedildi.";
                return RedirectToAction("UyeIsyeriBankaBilgi");
            }
            catch (Exception ex)
            {
                ViewBag.Basvurular = GetBasvuruSelectList();
                var innerMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                TempData["ErrorMessage"] = "Veritabanı Hatası: " + innerMessage;
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UyeIsyeriPosBasvuru(UyeIsyeriPosBasvuru model)
        {
            ModelState.Remove("Basvuru");
            ModelState.Remove("BasvuruId1");

            if (!ModelState.IsValid)
            {
                ViewBag.Basvurular = GetBasvuruSelectList();
                TempData["ErrorMessage"] = "Form verileri geçerli değil, lütfen alanları kontrol edin.";
                return View(model);
            }
            try
            {
                _context.PosIslemleri.Add(model);
                ShadowKeyKorumaEkle(model, model.BasvuruId);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Kurumsal POS Başvuru parametreleri başarıyla kaydedildi.";
                return RedirectToAction("UyeIsyeriPosBasvuru");
            }
            catch (Exception ex)
            {
                ViewBag.Basvurular = GetBasvuruSelectList();
                var innerMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                TempData["ErrorMessage"] = "Veritabanı Hatası: " + innerMessage;
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UyeIsyeriPosTalep(UyeIsyeriPosTalep model)
        {
            ModelState.Remove("Basvuru");
            ModelState.Remove("BasvuruId1");

            if (!ModelState.IsValid)
            {
                ViewBag.Basvurular = GetBasvuruSelectList();
                TempData["ErrorMessage"] = "Form verileri geçerli değil, lütfen alanları kontrol edin.";
                return View(model);
            }
            try
            {
                _context.PosTalepleri.Add(model);
                ShadowKeyKorumaEkle(model, model.BasvuruId);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "POS Cihaz/Envanter talebi başarıyla eklendi.";
                return RedirectToAction("UyeIsyeriPosTalep");
            }
            catch (Exception ex)
            {
                ViewBag.Basvurular = GetBasvuruSelectList();
                var innerMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                TempData["ErrorMessage"] = "Veritabanı Hatası: " + innerMessage;
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UyeIsyeriDokuman(UyeIsyeriDokuman model)
        {
            ModelState.Remove("Basvuru");
            ModelState.Remove("BasvuruId1");

            try
            {
                model.YuklemeTarihi = DateTime.Now;
                _context.Dokumanlar.Add(model);
                ShadowKeyKorumaEkle(model, model.BasvuruId);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Doküman sisteme başarıyla yüklendi.";
            }
            catch (Exception ex)
            {
                var innerMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                TempData["ErrorMessage"] = "Veritabanı Hatası: " + innerMessage;
            }
            return RedirectToAction("UyeIsyeriDokuman");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UyeIsyeriDurumLog(UyeIsyeriDurumLog model)
        {
            ModelState.Remove("Basvuru");
            ModelState.Remove("BasvuruId1");

            if (!ModelState.IsValid) { TempData["ErrorMessage"] = "Eksik parametre girişi."; return RedirectToAction("UyeIsyeriDurumLog"); }
            try
            {
                _context.DurumLoglari.Add(model);
                ShadowKeyKorumaEkle(model, model.BasvuruId);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Süreç durum logu başarıyla üretildi.";
            }
            catch (Exception ex)
            {
                var innerMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                TempData["ErrorMessage"] = "Veritabanı Hatası: " + innerMessage;
            }
            return RedirectToAction("UyeIsyeriDurumLog");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UyeIsyeriCalismaSarti(UyeIsyeriCalismaSarti model)
        {
            ModelState.Remove("Basvuru");
            ModelState.Remove("BasvuruId1");

            if (!ModelState.IsValid)
            {
                ViewBag.Basvurular = GetBasvuruSelectList();
                TempData["ErrorMessage"] = "Form verileri geçerli değil, lütfen alanları kontrol edin.";
                return View(model);
            }
            try
            {
                _context.CalismaSartiIslemleri.Add(model);
                ShadowKeyKorumaEkle(model, model.BasvuruId);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Finansal çalışma şartları başarıyla kaydedildi.";
                return RedirectToAction("UyeIsyeriCalismaSarti");
            }
            catch (Exception ex)
            {
                ViewBag.Basvurular = GetBasvuruSelectList();
                var innerMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                TempData["ErrorMessage"] = "Veritabanı Hatası: " + innerMessage;
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UyeIsyeriKredibilite(UyeIsyeriKredibilite model)
        {
            ModelState.Remove("Basvuru");
            ModelState.Remove("BasvuruId1");

            if (!ModelState.IsValid)
            {
                ViewBag.Basvurular = GetBasvuruSelectList();
                TempData["ErrorMessage"] = "Form verileri geçerli değil, lütfen alanları kontrol edin.";
                return View(model);
            }
            try
            {
                _context.KredibiliteIslemleri.Add(model);
                ShadowKeyKorumaEkle(model, model.BasvuruId);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Mali istihbarat ve kredibilite kaydı sisteme işlendi.";
                return RedirectToAction("UyeIsyeriKredibilite");
            }
            catch (Exception ex)
            {
                ViewBag.Basvurular = GetBasvuruSelectList();
                var innerMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                TempData["ErrorMessage"] = "Veritabanı Hatası: " + innerMessage;
                return View(model);
            }
        }
        #endregion

        #region GET METODLARI
        [HttpGet] public IActionResult UyeIsyeriBasvuru() => View(new UyeIsyeriBasvuruModel());
        [HttpGet] public IActionResult UyeIsyeriFirmaBilgi() { ViewBag.Basvurular = GetBasvuruSelectList(); return View(); }
        [HttpGet] public IActionResult UyeIsyeriYetkili() { ViewBag.Basvurular = GetBasvuruSelectList(); return View(); }
        [HttpGet] public IActionResult UyeIsyeriBankaBilgi() { ViewBag.Basvurular = GetBasvuruSelectList(); return View(); }
        [HttpGet] public IActionResult UyeIsyeriPosTalep() { ViewBag.Basvurular = GetBasvuruSelectList(); return View(); }
        [HttpGet] public IActionResult UyeIsyeriDokuman() { ViewBag.Basvurular = GetBasvuruSelectList(); return View(new UyeIsyeriDokuman()); }
        [HttpGet] public IActionResult UyeIsyeriDurumLog() { ViewBag.Basvurular = GetBasvuruSelectList(); ViewBag.Loglar = _context.DurumLoglari.ToList(); return View(); }
        [HttpGet] public IActionResult UyeIsyeriPosBasvuru() { ViewBag.Basvurular = GetBasvuruSelectList(); return View(); }
        [HttpGet] public IActionResult UyeIsyeriCalismaSarti() { ViewBag.Basvurular = GetBasvuruSelectList(); return View(); }
        [HttpGet] public IActionResult UyeIsyeriKredibilite() { ViewBag.Basvurular = GetBasvuruSelectList(); return View(); }
        #endregion
    }
}