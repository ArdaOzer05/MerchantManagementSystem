using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UyeIsyeriBasvuru.Models
{
    [Table("UyeIsyeriPosBasvuru")]
    public class UyeIsyeriPosBasvuru
    {
        [Key]
        public long PosBasvuruId { get; set; }

        [Required(ErrorMessage = "Lütfen ilişkili bir CRM başvurusu seçiniz.")]
        [Display(Name = "İlişkili Başvuru")]
        public long BasvuruId { get; set; }

        [Required(ErrorMessage = "POS tipi (Fiziki/Sanal) seçilmesi zorunludur.")]
        [StringLength(20)]
        [Display(Name = "POS Tipi")]
        public string PosTipi { get; set; }

        [Range(0, 50, ErrorMessage = "Masaüstü POS adedi 0 ile 50 arasında olmalıdır.")]
        [Display(Name = "Masaüstü POS Adedi")]
        public int MasaustuPosAdet { get; set; }

        [Range(0, 50, ErrorMessage = "Mobil POS adedi 0 ile 50 arasında olmalıdır.")]
        [Display(Name = "Mobil POS (GSM) Adedi")]
        public int MobilPosGsmAdet { get; set; }

        [Display(Name = "Toplam POS Sayısı")]
        public int ToplamPosSayisi { get; set; }

        [Display(Name = "Yazar Kasa POS mu?")]
        public bool IsYazarkasaPos { get; set; }

        [Display(Name = "Doktor POS mu?")]
        public bool IsDoktorPos { get; set; }

        [Display(Name = "Çoklu Döviz (Multi-Currency) POS mu?")]
        public bool IsMultiCurrencyPos { get; set; }

        [Display(Name = "Ortak POS mu? (Diğer Bankalarla Paylaşımlı)")]
        public bool IsOrtakPos { get; set; }

        [StringLength(100, ErrorMessage = "Misafir banka adı en fazla 100 karakter olabilir.")]
        [Display(Name = "Misafir Banka Açıklaması")]
        public string? MisafirBanka { get; set; }

        [Display(Name = "Tarım / Üye Kart POS mu?")]
        public bool IsTarimPos { get; set; }

        [Required(ErrorMessage = "VUK Mobil POS durum bilgisi zorunludur.")]
        [StringLength(50)]
        [Display(Name = "VUK Mobil POS Durumu")]
        public string VukMobilPosDurum { get; set; } = "Muaf";

        [Required(ErrorMessage = "VUK Sabit POS durum bilgisi zorunludur.")]
        [StringLength(50)]
        [Display(Name = "VUK Sabit POS Durumu")]
        public string VukSabitPosDurum { get; set; } = "Muaf";

        // --- EF CORE REALTIONSHIP ---
        [ForeignKey("BasvuruId")]
        public virtual UyeIsyeriBasvuruModel Basvuru { get; set; }
    }
}