using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UyeIsyeriBasvuru.Migrations
{
    /// <inheritdoc />
    public partial class InitialCleanDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UyeIsyeriBasvuru",
                columns: table => new
                {
                    BasvuruId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BasvuruNO = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MusteriTipi = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BasvuruTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BasvuruDurumu = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    OnayTarihi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RedNedeni = table.Column<int>(type: "int", nullable: true),
                    AktifMi = table.Column<bool>(type: "bit", nullable: false),
                    KayitTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UyeIsyeriBasvuru", x => x.BasvuruId);
                });

            migrationBuilder.CreateTable(
                name: "UyeIsyeriBankaBilgi",
                columns: table => new
                {
                    BankaBilgiId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BasvuruId = table.Column<long>(type: "bigint", nullable: false),
                    BankaAdi = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Iban = table.Column<string>(type: "nvarchar(26)", maxLength: 26, nullable: false),
                    HesapSahibi = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    HesapNo = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    BasvuruId1 = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UyeIsyeriBankaBilgi", x => x.BankaBilgiId);
                    table.ForeignKey(
                        name: "FK_UyeIsyeriBankaBilgi_UyeIsyeriBasvuru_BasvuruId",
                        column: x => x.BasvuruId,
                        principalTable: "UyeIsyeriBasvuru",
                        principalColumn: "BasvuruId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UyeIsyeriBankaBilgi_UyeIsyeriBasvuru_BasvuruId1",
                        column: x => x.BasvuruId1,
                        principalTable: "UyeIsyeriBasvuru",
                        principalColumn: "BasvuruId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UyeIsyeriCalismaSarti",
                columns: table => new
                {
                    CalismaSartiId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BasvuruId = table.Column<long>(type: "bigint", nullable: false),
                    MarkaPaylasimTipi = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    IsTaksitSecim = table.Column<bool>(type: "bit", nullable: false),
                    BasvuruTipi = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TekOdemeCalismaSekli = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TekOdemePuanOrani = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    IsOzelYetkiFlag = table.Column<bool>(type: "bit", nullable: false),
                    OzelYetkiVerenSicil = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    TaksitliCalismaSekli = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TaksitliPuanOrani = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    BasvuruId1 = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UyeIsyeriCalismaSarti", x => x.CalismaSartiId);
                    table.ForeignKey(
                        name: "FK_UyeIsyeriCalismaSarti_UyeIsyeriBasvuru_BasvuruId",
                        column: x => x.BasvuruId,
                        principalTable: "UyeIsyeriBasvuru",
                        principalColumn: "BasvuruId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UyeIsyeriCalismaSarti_UyeIsyeriBasvuru_BasvuruId1",
                        column: x => x.BasvuruId1,
                        principalTable: "UyeIsyeriBasvuru",
                        principalColumn: "BasvuruId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UyeIsyeriDokuman",
                columns: table => new
                {
                    DokumanId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BasvuruId = table.Column<long>(type: "bigint", nullable: false),
                    DokumanTipi = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DosyaAdi = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DosyaYolu = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    YuklemeTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BasvuruId1 = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UyeIsyeriDokuman", x => x.DokumanId);
                    table.ForeignKey(
                        name: "FK_UyeIsyeriDokuman_UyeIsyeriBasvuru_BasvuruId",
                        column: x => x.BasvuruId,
                        principalTable: "UyeIsyeriBasvuru",
                        principalColumn: "BasvuruId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UyeIsyeriDokuman_UyeIsyeriBasvuru_BasvuruId1",
                        column: x => x.BasvuruId1,
                        principalTable: "UyeIsyeriBasvuru",
                        principalColumn: "BasvuruId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UyeIsyeriDurumLog",
                columns: table => new
                {
                    LogId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BasvuruId = table.Column<long>(type: "bigint", nullable: false),
                    EskiDurum = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    YeniDurum = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Aciklama = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IslemYapanKullanici = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IslemTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BasvuruId1 = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UyeIsyeriDurumLog", x => x.LogId);
                    table.ForeignKey(
                        name: "FK_UyeIsyeriDurumLog_UyeIsyeriBasvuru_BasvuruId",
                        column: x => x.BasvuruId,
                        principalTable: "UyeIsyeriBasvuru",
                        principalColumn: "BasvuruId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UyeIsyeriDurumLog_UyeIsyeriBasvuru_BasvuruId1",
                        column: x => x.BasvuruId1,
                        principalTable: "UyeIsyeriBasvuru",
                        principalColumn: "BasvuruId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UyeIsyeriFirmaBilgi",
                columns: table => new
                {
                    FirmaBilgiId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BasvuruId = table.Column<long>(type: "bigint", nullable: false),
                    FirmaUnvan = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    VergiNo = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    VergiDairesi = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MersisNo = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    FaaliyetKonusu = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    KurulusTarihi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Telefon = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Adres = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Il = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BasvuruId1 = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UyeIsyeriFirmaBilgi", x => x.FirmaBilgiId);
                    table.ForeignKey(
                        name: "FK_UyeIsyeriFirmaBilgi_UyeIsyeriBasvuru_BasvuruId",
                        column: x => x.BasvuruId,
                        principalTable: "UyeIsyeriBasvuru",
                        principalColumn: "BasvuruId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UyeIsyeriFirmaBilgi_UyeIsyeriBasvuru_BasvuruId1",
                        column: x => x.BasvuruId1,
                        principalTable: "UyeIsyeriBasvuru",
                        principalColumn: "BasvuruId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UyeIsyeriKredibilite",
                columns: table => new
                {
                    KredibiliteId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BasvuruId = table.Column<long>(type: "bigint", nullable: false),
                    IsyeriFaaliyetKonusuDetay = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    GenelSubeGorusu = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    AylikYurtIciCiro = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AylikYurtIciCiroParaBirimi = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    AylikYurtDisiCiro = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AylikYurtDisiCiroParaBirimi = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    IsOlumsuzPosIstihbarat = table.Column<bool>(type: "bit", nullable: false),
                    BasvuruId1 = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UyeIsyeriKredibilite", x => x.KredibiliteId);
                    table.ForeignKey(
                        name: "FK_UyeIsyeriKredibilite_UyeIsyeriBasvuru_BasvuruId",
                        column: x => x.BasvuruId,
                        principalTable: "UyeIsyeriBasvuru",
                        principalColumn: "BasvuruId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UyeIsyeriKredibilite_UyeIsyeriBasvuru_BasvuruId1",
                        column: x => x.BasvuruId1,
                        principalTable: "UyeIsyeriBasvuru",
                        principalColumn: "BasvuruId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UyeIsyeriPosBasvuru",
                columns: table => new
                {
                    PosBasvuruId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BasvuruId = table.Column<long>(type: "bigint", nullable: false),
                    PosTipi = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MasaustuPosAdet = table.Column<int>(type: "int", nullable: false),
                    MobilPosGsmAdet = table.Column<int>(type: "int", nullable: false),
                    ToplamPosSayisi = table.Column<int>(type: "int", nullable: false),
                    IsYazarkasaPos = table.Column<bool>(type: "bit", nullable: false),
                    IsDoktorPos = table.Column<bool>(type: "bit", nullable: false),
                    IsMultiCurrencyPos = table.Column<bool>(type: "bit", nullable: false),
                    IsOrtakPos = table.Column<bool>(type: "bit", nullable: false),
                    MisafirBanka = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsTarimPos = table.Column<bool>(type: "bit", nullable: false),
                    VukMobilPosDurum = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    VukSabitPosDurum = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BasvuruId1 = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UyeIsyeriPosBasvuru", x => x.PosBasvuruId);
                    table.ForeignKey(
                        name: "FK_UyeIsyeriPosBasvuru_UyeIsyeriBasvuru_BasvuruId",
                        column: x => x.BasvuruId,
                        principalTable: "UyeIsyeriBasvuru",
                        principalColumn: "BasvuruId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UyeIsyeriPosBasvuru_UyeIsyeriBasvuru_BasvuruId1",
                        column: x => x.BasvuruId1,
                        principalTable: "UyeIsyeriBasvuru",
                        principalColumn: "BasvuruId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UyeIsyeriPosTalep",
                columns: table => new
                {
                    PosTalepId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BasvuruId = table.Column<long>(type: "bigint", nullable: false),
                    PosTipi = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PosAdedi = table.Column<int>(type: "int", nullable: false),
                    Marka = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UrunModel = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    KurulumAdres = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    TalepDurumu = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BasvuruId1 = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UyeIsyeriPosTalep", x => x.PosTalepId);
                    table.ForeignKey(
                        name: "FK_UyeIsyeriPosTalep_UyeIsyeriBasvuru_BasvuruId",
                        column: x => x.BasvuruId,
                        principalTable: "UyeIsyeriBasvuru",
                        principalColumn: "BasvuruId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UyeIsyeriPosTalep_UyeIsyeriBasvuru_BasvuruId1",
                        column: x => x.BasvuruId1,
                        principalTable: "UyeIsyeriBasvuru",
                        principalColumn: "BasvuruId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UyeIsyeriYetkili",
                columns: table => new
                {
                    YetkiliId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BasvuruId = table.Column<long>(type: "bigint", nullable: false),
                    Ad = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Soyad = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TcKimlikNo = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    CepTelefonu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    YetkiTipi = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BasvuruId1 = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UyeIsyeriYetkili", x => x.YetkiliId);
                    table.ForeignKey(
                        name: "FK_UyeIsyeriYetkili_UyeIsyeriBasvuru_BasvuruId",
                        column: x => x.BasvuruId,
                        principalTable: "UyeIsyeriBasvuru",
                        principalColumn: "BasvuruId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UyeIsyeriYetkili_UyeIsyeriBasvuru_BasvuruId1",
                        column: x => x.BasvuruId1,
                        principalTable: "UyeIsyeriBasvuru",
                        principalColumn: "BasvuruId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UyeIsyeriBankaBilgi_BasvuruId",
                table: "UyeIsyeriBankaBilgi",
                column: "BasvuruId");

            migrationBuilder.CreateIndex(
                name: "IX_UyeIsyeriBankaBilgi_BasvuruId1",
                table: "UyeIsyeriBankaBilgi",
                column: "BasvuruId1");

            migrationBuilder.CreateIndex(
                name: "IX_UyeIsyeriCalismaSarti_BasvuruId",
                table: "UyeIsyeriCalismaSarti",
                column: "BasvuruId");

            migrationBuilder.CreateIndex(
                name: "IX_UyeIsyeriCalismaSarti_BasvuruId1",
                table: "UyeIsyeriCalismaSarti",
                column: "BasvuruId1");

            migrationBuilder.CreateIndex(
                name: "IX_UyeIsyeriDokuman_BasvuruId",
                table: "UyeIsyeriDokuman",
                column: "BasvuruId");

            migrationBuilder.CreateIndex(
                name: "IX_UyeIsyeriDokuman_BasvuruId1",
                table: "UyeIsyeriDokuman",
                column: "BasvuruId1");

            migrationBuilder.CreateIndex(
                name: "IX_UyeIsyeriDurumLog_BasvuruId",
                table: "UyeIsyeriDurumLog",
                column: "BasvuruId");

            migrationBuilder.CreateIndex(
                name: "IX_UyeIsyeriDurumLog_BasvuruId1",
                table: "UyeIsyeriDurumLog",
                column: "BasvuruId1");

            migrationBuilder.CreateIndex(
                name: "IX_UyeIsyeriFirmaBilgi_BasvuruId",
                table: "UyeIsyeriFirmaBilgi",
                column: "BasvuruId");

            migrationBuilder.CreateIndex(
                name: "IX_UyeIsyeriFirmaBilgi_BasvuruId1",
                table: "UyeIsyeriFirmaBilgi",
                column: "BasvuruId1");

            migrationBuilder.CreateIndex(
                name: "IX_UyeIsyeriKredibilite_BasvuruId",
                table: "UyeIsyeriKredibilite",
                column: "BasvuruId");

            migrationBuilder.CreateIndex(
                name: "IX_UyeIsyeriKredibilite_BasvuruId1",
                table: "UyeIsyeriKredibilite",
                column: "BasvuruId1");

            migrationBuilder.CreateIndex(
                name: "IX_UyeIsyeriPosBasvuru_BasvuruId",
                table: "UyeIsyeriPosBasvuru",
                column: "BasvuruId");

            migrationBuilder.CreateIndex(
                name: "IX_UyeIsyeriPosBasvuru_BasvuruId1",
                table: "UyeIsyeriPosBasvuru",
                column: "BasvuruId1");

            migrationBuilder.CreateIndex(
                name: "IX_UyeIsyeriPosTalep_BasvuruId",
                table: "UyeIsyeriPosTalep",
                column: "BasvuruId");

            migrationBuilder.CreateIndex(
                name: "IX_UyeIsyeriPosTalep_BasvuruId1",
                table: "UyeIsyeriPosTalep",
                column: "BasvuruId1");

            migrationBuilder.CreateIndex(
                name: "IX_UyeIsyeriYetkili_BasvuruId",
                table: "UyeIsyeriYetkili",
                column: "BasvuruId");

            migrationBuilder.CreateIndex(
                name: "IX_UyeIsyeriYetkili_BasvuruId1",
                table: "UyeIsyeriYetkili",
                column: "BasvuruId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UyeIsyeriBankaBilgi");

            migrationBuilder.DropTable(
                name: "UyeIsyeriCalismaSarti");

            migrationBuilder.DropTable(
                name: "UyeIsyeriDokuman");

            migrationBuilder.DropTable(
                name: "UyeIsyeriDurumLog");

            migrationBuilder.DropTable(
                name: "UyeIsyeriFirmaBilgi");

            migrationBuilder.DropTable(
                name: "UyeIsyeriKredibilite");

            migrationBuilder.DropTable(
                name: "UyeIsyeriPosBasvuru");

            migrationBuilder.DropTable(
                name: "UyeIsyeriPosTalep");

            migrationBuilder.DropTable(
                name: "UyeIsyeriYetkili");

            migrationBuilder.DropTable(
                name: "UyeIsyeriBasvuru");
        }
    }
}
