using BlogApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Domain.Interfaces
{
    public interface IOpenAIService
    {
        Task<BlogType> IsContentAdultAsync(string content);

        Task<CommentProfanityStatus> ContainsProfanityAsync(string comment);
    }
}
