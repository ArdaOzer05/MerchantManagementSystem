using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UyeIsyeriBasvuru.Models
{
    [Table("UyeIsyeriDokuman")]
    public class UyeIsyeriDokuman
    {
        [Key]
        public long DokumanId { get; set; }

        [Required(ErrorMessage = "Başvuru seçimi zorunludur.")]
        [Display(Name = "İlişkili Başvuru")]
        public long BasvuruId { get; set; }

        [Required(ErrorMessage = "Doküman tipi zorunludur.")]
        [StringLength(100)]
        [Display(Name = "Doküman Tipi")]
        public string DokumanTipi { get; set; }

        [Required(ErrorMessage = "Dosya adı zorunludur.")]
        [StringLength(255, ErrorMessage = "Dosya adı en fazla 255 karakter olabilir.")]
        [Display(Name = "Dosya Adı")]
        public string DosyaAdi { get; set; }

        [Required(ErrorMessage = "Dosya yolu belirlenmelidir.")]
        [StringLength(500, ErrorMessage = "Dosya yolu en fazla 500 karakter olabilir.")]
        [Display(Name = "Dosya Depolama Yolu")]
        public string DosyaYolu { get; set; }

        [Required]
        [Display(Name = "Yükleme Tarihi")]
        public DateTime YuklemeTarihi { get; set; } = DateTime.Now;

        // --- EF CORE İLİŞKİ YAPISI (Navigation Property) ---
        [ForeignKey("BasvuruId")]
        public virtual UyeIsyeriBasvuruModel Basvuru { get; set; }
    }
}