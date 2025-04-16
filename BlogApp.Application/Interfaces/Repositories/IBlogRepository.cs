using BlogApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Application.Interfaces.Repositories
{
    public interface IBlogRepository
    {
        Task AddBlogAsync(Blog blog);

        Task UpdateBlogAsync(Blog blog);

        Task DeleteBlogAsync(int blogId);

        Task<Blog> GetBlogById(int blogId);

        Task<Blog> GetBlogbyIdWithAuthorAndCategoryAsync(int blogId);

        Task<List<Blog>> GetBlogListWithAuthorAndCategoryAsync();

        Task<List<Blog>> GetBlogsByCategoryIdAsync(int categoryId);

        Task<List<Blog>> GetBlogsByUserIdAsync(int userId);
    }
}
