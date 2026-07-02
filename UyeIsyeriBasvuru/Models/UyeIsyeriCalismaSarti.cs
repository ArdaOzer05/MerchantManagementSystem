using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UyeIsyeriBasvuru.Models
{
    [Table("UyeIsyeriCalismaSarti")]
    public class UyeIsyeriCalismaSarti
    {
        [Key]
        public long CalismaSartiId { get; set; }

        [Required(ErrorMessage = "Lütfen ilişkili bir CRM başvurusu seçiniz.")]
        [Display(Name = "İlişkili Başvuru")]
        public long BasvuruId { get; set; }

        [Required(ErrorMessage = "Marka paylaşım tipi seçimi zorunludur.")]
        [StringLength(30)]
        [Display(Name = "Marka Paylaşım Tipi")]
        public string MarkaPaylasimTipi { get; set; }

        [Display(Name = "Taksit Seçeneği Aktif Mi?")]
        public bool IsTaksitSecim { get; set; }

        [Required(ErrorMessage = "Başvuru tipi alanı zorunludur.")]
        [StringLength(50)]
        [Display(Name = "Başvuru Tipi")]
        public string BasvuruTipi { get; set; } = "Standart Başvuru";

        [Required(ErrorMessage = "Tek ödeme çalışma şekli seçilmelidir.")]
        [StringLength(50)]
        [Display(Name = "Tek Ödeme Çalışma Şekli")]
        public string TekOdemeCalismaSekli { get; set; }

        [Range(0, 100, ErrorMessage = "Puan oranı 0 ile 100 arasında olmalıdır.")]
        [Column(TypeName = "decimal(18, 3)")]
        [Display(Name = "Tek Ödeme Puan Oranı (%)")]
        public decimal TekOdemePuanOrani { get; set; }

        [Display(Name = "Özel Yetki Var Mı?")]
        public bool IsOzelYetkiFlag { get; set; }

        [StringLength(10, ErrorMessage = "Yetki veren personel sicil numarası en fazla 10 karakter olabilir.")]
        [Display(Name = "Yetki Veren Sicil")]
        public string? OzelYetkiVerenSicil { get; set; }

        [StringLength(50)]
        [Display(Name = "Taksitli Çalışma Şekli")]
        public string? TaksitliCalismaSekli { get; set; }

        [Range(0, 100, ErrorMessage = "Taksitli puan oranı 0 ile 100 arasında olmalıdır.")]
        [Column(TypeName = "decimal(18, 3)")]
        [Display(Name = "Taksitli Puan Oranı (%)")]
        public decimal TaksitliPuanOrani { get; set; }

        // --- EF CORE RELATIONSHIP ---
        [ForeignKey("BasvuruId")]
        public virtual UyeIsyeriBasvuruModel Basvuru { get; set; }
    }
}