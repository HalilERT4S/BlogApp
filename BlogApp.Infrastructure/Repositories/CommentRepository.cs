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
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CommentRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddCommentAsync(Comment comment)
        {
            await _dbContext.Comments.AddAsync(comment);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Comment>> GetCommentsByBlogIdAsync(int blogId)
        {
            return await _dbContext.Comments
            .Where(c => c.BlogId == blogId)
            .Include(c => c.User)
            .ToListAsync();
        }
    }
}
