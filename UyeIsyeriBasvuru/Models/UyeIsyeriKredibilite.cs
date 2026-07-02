using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UyeIsyeriBasvuru.Models
{
    [Table("UyeIsyeriKredibilite")]
    public class UyeIsyeriKredibilite
    {
        [Key]
        public long KredibiliteId { get; set; }

        [Required(ErrorMessage = "Lütfen ilişkili bir CRM başvurusu seçiniz.")]
        [Display(Name = "İlişkili Başvuru")]
        public long BasvuruId { get; set; }

        [Required(ErrorMessage = "İşyeri faaliyet konusu detaylarının girilmesi zorunludur.")]
        [StringLength(1000, ErrorMessage = "Açıklama en fazla 1000 karakter olabilir.")]
        [Display(Name = "İşyeri Faaliyet Konusu")]
        public string IsyeriFaaliyetKonusuDetay { get; set; }

        [Required(ErrorMessage = "Şube istihbarat ve değerlendirme görüşü girilmelidir.")]
        [StringLength(1000, ErrorMessage = "Şube görüşü en fazla 1000 karakter olabilir.")]
        [Display(Name = "Genel Şube Görüşü")]
        public string GenelSubeGorusu { get; set; }

        [Range(0, 999999999999, ErrorMessage = "Geçerli bir ciro tutarı giriniz.")]
        [Column(TypeName = "decimal(18, 2)")]
        [Display(Name = "Aylık Yurt İçi Ciro")]
        public decimal AylikYurtIciCiro { get; set; }

        [Required]
        [StringLength(5)]
        [Display(Name = "Para Birimi")]
        public string AylikYurtIciCiroParaBirimi { get; set; } = "TRY";

        [Range(0, 999999999999, ErrorMessage = "Geçerli bir ciro tutarı giriniz.")]
        [Column(TypeName = "decimal(18, 2)")]
        [Display(Name = "Aylık Yurt Dışı Ciro")]
        public decimal AylikYurtDisiCiro { get; set; }

        [Required]
        [StringLength(5)]
        [Display(Name = "Para Birimi")]
        public string AylikYurtDisiCiroParaBirimi { get; set; } = "TRY";

        [Display(Name = "Olumsuz POS İstihbarat Kaydı")]
        public bool IsOlumsuzPosIstihbarat { get; set; }

        // --- EF CORE RELATIONSHIP ---
        [ForeignKey("BasvuruId")]
        public virtual UyeIsyeriBasvuruModel Basvuru { get; set; }
    }
}