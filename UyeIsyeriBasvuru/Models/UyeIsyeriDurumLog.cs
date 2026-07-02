using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UyeIsyeriBasvuru.Models
{
    [Table("UyeIsyeriDurumLog")]
    public class UyeIsyeriDurumLog
    {
        [Key]
        public long LogId { get; set; }

        [Required(ErrorMessage = "İlişkili bir başvuru seçilmesi zorunludur.")]
        [Display(Name = "İlişkili Başvuru")]
        public long BasvuruId { get; set; }

        [Required(ErrorMessage = "Eski durum bilgisi zorunludur.")]
        [StringLength(50)]
        [Display(Name = "Eski Başvuru Durumu")]
        public string EskiDurum { get; set; }

        [Required(ErrorMessage = "Yeni durum bilgisi zorunludur.")]
        [StringLength(50)]
        [Display(Name = "Yeni Başvuru Durumu")]
        public string YeniDurum { get; set; }

        [StringLength(500, ErrorMessage = "Değişiklik gerekçesi/açıklama en fazla 500 karakter olabilir.")]
        [Display(Name = "Değişiklik Gerekçesi / Açıklama")]
        public string Aciklama { get; set; }

        [Required(ErrorMessage = "İşlemi gerçekleştiren personel belirtilmelidir.")]
        [StringLength(100)]
        [Display(Name = "İşlem Yapan Kullanıcı")]
        public string IslemYapanKullanici { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Log İşlem Tarihi")]
        public DateTime IslemTarihi { get; set; } = DateTime.Now;

        // --- EF CORE İLİŞKİ YAPISI (Navigation Property) ---
        [ForeignKey("BasvuruId")]
        public virtual UyeIsyeriBasvuruModel Basvuru { get; set; }
    }
}