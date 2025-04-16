using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Application.Interfaces.Services
{
    public interface IImageService
    {
        Task<string> UploadImageAsync(IFormFile imageFile);

        Task<bool> DeleteImageAsync(string imageUrl);
    }
}
