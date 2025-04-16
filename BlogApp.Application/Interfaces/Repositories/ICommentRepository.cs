using BlogApp.Application.Models;
using BlogApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Application.Interfaces.Repositories
{
    public interface ICommentRepository
    {
        Task<List<Comment>> GetCommentsByBlogIdAsync(int blogId);

        Task AddCommentAsync(Comment comment); 
    }
}
