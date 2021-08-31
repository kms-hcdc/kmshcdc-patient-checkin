using log4net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace PatientChecking.Services.Image
{
    public class ImageService : IImageService
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

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

        public async Task<string> SaveImage(IFormFile formFile)
        {
            try
            {
                string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "Image");
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(formFile.FileName);

                var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                await formFile.CopyToAsync(new FileStream(filePath, FileMode.Create));

                var avatarLink = "/Image/" + uniqueFileName;

                return avatarLink;
            }
            catch (DirectoryNotFoundException ex)
            {
                _log.Error(ex.Message);
                throw;
            }
        }
    }
}
