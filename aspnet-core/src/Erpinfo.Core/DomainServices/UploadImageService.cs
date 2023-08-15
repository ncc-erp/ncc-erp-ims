using Abp.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Erpinfo.DomainServices
{
    public class UploadImageService : BaseDomainService, IUploadImage
    {
        private IHostingEnvironment _hostingEnvironment;

        public UploadImageService(IHostingEnvironment environment)
        {
            _hostingEnvironment = environment;
        }
        public async Task<string> UploadImage(IFormFile file)
        {
            try
            {
                var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "image");
                if (file != null && file.Length > 0)
                {
                    var fileName = DateTimeOffset.Now.ToUnixTimeMilliseconds() + "_" + file.FileName;
                    var filePath = Path.Combine(uploads, fileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                    using (var fileSubStream = File.OpenRead(filePath))
                    {
                        return $"/image/{fileName}";
                    }
                }
                return null;
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public string CreateImageFromBase64(string base64)
        {
            try
            {
                if (base64 == null || base64 == "")
                {
                    return null;
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
                    return null;
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
            catch(Exception ex)
            {
                return null;
            }
        }
    }
}
