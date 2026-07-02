using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UyeIsyeriBasvuru.Models
{
    [Table("UyeIsyeriFirmaBilgi")]
    public class UyeIsyeriFirmaBilgi
    {
        [Key]
        public long FirmaBilgiId { get; set; }

        [Required(ErrorMessage = "İlişkili bir başvuru seçilmelidir.")]
        [Display(Name = "İlişkili Başvuru")]
        public long BasvuruId { get; set; }

        [Required(ErrorMessage = "Firma ticari ünvanı zorunludur.")]
        [StringLength(200, ErrorMessage = "Ünvan en fazla 200 karakter olabilir.")]
        [Display(Name = "Firma Ticari Ünvanı")]
        public string FirmaUnvan { get; set; }

        [Required(ErrorMessage = "Vergi numarası zorunludur.")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Vergi numarası tam 10 haneli olmalıdır.")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Vergi numarası sadece rakamlardan oluşmalıdır.")]
        [Display(Name = "Vergi Numarası")]
        public string VergiNo { get; set; }

        [Required(ErrorMessage = "Vergi dairesi seçimi veya girişi zorunludur.")]
        [StringLength(100, ErrorMessage = "Vergi dairesi en fazla 100 karakter olabilir.")]
        [Display(Name = "Vergi Dairesi")]
        public string VergiDairesi { get; set; }

        [StringLength(16, ErrorMessage = "MERSİS numarası en fazla 16 karakter olabilir.")]
        [Display(Name = "MERSİS Numarası")]
        public string MersisNo { get; set; }

        [Required(ErrorMessage = "Faaliyet konusu zorunludur.")]
        [StringLength(250, ErrorMessage = "Faaliyet konusu en fazla 250 karakter olabilir.")]
        [Display(Name = "Faaliyet Konusu")]
        public string FaaliyetKonusu { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Kuruluş Tarihi")]
        public DateTime? KurulusTarihi { get; set; }

        [Required(ErrorMessage = "Telefon numarası zorunludur.")]
        [RegularExpression(@"^5[0-9]{9}$", ErrorMessage = "Telefon formatı başında sıfır olmadan 5XXXXXXXXX şeklinde olmalıdır.")]
        [Display(Name = "Firma Telefonu")]
        public string Telefon { get; set; }

        [Required(ErrorMessage = "E-posta adresi zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir kurumsal e-posta adresi giriniz.")]
        [StringLength(100, ErrorMessage = "E-posta alanı en fazla 100 karakter olabilir.")]
        [Display(Name = "E-Posta Adresi")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Firma açık adresi zorunludur.")]
        [StringLength(500, ErrorMessage = "Adres en fazla 500 karakter olabilir.")]
        [Display(Name = "Açık Adres")]
        public string Adres { get; set; }

        [Required(ErrorMessage = "İl seçimi zorunludur.")]
        [StringLength(50)]
        [Display(Name = "Firma Merkez İli")]
        public string Il { get; set; }

        // --- EF CORE İLİŞKİ YAPISI (Navigation Property) ---
        [ForeignKey("BasvuruId")]
        public virtual UyeIsyeriBasvuruModel Basvuru { get; set; }
    }
}