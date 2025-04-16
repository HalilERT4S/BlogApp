using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BlogApp.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? UserTypeId { get; set; } 
        public Role? UserType { get; set; }
        public List<Blog> Blogs { get; set; } = new List<Blog>();
        public List<Comment> Comments { get; set; } = new List<Comment>();
        public List<Like> Likes { get; set; } = new List<Like>();
        public List<SavedBlog> SavedBlogs { get; set; } = new List<SavedBlog>();
    }
}
