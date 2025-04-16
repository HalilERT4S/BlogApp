using BlogApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Application.Models
{
    public class BlogModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string AuthorName { get; set; }
        public DateTime PublishDate { get; set; }
        public string CategoryName { get; set; }
        public string? ImageUrl { get; set; }
        public BlogType BlogType { get; set; }
    }
}
