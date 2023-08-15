using Erpinfo.DomainServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Erpinfo.Image
{
    public class ImageAppService: ErpinfoAppServiceBase
    {
        private IHostingEnvironment _hostingEnvironment;
        private readonly UploadImageService _imageService;

        public ImageAppService(IHostingEnvironment environment, UploadImageService imageService)
        {
            _hostingEnvironment = environment;
            _imageService = imageService;
        }
        public async Task<string> UploadImage(IFormFile file)
        {
            var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "image");
            if (file.Length > 0)
            {
                var filePath = Path.Combine(uploads, file.FileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
                using (var fileSubStream = File.OpenRead(filePath))
                {
                    return $"/image/{file.FileName}";
                }
            }
            return "error";
        }

        public string CreateImageFromBase64([FromForm] string base64)
        {
            if (base64 == null || base64 == "")
            {
                return "error";
            }
            string validChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new Random();
            int size = 8;
            char[] chars = new char[size];
            for (int i = 0; i < size; i++)
            {
                chars[i] = validChars[random.Next(0, validChars.Length)];
            }

            byte[] imageBytes = Convert.FromBase64String(base64);
            MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);

            System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
            if (image.Size.IsEmpty)
            {
                return "error";
            }
            var tempPath = Path.Combine(_hostingEnvironment.WebRootPath, "image");

            var fileName = string.Format(@"{0}_{1}.png", new string(chars), DateTime.Now.Ticks);
            if (!Directory.Exists(tempPath))
            {
                Directory.CreateDirectory(tempPath);
            }
            image.Save(Path.Combine(tempPath, fileName), ImageFormat.Png);
            return $"/image/{fileName}";
        }
    }
}
