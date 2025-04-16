using BlogApp.Domain.Entities;
using BlogApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Application.Models
{
    public class CommentModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Text { get; set; }
        public DateTime CreatedDate { get; set; }
        public CommentProfanityStatus ProfanityStatus { get; set; }

    }
}
