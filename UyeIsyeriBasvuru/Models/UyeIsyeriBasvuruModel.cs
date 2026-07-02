using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UyeIsyeriBasvuru.Models
{
    [Table("UyeIsyeriBasvuru")]
    public class UyeIsyeriBasvuruModel
    {
        [Key]
        public long BasvuruId { get; set; }

        [Required(ErrorMessage = "Başvuru numarası boş geçilemez.")]
        [StringLength(20, ErrorMessage = "Başvuru numarası en fazla 20 karakter olabilir.")]
        [Display(Name = "Başvuru Numarası")]
        public string? BasvuruNO { get; set; }

        [Required(ErrorMessage = "Lütfen müşteri tipini seçiniz.")]
        [StringLength(50, ErrorMessage = "Müşteri tipi alanı en fazla 50 karakter olabilir.")]
        [Display(Name = "Müşteri Tipi")]
        public string? MusteriTipi { get; set; }

        [Required(ErrorMessage = "Başvuru tarihi girilmelidir.")]
        [DataType(DataType.Date)]
        [Display(Name = "Başvuru Tarihi")]
        public DateTime BasvuruTarihi { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Başvuru durumu belirtilmelidir.")]
        [StringLength(50, ErrorMessage = "Başvuru durumu en fazla 50 karakter olabilir.")]
        [Display(Name = "Başvuru Durumu")]
        public string BasvuruDurumu { get; set; } = "İncelemede"; // Varsayılan kurumsal değer

        [DataType(DataType.Date)]
        [Display(Name = "Onay Tarihi")]
        public DateTime? OnayTarihi { get; set; }

        [Display(Name = "Red Nedeni (Kod)")]
        public int? RedNedeni { get; set; }

        [Display(Name = "Aktif Başvuru")]
        public bool AktifMi { get; set; } = true;

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Sistem Kayıt Tarihi")]
        public DateTime KayitTarihi { get; set; } = DateTime.Now;

        [DataType(DataType.Date)]
        [Display(Name = "Son Güncelleme Tarihi")]
        public DateTime? GuncellemeTarihi { get; set; }

        // --- ORTAK İLİŞKİLER (Senior Best-Practice / Navigation Properties) ---
        // Koleksiyonlar nullable yapıldı ve form aşamasında Validation hatası vermemesi için initialize edildi.
        public virtual ICollection<UyeIsyeriFirmaBilgi>? FirmaBilgileri { get; set; } = new List<UyeIsyeriFirmaBilgi>();
        public virtual ICollection<UyeIsyeriYetkili>? Yetkililer { get; set; } = new List<UyeIsyeriYetkili>();
        public virtual ICollection<UyeIsyeriBankaBilgi>? BankaBilgileri { get; set; } = new List<UyeIsyeriBankaBilgi>();
        public virtual ICollection<UyeIsyeriPosBasvuru>? PosIslemleri { get; set; } = new List<UyeIsyeriPosBasvuru>();
        public virtual ICollection<UyeIsyeriPosTalep>? PosTalepleri { get; set; } = new List<UyeIsyeriPosTalep>();
        public virtual ICollection<UyeIsyeriDokuman>? Dokumanlar { get; set; } = new List<UyeIsyeriDokuman>();
        public virtual ICollection<UyeIsyeriDurumLog>? DurumLoglari { get; set; } = new List<UyeIsyeriDurumLog>();
        public virtual ICollection<UyeIsyeriCalismaSarti>? CalismaSartiIslemleri { get; set; } = new List<UyeIsyeriCalismaSarti>();
        public virtual ICollection<UyeIsyeriKredibilite>? KredibiliteIslemleri { get; set; } = new List<UyeIsyeriKredibilite>();
    }
}