using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using PatientChecking.Services.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PatientChecking.Services
{
    public class ImageService : IImageService
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public ImageService(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public bool IsImageFile(IFormFile formFile)
        {
            var fileExtension = Path.GetExtension(formFile.FileName).ToLower();

            if (fileExtension != ".jpg" && fileExtension != ".png" && fileExtension != ".jpeg")
            {
                return false;
            }

            return true;
        }

        public string SaveImage(IFormFile formFile)
        {
            try
            {
                string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "Image");
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(formFile.FileName);

                var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                formFile.CopyTo(new FileStream(filePath, FileMode.Create));

                var avatarLink = "/Image/" + uniqueFileName;

                return avatarLink;
            }
            catch (IOException)
            {
                throw new IOException("Cannot save image to server");
            }
        }
    }
}
