using BlogApp.Application.Interfaces.Services;
using BlogApp.Application.Services;
using BlogApp.Domain.Entities;
using BlogApp.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Infrastructure.Data.SeedData
{
    public static class DataGenerator
    {

        public static void Seed(ModelBuilder builder)
        {
            SeedRoles(builder);
            SeedCategories(builder);
            SeedUsers(builder);
            SeedBlogs(builder);
            SeedComments(builder);
        }


        private static void SeedRoles(ModelBuilder builder)
        {
            List<Role> roles = new List<Role>
            {
                new Role { Id = 1, Name="User"},
                new Role { Id = 2, Name="Admin"}
            };
            builder.Entity<Role>().HasData(roles);
        }

        private static void SeedCategories(ModelBuilder builder)
        {
            string[] categoryNames = {
                "Teknoloji", "Seyahat", "Yemek", "Moda", "Sağlık", "Spor", "Otomobil", "Sinema", "Müzik", "Kitap",
                "Bilim", "Sanat", "Tarih", "Ekonomi", "Eğitim", "Oyun", "Fotoğrafçılık", "Bahçe", "Evcil Hayvan", "Felsefe"
            };

            List<Category> categories = new List<Category>();
            for (int i = 1; i <= 20; i++)
            {
                categories.Add(new Category { Id = i, Name = categoryNames[i - 1] });
            }
            builder.Entity<Category>().HasData(categories);
        }

        private static void SeedUsers(ModelBuilder builder)
        {
            byte[] userPasswordHash = new byte[] { 137, 158, 165, 199, 11, 117, 125, 202, 100, 132, 120, 19, 145, 150, 101, 144, 127, 228, 122, 112, 170, 196, 169, 126, 116, 181, 146, 183, 179, 151, 196, 118, 135, 235, 166, 227, 172, 143, 120, 122, 151, 191, 158, 123, 118, 171, 193, 132, 10, 188, 167, 106, 142, 136, 173, 149, 197, 184, 110, 195, 169, 175, 141, 115 };
            byte[] userPasswordSalt = new byte[] { 112, 15, 203, 179, 125, 186, 183, 189, 205, 119, 165, 183, 185, 214, 109, 130, 14, 105, 139, 164, 197, 153, 143, 194, 108, 196, 133, 127, 120, 173, 150, 101, 188, 154, 188, 11, 166, 162, 11, 104, 142, 177, 134, 167, 135, 118, 158, 116, 107, 110, 197, 153, 148, 106, 156, 247, 190, 148, 146, 187, 128, 181, 145, 144 };
            List<User> users = new List<User>();

            users.Add(new User
            {
                Id = 1,
                Username = "yazar1",
                PasswordHash = userPasswordHash,
                PasswordSalt = userPasswordSalt,
                FirstName = "Ahmet",
                LastName = "Yılmaz",
                UserTypeId = 1,
            });

            users.Add(new User
            {
                Id = 2,
                Username = "yazar2",
                PasswordHash = userPasswordHash,
                PasswordSalt = userPasswordSalt,
                FirstName = "Ayşe",
                LastName = "Demir",
                UserTypeId = 1,
            });

            users.Add(new User
            {
                Id = 3,
                Username = "editor1",
                PasswordHash = userPasswordHash,
                PasswordSalt = userPasswordSalt,
                FirstName = "Mehmet",
                LastName = "Öztürk",
                UserTypeId = 1,
            });

            users.Add(new User
            {
                Id = 4,
                Username = "okur1",
                PasswordHash = userPasswordHash,
                PasswordSalt = userPasswordSalt,
                FirstName = "Zeynep",
                LastName = "Kaya",
                UserTypeId = 1,
            });

            users.Add(new User
            {
                Id = 5,
                Username = "okur2",
                PasswordHash = userPasswordHash,
                PasswordSalt = userPasswordSalt,
                FirstName = "Ali",
                LastName = "Can",
                UserTypeId = 1,
            });

            users.Add(new User
            {
                Id = 6,
                Username = "admin", //Password admin123
                PasswordHash =  new byte[] {
                    0x39, 0x1E, 0xA4, 0x9B, 0xDC, 0x41, 0x3A, 0xF9, 0x5E, 0xF9, 0x5F, 0xD4, 0x0E, 0x46, 0x6F, 0xAD,
                    0xD3, 0x73, 0x65, 0x56, 0x4C, 0xC6, 0x72, 0x53, 0x26, 0x9F, 0x1B, 0x03, 0xDB, 0x0B, 0x60, 0x88,
                    0xB3, 0x4E, 0x83, 0xCB, 0x3B, 0xB9, 0x22, 0x3A, 0x5B, 0x49, 0x8D, 0x29, 0xE6, 0xEE, 0xDD, 0xAD,
                    0xBA, 0xB8, 0x1A, 0x91, 0x0A, 0x44, 0x5A, 0xE1, 0x93, 0x6F, 0x71, 0x9F, 0xE0, 0x10, 0xBA, 0x63
                },
                PasswordSalt =  new byte[] {
                    0xFA, 0x01, 0x38, 0x52, 0x01, 0xAD, 0x47, 0xEE, 0x8D, 0xE8, 0xBE, 0x67, 0x90, 0x0E, 0xC6, 0x33,
                    0xC7, 0xBD, 0x84, 0xCF, 0xB6, 0x5F, 0x42, 0xFF, 0x6B, 0x6E, 0x90, 0x08, 0x0B, 0xB7, 0x3F, 0x7A,
                    0x43, 0x68, 0xF3, 0x4A, 0xCC, 0x24, 0x7C, 0x7A, 0x2F, 0x17, 0x7C, 0xAF, 0xAF, 0x87, 0x43, 0x6B,
                    0x0B, 0x2C, 0x98, 0x7E, 0x30, 0x8D, 0xA3, 0x7F, 0x6A, 0x68, 0x22, 0x3C, 0xC3, 0x76, 0xA8, 0x25,
                    0x9E, 0x93, 0xE4, 0x7C, 0x51, 0xEB, 0x6B, 0x75, 0x3E, 0xAB, 0x10, 0xCA, 0xDF, 0xC2, 0xA0, 0x0C,
                    0xE9, 0xBC, 0xDF, 0x18, 0xD6, 0x10, 0x4F, 0xE7, 0x39, 0x90, 0x5E, 0xC1, 0x78, 0x5A, 0xA1, 0x31,
                    0xF0, 0x2A, 0x5D, 0x41, 0x18, 0x9E, 0x55, 0x1D, 0x6F, 0xBB, 0x44, 0x06, 0xE5, 0x43, 0xDF, 0x85,
                    0x92, 0xF5, 0x6F, 0xBC, 0xC8, 0xE0, 0x35, 0x00, 0x58, 0x40, 0x96, 0x04, 0xA7, 0xC6, 0xB3, 0x16 },
                FirstName = "Admin",
                LastName = "Admin",
                UserTypeId = 2,
            });

            builder.Entity<User>().HasData(users);
        }

        private static void SeedBlogs(ModelBuilder builder)
        {
            List<Blog> blogs = new List<Blog>
            {
                new Blog
                {
                    Id = 1,
                    Title = "İlk Teknoloji Blogu",
                    Content = "Teknoloji dünyasındaki son gelişmeler...",
                    PublishDate = new DateTime(2025, 4, 4),
                    UserId = 1,
                    CategoryId = 1,
                    ImageUrl = null,
                    Type = BlogType.Normal,
                },
                new Blog
                {
                    Id = 2,
                    Title = "Seyahat Notları: Paris",
                    Content = "Paris'te gezilecek yerler ve deneyimler...",
                    PublishDate = new DateTime(2025, 4, 9),
                    UserId = 2,
                    CategoryId = 2,
                    ImageUrl = null,
                    Type = BlogType.Normal,
                },
                new Blog
                {
                    Id = 3,
                    Title = "En Lezzetli Yemek Tarifleri",
                    Content = "Evde kolayca yapabileceğiniz harika tarifler...",
                    PublishDate = new DateTime(2025, 4, 12), 
                    UserId = 1,
                    CategoryId = 3,
                    ImageUrl = null,
                    Type = BlogType.Normal,
                },
                new Blog
                {
                    Id = 4,
                    Title = "Yeni Moda Akımları",
                    Content = "Son moda trendleri ve kombin önerileri...",
                    PublishDate = new DateTime(2025, 3, 30), 
                    UserId = 2,
                    CategoryId = 4,
                    ImageUrl = null,
                    Type = BlogType.Normal,
                },
                new Blog
                {
                    Id = 5,
                    Title = "Sağlıklı Yaşam Sırları",
                    Content = "Sağlıklı beslenme ve egzersiz ipuçları...",
                    PublishDate = new DateTime(2025, 4, 6), 
                    UserId = 3,
                    CategoryId = 5,
                    ImageUrl = null,
                    Type = BlogType.Normal
                },
                new Blog
                {
                    Id = 6,
                    Title = "Spor Dünyasından Haberler",
                    Content = "Son spor olayları ve gelişmeler...",
                    PublishDate = new DateTime(2025, 4, 11), 
                    UserId = 1,
                    CategoryId = 6,
                    ImageUrl = null,
                    Type = BlogType.Normal
                },
                new Blog
                {
                    Id = 7,
                    Title = "En Yeni Otomobil Modelleri",
                    Content = "Otomobil sektöründeki son yenilikler...",
                    PublishDate = new DateTime(2025, 3, 25), 
                    UserId = 2,
                    CategoryId = 7,
                    ImageUrl = null,
                    Type = BlogType.Normal
                },
                new Blog
                {
                    Id = 8,
                    Title = "Kaçırılmaması Gereken Filmler",
                    Content = "Sinema dünyasından en iyi yapımlar...",
                    PublishDate = new DateTime(2025, 4, 2), 
                    UserId = 4,
                    CategoryId = 8,
                    ImageUrl = null,
                    Type = BlogType.Normal
                },
                new Blog
                {
                    Id = 9,
                    Title = "Müzik Listeleri ve Öneriler",
                    Content = "Farklı türlerde müzik zevkine hitap eden listeler...",
                    PublishDate = new DateTime(2025, 4, 8), 
                    UserId = 5,
                    CategoryId = 9,
                    ImageUrl = null,
                    Type = BlogType.Normal
                },
                new Blog
                {
                    Id = 10,
                    Title = "Okunması Gereken Kitaplar",
                    Content = "Edebiyat dünyasından seçkin eserler...",
                    PublishDate = new DateTime(2025, 4, 13), 
                    UserId = 1,
                    CategoryId = 10,
                    ImageUrl = null,
                    Type = BlogType.Normal
                }
            };
            builder.Entity<Blog>().HasData(blogs);
        }

        private static void SeedComments(ModelBuilder builder)
        {
            List<Comment> comments = new List<Comment>
    {
        new Comment { Id = 1, BlogId = 1, UserId = 2, Text = "Bu teknoloji yazısı çok bilgilendiriciydi!", CreatedDate = new DateTime(2025, 4, 5, 9, 56, 0), ProfanityStatus = CommentProfanityStatus.Clean },
        new Comment { Id = 2, BlogId = 1, UserId = 1, Text = "Katılıyorum, teşekkürler!", CreatedDate = new DateTime(2025, 4, 5, 11, 56, 0), ProfanityStatus = CommentProfanityStatus.Clean },
        new Comment { Id = 3, BlogId = 2, UserId = 1, Text = "Paris'e gitmek istiyorum, harika fotoğraflar!", CreatedDate = new DateTime(2025, 4, 10, 9, 56, 0), ProfanityStatus = CommentProfanityStatus.Clean },
        new Comment { Id = 4, BlogId = 2, UserId = 3, Text = "Ben de gitmek istiyorum!", CreatedDate = new DateTime(2025, 4, 10, 12, 56, 0), ProfanityStatus = CommentProfanityStatus.Clean},
        new Comment{Id=5, BlogId=3, UserId=4, Text="Tarif için teşekkürler, deneyeceğim.", CreatedDate= new DateTime(2025, 4, 13, 9, 56, 0), ProfanityStatus=CommentProfanityStatus.Clean},
        new Comment{Id=6, BlogId=3, UserId=1, Text="Ellerinize sağlık çok güzel görünüyor.", CreatedDate= new DateTime(2025, 4, 13, 13, 56, 0), ProfanityStatus=CommentProfanityStatus.Clean},
        new Comment{Id=7, BlogId=4, UserId=5, Text="Moda ile ilgili daha fazla içerik bekliyoruz.", CreatedDate= new DateTime(2025, 4, 1, 9, 56, 0), ProfanityStatus=CommentProfanityStatus.Clean},
        new Comment{Id=8, BlogId=4, UserId=2, Text="Kesinlikle çok güzel kombinler.", CreatedDate= new DateTime(2025, 4, 1, 10, 56, 0), ProfanityStatus=CommentProfanityStatus.Clean},
        new Comment{Id=9, BlogId=5, UserId=1, Text="Bu sağlıklı yaşam önerileri harika!", CreatedDate= new DateTime(2025, 4, 7, 9, 56, 0), ProfanityStatus=CommentProfanityStatus.Clean},
        new Comment{Id=10, BlogId=5, UserId=3, Text="Sağlıklı beslenme çok önemli.", CreatedDate= new DateTime(2025, 4, 7, 11, 56, 0), ProfanityStatus=CommentProfanityStatus.Clean},
        new Comment{Id=11, BlogId=6, UserId=2, Text="Spor haberleri için teşekkürler.", CreatedDate= new DateTime(2025, 4, 12, 9, 56, 0), ProfanityStatus=CommentProfanityStatus.Clean},
        new Comment{Id=12, BlogId=6, UserId=4, Text="Hangi takımı destekliyorsunuz?", CreatedDate= new DateTime(2025, 4, 12, 14, 56, 0), ProfanityStatus=CommentProfanityStatus.Clean},
        new Comment{Id=13, BlogId=7, UserId=5, Text="Bu otomobil modelleri harika.", CreatedDate= new DateTime(2025, 3, 26, 9, 56, 0), ProfanityStatus=CommentProfanityStatus.Clean},
        new Comment{Id=14, BlogId=7, UserId=1, Text="Fiyatları hakkında bilgi var mı?", CreatedDate= new DateTime(2025, 3, 26, 12, 56, 0), ProfanityStatus=CommentProfanityStatus.Clean},
        new Comment{Id=15, BlogId=8, UserId=3, Text="Bu filmleri izleyeceğim.", CreatedDate= new DateTime(2025, 4, 3, 9, 56, 0), ProfanityStatus=CommentProfanityStatus.Clean},
        new Comment{Id=16, BlogId=8, UserId=2, Text="Sinema keyfi başlıyor.", CreatedDate= new DateTime(2025, 4, 3, 10, 56, 0), ProfanityStatus=CommentProfanityStatus.Clean},
        new Comment{Id=17, BlogId=9, UserId=4, Text="Müzik listeleri çok iyi.", CreatedDate= new DateTime(2025, 4, 9, 9, 56, 0), ProfanityStatus=CommentProfanityStatus.Clean},
        new Comment{Id=18, BlogId=9, UserId=1, Text="Yeni şarkılar keşfettim.", CreatedDate= new DateTime(2025, 4, 9, 11, 56, 0), ProfanityStatus=CommentProfanityStatus.Clean},
        new Comment{Id=19, BlogId=10, UserId=5, Text="Bu kitapları okumalıyım.", CreatedDate= new DateTime(2025, 4, 14, 9, 56, 0), ProfanityStatus=CommentProfanityStatus.Clean},
        new Comment{Id=20, BlogId=10, UserId=2, Text="Kitap önerisi için teşekkürler.", CreatedDate= new DateTime(2025, 4, 14, 3, 56, 0), ProfanityStatus=CommentProfanityStatus.Clean},
        new Comment { Id = 21, BlogId = 1, UserId = 3, Text = "Bu yazı çok kötüydü!", CreatedDate = new DateTime(2025, 4, 6, 9, 56, 0), ProfanityStatus = CommentProfanityStatus.Clean },
        new Comment { Id = 22, BlogId = 1, UserId = 4, Text = "Katılmıyorum, bence harikaydı.", CreatedDate = new DateTime(2025, 4, 6, 10, 56, 0), ProfanityStatus = CommentProfanityStatus.Clean },
        new Comment { Id = 23, BlogId = 2, UserId = 5, Text = "Paris pahalı bir şehir mi?", CreatedDate = new DateTime(2025, 4, 11, 9, 56, 0), ProfanityStatus = CommentProfanityStatus.Clean },
        new Comment { Id = 24, BlogId = 2, UserId = 2, Text = "Evet, biraz pahalı ama değer.", CreatedDate = new DateTime(2025, 4, 11, 13, 56, 0), ProfanityStatus = CommentProfanityStatus.Clean },
        new Comment { Id = 25, BlogId = 3, UserId = 3, Text = "Bu tarifi denedim, harika oldu!", CreatedDate = new DateTime(2025, 4, 14, 9, 56, 0), ProfanityStatus = CommentProfanityStatus.Clean },
        new Comment { Id = 26, BlogId = 3, UserId = 5, Text = "Hangi malzemeleri kullandınız?", CreatedDate = new DateTime(2025, 4, 14, 7, 56, 0), ProfanityStatus = CommentProfanityStatus.Clean },
        new Comment { Id = 27, BlogId = 4, UserId = 1, Text = "Bu moda akımı çok garip.", CreatedDate = new DateTime(2025, 4, 1, 9, 56, 0), ProfanityStatus = CommentProfanityStatus.Clean },
        new Comment { Id = 28, BlogId = 4, UserId = 4, Text = "Bence çok şık.", CreatedDate = new DateTime(2025, 4, 1, 11, 56, 0), ProfanityStatus = CommentProfanityStatus.Clean },
        new Comment { Id = 29, BlogId = 5, UserId = 2, Text = "Bu öneriler işe yarıyor mu?", CreatedDate = new DateTime(2025, 4, 8, 9, 56, 0), ProfanityStatus = CommentProfanityStatus.Clean },
        new Comment { Id = 30, BlogId = 5, UserId = 5, Text = "Evet, düzenli uyguluyorum.", CreatedDate = new DateTime(2025, 4, 8, 10, 56, 0), ProfanityStatus = CommentProfanityStatus.Clean }
    };
            builder.Entity<Comment>().HasData(comments);
        }
    }
}
