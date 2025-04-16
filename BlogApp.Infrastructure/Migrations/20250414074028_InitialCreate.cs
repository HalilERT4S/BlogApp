using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BlogApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UserTypeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Roles_UserTypeId",
                        column: x => x.UserTypeId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Blogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PublishDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Blogs_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Blogs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProfanityStatus = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    BlogId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_Blogs_BlogId",
                        column: x => x.BlogId,
                        principalTable: "Blogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Likes",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    BlogId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Likes", x => new { x.UserId, x.BlogId });
                    table.ForeignKey(
                        name: "FK_Likes_Blogs_BlogId",
                        column: x => x.BlogId,
                        principalTable: "Blogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Likes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SavedBlogs",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    BlogId = table.Column<int>(type: "int", nullable: false),
                    SavedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SavedBlogs", x => new { x.UserId, x.BlogId });
                    table.ForeignKey(
                        name: "FK_SavedBlogs_Blogs_BlogId",
                        column: x => x.BlogId,
                        principalTable: "Blogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SavedBlogs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Teknoloji" },
                    { 2, "Seyahat" },
                    { 3, "Yemek" },
                    { 4, "Moda" },
                    { 5, "Sağlık" },
                    { 6, "Spor" },
                    { 7, "Otomobil" },
                    { 8, "Sinema" },
                    { 9, "Müzik" },
                    { 10, "Kitap" },
                    { 11, "Bilim" },
                    { 12, "Sanat" },
                    { 13, "Tarih" },
                    { 14, "Ekonomi" },
                    { 15, "Eğitim" },
                    { 16, "Oyun" },
                    { 17, "Fotoğrafçılık" },
                    { 18, "Bahçe" },
                    { 19, "Evcil Hayvan" },
                    { 20, "Felsefe" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "User" },
                    { 2, "Admin" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "FirstName", "LastName", "PasswordHash", "PasswordSalt", "UserTypeId", "Username" },
                values: new object[,]
                {
                    { 1, "Ahmet", "Yılmaz", new byte[] { 137, 158, 165, 199, 11, 117, 125, 202, 100, 132, 120, 19, 145, 150, 101, 144, 127, 228, 122, 112, 170, 196, 169, 126, 116, 181, 146, 183, 179, 151, 196, 118, 135, 235, 166, 227, 172, 143, 120, 122, 151, 191, 158, 123, 118, 171, 193, 132, 10, 188, 167, 106, 142, 136, 173, 149, 197, 184, 110, 195, 169, 175, 141, 115 }, new byte[] { 112, 15, 203, 179, 125, 186, 183, 189, 205, 119, 165, 183, 185, 214, 109, 130, 14, 105, 139, 164, 197, 153, 143, 194, 108, 196, 133, 127, 120, 173, 150, 101, 188, 154, 188, 11, 166, 162, 11, 104, 142, 177, 134, 167, 135, 118, 158, 116, 107, 110, 197, 153, 148, 106, 156, 247, 190, 148, 146, 187, 128, 181, 145, 144 }, 1, "yazar1" },
                    { 2, "Ayşe", "Demir", new byte[] { 137, 158, 165, 199, 11, 117, 125, 202, 100, 132, 120, 19, 145, 150, 101, 144, 127, 228, 122, 112, 170, 196, 169, 126, 116, 181, 146, 183, 179, 151, 196, 118, 135, 235, 166, 227, 172, 143, 120, 122, 151, 191, 158, 123, 118, 171, 193, 132, 10, 188, 167, 106, 142, 136, 173, 149, 197, 184, 110, 195, 169, 175, 141, 115 }, new byte[] { 112, 15, 203, 179, 125, 186, 183, 189, 205, 119, 165, 183, 185, 214, 109, 130, 14, 105, 139, 164, 197, 153, 143, 194, 108, 196, 133, 127, 120, 173, 150, 101, 188, 154, 188, 11, 166, 162, 11, 104, 142, 177, 134, 167, 135, 118, 158, 116, 107, 110, 197, 153, 148, 106, 156, 247, 190, 148, 146, 187, 128, 181, 145, 144 }, 1, "yazar2" },
                    { 3, "Mehmet", "Öztürk", new byte[] { 137, 158, 165, 199, 11, 117, 125, 202, 100, 132, 120, 19, 145, 150, 101, 144, 127, 228, 122, 112, 170, 196, 169, 126, 116, 181, 146, 183, 179, 151, 196, 118, 135, 235, 166, 227, 172, 143, 120, 122, 151, 191, 158, 123, 118, 171, 193, 132, 10, 188, 167, 106, 142, 136, 173, 149, 197, 184, 110, 195, 169, 175, 141, 115 }, new byte[] { 112, 15, 203, 179, 125, 186, 183, 189, 205, 119, 165, 183, 185, 214, 109, 130, 14, 105, 139, 164, 197, 153, 143, 194, 108, 196, 133, 127, 120, 173, 150, 101, 188, 154, 188, 11, 166, 162, 11, 104, 142, 177, 134, 167, 135, 118, 158, 116, 107, 110, 197, 153, 148, 106, 156, 247, 190, 148, 146, 187, 128, 181, 145, 144 }, 1, "editor1" },
                    { 4, "Zeynep", "Kaya", new byte[] { 137, 158, 165, 199, 11, 117, 125, 202, 100, 132, 120, 19, 145, 150, 101, 144, 127, 228, 122, 112, 170, 196, 169, 126, 116, 181, 146, 183, 179, 151, 196, 118, 135, 235, 166, 227, 172, 143, 120, 122, 151, 191, 158, 123, 118, 171, 193, 132, 10, 188, 167, 106, 142, 136, 173, 149, 197, 184, 110, 195, 169, 175, 141, 115 }, new byte[] { 112, 15, 203, 179, 125, 186, 183, 189, 205, 119, 165, 183, 185, 214, 109, 130, 14, 105, 139, 164, 197, 153, 143, 194, 108, 196, 133, 127, 120, 173, 150, 101, 188, 154, 188, 11, 166, 162, 11, 104, 142, 177, 134, 167, 135, 118, 158, 116, 107, 110, 197, 153, 148, 106, 156, 247, 190, 148, 146, 187, 128, 181, 145, 144 }, 1, "okur1" },
                    { 5, "Ali", "Can", new byte[] { 137, 158, 165, 199, 11, 117, 125, 202, 100, 132, 120, 19, 145, 150, 101, 144, 127, 228, 122, 112, 170, 196, 169, 126, 116, 181, 146, 183, 179, 151, 196, 118, 135, 235, 166, 227, 172, 143, 120, 122, 151, 191, 158, 123, 118, 171, 193, 132, 10, 188, 167, 106, 142, 136, 173, 149, 197, 184, 110, 195, 169, 175, 141, 115 }, new byte[] { 112, 15, 203, 179, 125, 186, 183, 189, 205, 119, 165, 183, 185, 214, 109, 130, 14, 105, 139, 164, 197, 153, 143, 194, 108, 196, 133, 127, 120, 173, 150, 101, 188, 154, 188, 11, 166, 162, 11, 104, 142, 177, 134, 167, 135, 118, 158, 116, 107, 110, 197, 153, 148, 106, 156, 247, 190, 148, 146, 187, 128, 181, 145, 144 }, 1, "okur2" },
                    { 6, "Admin", "Admin", new byte[] { 179, 216, 171, 193, 165, 133, 133, 206, 149, 196, 175, 245, 193, 113, 198, 249, 166, 170, 119, 254, 136, 17, 166, 118, 143, 159, 102, 115, 130, 171, 241, 15, 148, 188, 121, 179, 191, 231, 103, 234, 191, 197, 168, 169, 143, 187, 127, 107, 172, 205, 242, 183, 110, 120, 131, 145, 158, 149, 135, 161, 11, 127, 109, 141 }, new byte[] { 185, 130, 16, 160, 161, 194, 238, 161, 146, 171, 139, 132, 188, 151, 164, 131, 183, 179, 149, 231, 18, 139, 11, 236, 206, 146, 246, 178, 207, 148, 19, 138, 119, 11, 158, 242, 166, 192, 149, 166, 181, 24, 10, 195, 210, 175, 231, 166, 139, 152, 131, 166, 18, 147, 168, 13, 159, 108, 125, 130, 178, 167, 140 }, 2, "admin" }
                });

            migrationBuilder.InsertData(
                table: "Blogs",
                columns: new[] { "Id", "CategoryId", "Content", "ImageUrl", "IsDeleted", "PublishDate", "Title", "Type", "UserId" },
                values: new object[,]
                {
                    { 1, 1, "Teknoloji dünyasındaki son gelişmeler...", null, false, new DateTime(2025, 4, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "İlk Teknoloji Blogu", 0, 1 },
                    { 2, 2, "Paris'te gezilecek yerler ve deneyimler...", null, false, new DateTime(2025, 4, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "Seyahat Notları: Paris", 0, 2 },
                    { 3, 3, "Evde kolayca yapabileceğiniz harika tarifler...", null, false, new DateTime(2025, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "En Lezzetli Yemek Tarifleri", 0, 1 },
                    { 4, 4, "Son moda trendleri ve kombin önerileri...", null, false, new DateTime(2025, 3, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Yeni Moda Akımları", 0, 2 },
                    { 5, 5, "Sağlıklı beslenme ve egzersiz ipuçları...", null, false, new DateTime(2025, 4, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sağlıklı Yaşam Sırları", 0, 3 },
                    { 6, 6, "Son spor olayları ve gelişmeler...", null, false, new DateTime(2025, 4, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "Spor Dünyasından Haberler", 0, 1 },
                    { 7, 7, "Otomobil sektöründeki son yenilikler...", null, false, new DateTime(2025, 3, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "En Yeni Otomobil Modelleri", 0, 2 },
                    { 8, 8, "Sinema dünyasından en iyi yapımlar...", null, false, new DateTime(2025, 4, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kaçırılmaması Gereken Filmler", 0, 4 },
                    { 9, 9, "Farklı türlerde müzik zevkine hitap eden listeler...", null, false, new DateTime(2025, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "Müzik Listeleri ve Öneriler", 0, 5 },
                    { 10, 10, "Edebiyat dünyasından seçkin eserler...", null, false, new DateTime(2025, 4, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "Okunması Gereken Kitaplar", 0, 1 }
                });

            migrationBuilder.InsertData(
                table: "Comments",
                columns: new[] { "Id", "BlogId", "CreatedDate", "ProfanityStatus", "Text", "UserId" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2025, 4, 5, 9, 56, 0, 0, DateTimeKind.Unspecified), 0, "Bu teknoloji yazısı çok bilgilendiriciydi!", 2 },
                    { 2, 1, new DateTime(2025, 4, 5, 11, 56, 0, 0, DateTimeKind.Unspecified), 0, "Katılıyorum, teşekkürler!", 1 },
                    { 3, 2, new DateTime(2025, 4, 10, 9, 56, 0, 0, DateTimeKind.Unspecified), 0, "Paris'e gitmek istiyorum, harika fotoğraflar!", 1 },
                    { 4, 2, new DateTime(2025, 4, 10, 12, 56, 0, 0, DateTimeKind.Unspecified), 0, "Ben de gitmek istiyorum!", 3 },
                    { 5, 3, new DateTime(2025, 4, 13, 9, 56, 0, 0, DateTimeKind.Unspecified), 0, "Tarif için teşekkürler, deneyeceğim.", 4 },
                    { 6, 3, new DateTime(2025, 4, 13, 13, 56, 0, 0, DateTimeKind.Unspecified), 0, "Ellerinize sağlık çok güzel görünüyor.", 1 },
                    { 7, 4, new DateTime(2025, 4, 1, 9, 56, 0, 0, DateTimeKind.Unspecified), 0, "Moda ile ilgili daha fazla içerik bekliyoruz.", 5 },
                    { 8, 4, new DateTime(2025, 4, 1, 10, 56, 0, 0, DateTimeKind.Unspecified), 0, "Kesinlikle çok güzel kombinler.", 2 },
                    { 9, 5, new DateTime(2025, 4, 7, 9, 56, 0, 0, DateTimeKind.Unspecified), 0, "Bu sağlıklı yaşam önerileri harika!", 1 },
                    { 10, 5, new DateTime(2025, 4, 7, 11, 56, 0, 0, DateTimeKind.Unspecified), 0, "Sağlıklı beslenme çok önemli.", 3 },
                    { 11, 6, new DateTime(2025, 4, 12, 9, 56, 0, 0, DateTimeKind.Unspecified), 0, "Spor haberleri için teşekkürler.", 2 },
                    { 12, 6, new DateTime(2025, 4, 12, 14, 56, 0, 0, DateTimeKind.Unspecified), 0, "Hangi takımı destekliyorsunuz?", 4 },
                    { 13, 7, new DateTime(2025, 3, 26, 9, 56, 0, 0, DateTimeKind.Unspecified), 0, "Bu otomobil modelleri harika.", 5 },
                    { 14, 7, new DateTime(2025, 3, 26, 12, 56, 0, 0, DateTimeKind.Unspecified), 0, "Fiyatları hakkında bilgi var mı?", 1 },
                    { 15, 8, new DateTime(2025, 4, 3, 9, 56, 0, 0, DateTimeKind.Unspecified), 0, "Bu filmleri izleyeceğim.", 3 },
                    { 16, 8, new DateTime(2025, 4, 3, 10, 56, 0, 0, DateTimeKind.Unspecified), 0, "Sinema keyfi başlıyor.", 2 },
                    { 17, 9, new DateTime(2025, 4, 9, 9, 56, 0, 0, DateTimeKind.Unspecified), 0, "Müzik listeleri çok iyi.", 4 },
                    { 18, 9, new DateTime(2025, 4, 9, 11, 56, 0, 0, DateTimeKind.Unspecified), 0, "Yeni şarkılar keşfettim.", 1 },
                    { 19, 10, new DateTime(2025, 4, 14, 9, 56, 0, 0, DateTimeKind.Unspecified), 0, "Bu kitapları okumalıyım.", 5 },
                    { 20, 10, new DateTime(2025, 4, 14, 3, 56, 0, 0, DateTimeKind.Unspecified), 0, "Kitap önerisi için teşekkürler.", 2 },
                    { 21, 1, new DateTime(2025, 4, 6, 9, 56, 0, 0, DateTimeKind.Unspecified), 0, "Bu yazı çok kötüydü!", 3 },
                    { 22, 1, new DateTime(2025, 4, 6, 10, 56, 0, 0, DateTimeKind.Unspecified), 0, "Katılmıyorum, bence harikaydı.", 4 },
                    { 23, 2, new DateTime(2025, 4, 11, 9, 56, 0, 0, DateTimeKind.Unspecified), 0, "Paris pahalı bir şehir mi?", 5 },
                    { 24, 2, new DateTime(2025, 4, 11, 13, 56, 0, 0, DateTimeKind.Unspecified), 0, "Evet, biraz pahalı ama değer.", 2 },
                    { 25, 3, new DateTime(2025, 4, 14, 9, 56, 0, 0, DateTimeKind.Unspecified), 0, "Bu tarifi denedim, harika oldu!", 3 },
                    { 26, 3, new DateTime(2025, 4, 14, 7, 56, 0, 0, DateTimeKind.Unspecified), 0, "Hangi malzemeleri kullandınız?", 5 },
                    { 27, 4, new DateTime(2025, 4, 1, 9, 56, 0, 0, DateTimeKind.Unspecified), 0, "Bu moda akımı çok garip.", 1 },
                    { 28, 4, new DateTime(2025, 4, 1, 11, 56, 0, 0, DateTimeKind.Unspecified), 0, "Bence çok şık.", 4 },
                    { 29, 5, new DateTime(2025, 4, 8, 9, 56, 0, 0, DateTimeKind.Unspecified), 0, "Bu öneriler işe yarıyor mu?", 2 },
                    { 30, 5, new DateTime(2025, 4, 8, 10, 56, 0, 0, DateTimeKind.Unspecified), 0, "Evet, düzenli uyguluyorum.", 5 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Blogs_CategoryId",
                table: "Blogs",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Blogs_UserId",
                table: "Blogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_BlogId",
                table: "Comments",
                column: "BlogId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserId",
                table: "Comments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Likes_BlogId",
                table: "Likes",
                column: "BlogId");

            migrationBuilder.CreateIndex(
                name: "IX_SavedBlogs_BlogId",
                table: "SavedBlogs",
                column: "BlogId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserTypeId",
                table: "Users",
                column: "UserTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Likes");

            migrationBuilder.DropTable(
                name: "SavedBlogs");

            migrationBuilder.DropTable(
                name: "Blogs");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
