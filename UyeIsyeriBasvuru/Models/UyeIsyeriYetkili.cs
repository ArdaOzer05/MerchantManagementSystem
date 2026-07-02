using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UyeIsyeriBasvuru.Models
{
    [Table("UyeIsyeriYetkili")]
    public class UyeIsyeriYetkili
    {
        [Key]
        public long YetkiliId { get; set; }

        [Required(ErrorMessage = "İlişkili bir başvuru seçilmesi zorunludur.")]
        [Display(Name = "İlişkili Başvuru")]
        public long BasvuruId { get; set; }

        [Required(ErrorMessage = "Yetkili adı alanı zorunludur.")]
        [StringLength(50, ErrorMessage = "Yetkili adı en fazla 50 karakter olabilir.")]
        [Display(Name = "Yetkili Adı")]
        public string Ad { get; set; }

        [Required(ErrorMessage = "Yetkili soyadı alanı zorunludur.")]
        [StringLength(50, ErrorMessage = "Yetkili soyadı en fazla 50 karakter olabilir.")]
        [Display(Name = "Yetkili Soyadı")]
        public string Soyad { get; set; }

        [Required(ErrorMessage = "T.C. Kimlik Numarası zorunludur.")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "T.C. Kimlik Numarası tam 11 haneli olmalıdır.")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "T.C. Kimlik Numarası sadece rakamlardan oluşmalıdır.")]
        [Display(Name = "T.C. Kimlik No")]
        public string TcKimlikNo { get; set; }

        [Required(ErrorMessage = "Cep telefonu zorunludur.")]
        [RegularExpression(@"^5[0-9]{9}$", ErrorMessage = "Telefon formatı başında sıfır olmadan 5XXXXXXXXX şeklinde olmalıdır.")]
        [Display(Name = "Cep Telefonu")]
        public string CepTelefonu { get; set; }

        [Required(ErrorMessage = "E-posta alanı zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        [StringLength(100, ErrorMessage = "E-posta adresi en fazla 100 karakter olabilir.")]
        [Display(Name = "E-Posta Adresi")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Yetki tipi seçimi zorunludur.")]
        [StringLength(50, ErrorMessage = "Yetki tipi alanı en fazla 50 karakter olabilir.")]
        [Display(Name = "Şirket İçi Yetki Tipi")]
        public string YetkiTipi { get; set; }

        // --- EF CORE İLİŞKİ YAPISI (Navigation Property) ---
        [ForeignKey("BasvuruId")]
        public virtual UyeIsyeriBasvuruModel Basvuru { get; set; }
    }
}