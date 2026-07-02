using Microsoft.EntityFrameworkCore;
using UyeIsyeriBasvuru.Models;

namespace UyeIsyeriBasvuru.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // Mevcut Tablolar
        public DbSet<UyeIsyeriBasvuruModel> Basvurular { get; set; }
        public DbSet<UyeIsyeriFirmaBilgi> FirmaBilgileri { get; set; }
        public DbSet<UyeIsyeriYetkili> Yetkililer { get; set; }
        public DbSet<UyeIsyeriBankaBilgi> BankaBilgileri { get; set; }
        public DbSet<UyeIsyeriPosTalep> PosTalepleri { get; set; }
        public DbSet<UyeIsyeriDokuman> Dokumanlar { get; set; }
        public DbSet<UyeIsyeriDurumLog> DurumLoglari { get; set; }

        // YENİ EKLENEN TABLOLAR
        public DbSet<UyeIsyeriPosBasvuru> PosIslemleri { get; set; }
        public DbSet<UyeIsyeriCalismaSarti> CalismaSartiIslemleri { get; set; }
        public DbSet<UyeIsyeriKredibilite> KredibiliteIslemleri { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // --- Ana Tablo Tanımlaması ---
            modelBuilder.Entity<UyeIsyeriBasvuruModel>().ToTable("UyeIsyeriBasvuru");

            // --- Bire Çok (1-to-Many) İlişki ve Foreign Key Tanımlamaları ---

            // 1. Firma Bilgileri İlişkisi
            modelBuilder.Entity<UyeIsyeriFirmaBilgi>()
                .ToTable("UyeIsyeriFirmaBilgi")
                .HasKey(x => x.FirmaBilgiId);

            modelBuilder.Entity<UyeIsyeriBasvuruModel>()
                .HasMany(b => b.FirmaBilgileri)
                .WithOne()
                .HasForeignKey(f => f.BasvuruId)
                .OnDelete(DeleteBehavior.Restrict); // Restrict olarak güncellendi

            // 2. Yetkililer İlişkisi
            modelBuilder.Entity<UyeIsyeriYetkili>()
                .ToTable("UyeIsyeriYetkili")
                .HasKey(x => x.YetkiliId);

            modelBuilder.Entity<UyeIsyeriBasvuruModel>()
                .HasMany(b => b.Yetkililer)
                .WithOne()
                .HasForeignKey(y => y.BasvuruId)
                .OnDelete(DeleteBehavior.Restrict); // Restrict olarak güncellendi

            // 3. Banka Bilgileri İlişkisi (Hata Veren Ana Nokta)
            modelBuilder.Entity<UyeIsyeriBankaBilgi>()
                .ToTable("UyeIsyeriBankaBilgi")
                .HasKey(x => x.BankaBilgiId);

            modelBuilder.Entity<UyeIsyeriBasvuruModel>()
                .HasMany(b => b.BankaBilgileri)
                .WithOne()
                .HasForeignKey(bn => bn.BasvuruId)
                .OnDelete(DeleteBehavior.Restrict); // Restrict olarak güncellendi

            // 4. POS Talepleri İlişkisi
            modelBuilder.Entity<UyeIsyeriPosTalep>()
                .ToTable("UyeIsyeriPosTalep")
                .HasKey(x => x.PosTalepId);

            modelBuilder.Entity<UyeIsyeriBasvuruModel>()
                .HasMany(b => b.PosTalepleri)
                .WithOne()
                .HasForeignKey(p => p.BasvuruId)
                .OnDelete(DeleteBehavior.Restrict); // Restrict olarak güncellendi

            // 5. Kredibilite İşlemleri İlişkisi
            modelBuilder.Entity<UyeIsyeriKredibilite>()
                .ToTable("UyeIsyeriKredibilite")
                .HasKey(x => x.KredibiliteId);

            modelBuilder.Entity<UyeIsyeriBasvuruModel>()
                .HasMany(b => b.KredibiliteIslemleri)
                .WithOne()
                .HasForeignKey(k => k.BasvuruId)
                .OnDelete(DeleteBehavior.Restrict); // Restrict olarak güncellendi

            // 6. Dokümanlar İlişkisi
            modelBuilder.Entity<UyeIsyeriDokuman>()
                .ToTable("UyeIsyeriDokuman")
                .HasKey(x => x.DokumanId);

            modelBuilder.Entity<UyeIsyeriBasvuruModel>()
                .HasMany(b => b.Dokumanlar)
                .WithOne()
                .HasForeignKey(d => d.BasvuruId)
                .OnDelete(DeleteBehavior.Restrict); // Restrict olarak güncellendi

            // 7. Durum Logları İlişkisi
            modelBuilder.Entity<UyeIsyeriDurumLog>()
                .ToTable("UyeIsyeriDurumLog")
                .HasKey(x => x.LogId);

            modelBuilder.Entity<UyeIsyeriBasvuruModel>()
                .HasMany(b => b.DurumLoglari)
                .WithOne()
                .HasForeignKey(l => l.BasvuruId)
                .OnDelete(DeleteBehavior.Restrict); // Restrict olarak güncellendi

            // 8. POS Başvuru / İşlemleri İlişkisi
            modelBuilder.Entity<UyeIsyeriPosBasvuru>()
                .ToTable("UyeIsyeriPosBasvuru")
                .HasKey(x => x.PosBasvuruId);

            modelBuilder.Entity<UyeIsyeriBasvuruModel>()
                .HasMany(b => b.PosIslemleri)
                .WithOne()
                .HasForeignKey(p => p.BasvuruId)
                .OnDelete(DeleteBehavior.Restrict); // Restrict olarak güncellendi

            // 9. Çalışma Şartı İşlemleri İlişkisi
            modelBuilder.Entity<UyeIsyeriCalismaSarti>()
                .ToTable("UyeIsyeriCalismaSarti")
                .HasKey(x => x.CalismaSartiId);

            modelBuilder.Entity<UyeIsyeriBasvuruModel>()
                .HasMany(b => b.CalismaSartiIslemleri)
                .WithOne()
                .HasForeignKey(c => c.BasvuruId)
                .OnDelete(DeleteBehavior.Restrict); // Restrict olarak güncellendi

            // --- GARANTİ ADIM ---
            // Gözden kaçan veya shadow property olarak üretilen tüm dış anahtarları da Restrict moduna çeker.
            foreach (var foreignKey in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
    }
}