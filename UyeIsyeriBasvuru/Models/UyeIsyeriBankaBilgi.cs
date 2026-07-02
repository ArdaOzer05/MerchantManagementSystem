using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UyeIsyeriBasvuru.Models
{
    [Table("UyeIsyeriBankaBilgi")]
    public class UyeIsyeriBankaBilgi
    {
        [Key]
        public long BankaBilgiId { get; set; }

        [Required(ErrorMessage = "İlişkili bir başvuru seçilmesi zorunludur.")]
        [Display(Name = "İlişkili Başvuru")]
        public long BasvuruId { get; set; }

        [Required(ErrorMessage = "Banka adı girilmesi zorunludur.")]
        [StringLength(100, ErrorMessage = "Banka adı en fazla 100 karakter olabilir.")]
        [Display(Name = "Banka Adı")]
        public string BankaAdi { get; set; }

        [Required(ErrorMessage = "IBAN alanı boş geçilemez.")]
        [StringLength(26, MinimumLength = 26, ErrorMessage = "IBAN numarası tam 26 karakter olmalıdır.")]
        [RegularExpression(@"^TR[0-9]{24}$", ErrorMessage = "Geçerli bir TR IBAN formatı giriniz (Örn: TR ve peşine 24 rakam).")]
        [Display(Name = "IBAN Numarası")]
        public string Iban { get; set; }

        [Required(ErrorMessage = "Hesap sahibi bilgisi zorunludur.")]
        [StringLength(150, ErrorMessage = "Hesap sahibi alanı en fazla 150 karakter olabilir.")]
        [Display(Name = "Hesap Sahibi (Ünvan/Ad Soyad)")]
        public string HesapSahibi { get; set; }

        [Required(ErrorMessage = "Hesap numarası zorunludur.")]
        [StringLength(30, ErrorMessage = "Hesap numarası en fazla 30 karakter olabilir.")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Hesap numarası sadece rakamlardan oluşmalıdır.")]
        [Display(Name = "Hesap Numarası")]
        public string HesapNo { get; set; }

        // --- EF CORE İLİŞKİ YAPISI (Navigation Property) ---
        [ForeignKey("BasvuruId")]
        public virtual UyeIsyeriBasvuruModel Basvuru { get; set; }
    }
}