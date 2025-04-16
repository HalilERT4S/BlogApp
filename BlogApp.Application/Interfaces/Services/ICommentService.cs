using BlogApp.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Application.Interfaces.Services
{
    public interface ICommentService
    {
        Task<List<CommentModel>> GetCommentModelsByBlogId(int blogId);

        Task<bool> AddCommentAsync(CommentAddModel commentAdd);
    }
}
