using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UyeIsyeriBasvuru.Models
{
    [Table("UyeIsyeriPosTalep")]
    public class UyeIsyeriPosTalep
    {
        [Key]
        public long PosTalepId { get; set; }

        [Required(ErrorMessage = "İlişkili bir başvuru seçilmesi zorunludur.")]
        [Display(Name = "İlişkili Başvuru")]
        public long BasvuruId { get; set; }

        [Required(ErrorMessage = "POS tipi (Fiziki, Sanal/VPOS, Mobil vb.) seçilmesi zorunludur.")]
        [StringLength(50)]
        [Display(Name = "POS Tipi")]
        public string PosTipi { get; set; }

        [Required(ErrorMessage = "POS adedi girilmesi zorunludur.")]
        [Range(1, 100, ErrorMessage = "Talep edilen adet en az 1, en fazla 100 olmalıdır.")]
        [Display(Name = "Talep Edilen Adet")]
        public int PosAdedi { get; set; }

        [Required(ErrorMessage = "Marka bilgisi zorunludur.")]
        [StringLength(50, ErrorMessage = "Marka en fazla 50 karakter olabilir.")]
        [Display(Name = "Cihaz Markası")]
        public string Marka { get; set; }

        [Required(ErrorMessage = "Ürün modeli zorunludur.")]
        [StringLength(50, ErrorMessage = "Model en fazla 50 karakter olabilir.")]
        [Display(Name = "Cihaz Modeli")]
        public string UrunModel { get; set; }

        [StringLength(500, ErrorMessage = "Kurulum adresi en fazla 500 karakter olabilir.")]
        [Display(Name = "Fiziki Kurulum Adresi")]
        public string? KurulumAdres { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Talep Durumu")]
        public string TalepDurumu { get; set; } = "Beklemede";

        // --- EF CORE İLİŞKİ YAPISI (Navigation Property) ---
        [ForeignKey("BasvuruId")]
        public virtual UyeIsyeriBasvuruModel Basvuru { get; set; }
    }
}