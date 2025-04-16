using BlogApp.Application.Interfaces.Repositories;
using BlogApp.Application.Interfaces.Services;
using BlogApp.Application.Models;
using BlogApp.Domain.Entities;
using BlogApp.Domain.Enums;
using BlogApp.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Application.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IOpenAIService _openAIService;

        public CommentService(ICommentRepository commentRepository,IOpenAIService openAIService)
        {
            _commentRepository = commentRepository;
            _openAIService = openAIService;

        }
        public async Task<List<CommentModel>> GetCommentModelsByBlogId(int blogId)
        {
            var comments = await _commentRepository.GetCommentsByBlogIdAsync(blogId);
            var commentModels = comments.Select(c => new CommentModel
            {
                Id = c.Id,
                FullName = c.User.FirstName + " " + c.User.LastName,
                Text = c.Text,
                CreatedDate = c.CreatedDate,
                ProfanityStatus = c.ProfanityStatus,
            }).ToList();
            return commentModels;
        }

        public async Task<bool> AddCommentAsync(CommentAddModel commentAdd) {
            var comment = new Comment
            {
                BlogId = commentAdd.BlogId,
                UserId = commentAdd.UserId,
                CreatedDate = DateTime.UtcNow,
                Text = commentAdd.Text,
                ProfanityStatus = await _openAIService.ContainsProfanityAsync(commentAdd.Text),
            };
            try
            {
                await _commentRepository.AddCommentAsync(comment);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
    }
}
