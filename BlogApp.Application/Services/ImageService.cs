using BlogApp.Application.Interfaces.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Application.Services
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly string _imageFolderPath;

        public ImageService(IWebHostEnvironment environment)
        {
            _environment = environment;
            _imageFolderPath = Path.Combine(_environment.WebRootPath, "images");
            
            if (!Directory.Exists(_imageFolderPath))
            {
                Directory.CreateDirectory(_imageFolderPath);
            }
        }

        public async Task<string> UploadImageAsync(IFormFile imageFile)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                var filePath = Path.Combine(_imageFolderPath, fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }

                return "/images/" + fileName; 
            }

            return null; 
        }

        public async Task<bool> DeleteImageAsync(string imageUrl)
        {
            if (!string.IsNullOrEmpty(imageUrl))
            {
                var fileName = Path.GetFileName(imageUrl);
                var filePath = Path.Combine(_imageFolderPath, fileName);

                if (File.Exists(filePath))
                {
                    try
                    {
                        File.Delete(filePath);
                        return true;
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }
            }
            return false;
        }

    }
}
