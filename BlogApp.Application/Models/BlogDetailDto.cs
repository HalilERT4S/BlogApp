using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Application.Models
{
    public class BlogDetailDto
    {
        public BlogModel Blog { get; set; }

        public List<CommentModel> Comments { get; set; }
    }
}
