using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Application.Models
{
    public class CommentAddModel
    {
        public int BlogId { get; set; }
        public int UserId { get; set; }
        public string Text {  get; set; }
    }
}
