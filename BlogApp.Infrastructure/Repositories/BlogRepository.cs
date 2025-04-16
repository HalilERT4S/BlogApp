using BlogApp.Application.Interfaces.Repositories;
using BlogApp.Application.Models;
using BlogApp.Domain.Entities;
using BlogApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Infrastructure.Repositories
{
    public class BlogRepository : IBlogRepository
    {
        private readonly ApplicationDbContext _context;

        public BlogRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddBlogAsync(Blog blog)
        {
            await _context.Blogs.AddAsync(blog);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateBlogAsync(Blog blog)
        {
            _context.Blogs.Update(blog);
            await _context.SaveChangesAsync();
        }

        public async Task<Blog> GetBlogById(int blogId)
        {
            var blog = await _context.Blogs.FindAsync(blogId);
            return blog;
        }

        public async Task<List<Blog>> GetBlogListWithAuthorAndCategoryAsync()
        {
            return await _context.Blogs
            .Where(b => !b.IsDeleted)
            .Include(b => b.Category)
            .Include(b => b.User)
            .OrderByDescending(b => b.PublishDate)
            .ToListAsync();
        }

        public async Task<Blog> GetBlogbyIdWithAuthorAndCategoryAsync(int blogId)
        {
            return await _context.Blogs
            .Where(b => b.Id == blogId)
            .Include(b => b.Category)
            .Include(b => b.User)
            .FirstOrDefaultAsync();
        }

        public async Task<List<Blog>> GetBlogsByCategoryIdAsync(int categoryId)
        {
            return await _context.Blogs
                .Where(b => b.CategoryId == categoryId && !b.IsDeleted)
                .Include(b => b.Category)
                .Include(b => b.User)
                .OrderByDescending(b => b.PublishDate)
                .ToListAsync();
        }

        public async Task<List<Blog>> GetBlogsByUserIdAsync(int userId)
        {
            return await _context.Blogs
                .Where(b => b.UserId == userId && !b.IsDeleted)
                .Include(b => b.Category)
                .Include(b => b.User)
                .ToListAsync();
        }

        public async Task DeleteBlogAsync(int blogId)
        {
            var blog = await GetBlogById(blogId);
            blog.IsDeleted = true;
            await UpdateBlogAsync(blog);
        }
    }
}
