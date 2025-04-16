using BlogApp.Domain.Entities;
using BlogApp.Infrastructure.Data.SeedData;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<SavedBlog> SavedBlogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasKey(r => r.Id);
            modelBuilder.Entity<Role>().Property(r => r.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Role>().Property(r => r.Name).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<Role>().HasMany(r => r.Users).WithOne(u => u.UserType).HasForeignKey(u => u.UserTypeId).OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<User>().Property(u => u.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<User>().Property(u => u.Username).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<User>().Property(u => u.PasswordHash).IsRequired();
            modelBuilder.Entity<User>().Property(u => u.PasswordSalt).IsRequired();
            modelBuilder.Entity<User>().Property(u => u.FirstName).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<User>().Property(u => u.LastName).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<User>().HasMany(u => u.Blogs).WithOne(b => b.User).HasForeignKey(b => b.UserId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<User>().HasMany(u => u.Comments).WithOne(c => c.User).HasForeignKey(c => c.UserId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<User>().HasMany(u => u.Likes).WithOne(l => l.User).HasForeignKey(l => l.UserId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<User>().HasMany(u => u.SavedBlogs).WithOne(sb => sb.User).HasForeignKey(sb => sb.UserId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();
            modelBuilder.Entity<User>().HasOne(u => u.UserType).WithMany(r => r.Users).HasForeignKey(u => u.UserTypeId).OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Blog>().HasKey(b => b.Id);
            modelBuilder.Entity<Blog>().Property(b => b.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Blog>().Property(b => b.Title).IsRequired().HasMaxLength(200);
            modelBuilder.Entity<Blog>().Property(b => b.Content).IsRequired();
            modelBuilder.Entity<Blog>().Property(b => b.PublishDate).IsRequired();
            modelBuilder.Entity<Blog>().Property(b => b.ImageUrl).HasMaxLength(255);
            modelBuilder.Entity<Blog>().Property(b => b.IsDeleted).IsRequired();
            modelBuilder.Entity<Blog>().HasOne(b => b.User).WithMany(u => u.Blogs).HasForeignKey(b => b.UserId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Blog>().HasOne(b => b.Category).WithMany(c => c.Blogs).HasForeignKey(b => b.CategoryId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Blog>().HasMany(b => b.Comments).WithOne(c => c.Blog).HasForeignKey(c => c.BlogId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Blog>().HasMany(b => b.Likes).WithOne(l => l.Blog).HasForeignKey(l => l.BlogId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Blog>().HasMany(b => b.SavedBlogs).WithOne(sb => sb.Blog).HasForeignKey(sb => sb.BlogId).OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Category>().HasKey(c => c.Id);
            modelBuilder.Entity<Category>().Property(c => c.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Category>().Property(c => c.Name).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Category>().HasMany(c => c.Blogs).WithOne(b => b.Category).HasForeignKey(b => b.CategoryId).OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Comment>().HasKey(c => c.Id);
            modelBuilder.Entity<Comment>().Property(c => c.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Comment>().Property(c => c.Text).IsRequired();
            modelBuilder.Entity<Comment>().Property(c => c.CreatedDate).IsRequired();
            modelBuilder.Entity<Comment>().HasOne(c => c.User).WithMany(u => u.Comments).HasForeignKey(c => c.UserId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Comment>().HasOne(c => c.Blog).WithMany(b => b.Comments).HasForeignKey(c => c.BlogId).OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Like>().HasKey(l => new { l.UserId, l.BlogId });
            modelBuilder.Entity<Like>().HasOne(l => l.User).WithMany(u => u.Likes).HasForeignKey(l => l.UserId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Like>().HasOne(l => l.Blog).WithMany(b => b.Likes).HasForeignKey(l => l.BlogId).OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<SavedBlog>().HasKey(sb => new { sb.UserId, sb.BlogId });
            modelBuilder.Entity<SavedBlog>().Property(sb => sb.SavedDate).IsRequired();
            modelBuilder.Entity<SavedBlog>().HasOne(sb => sb.User).WithMany(u => u.SavedBlogs).HasForeignKey(sb => sb.UserId).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<SavedBlog>().HasOne(sb => sb.Blog).WithMany(b => b.SavedBlogs).HasForeignKey(sb => sb.BlogId).OnDelete(DeleteBehavior.Cascade);


            DataGenerator.Seed(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }
    }
}
