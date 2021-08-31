using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Moq;
using PatientChecking.Services.Image;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace PatientCheckIn.Tests.Services.ImageServices
{
    public class ImageServiceTests
    {
        [Fact]
        public void IsImageFile_Ok()
        {
            // Arrange.
            var fileMock = new Mock<IFormFile>();
            //Setup mock file using a memory stream
            var fileName = "avatar.jpg";
            var ms = new MemoryStream();
            ms.Position = 0;
            fileMock.Setup(x => x.OpenReadStream()).Returns(ms);
            fileMock.Setup(x => x.FileName).Returns(fileName);
            fileMock.Setup(x => x.Length).Returns(ms.Length);

            var formFile = fileMock.Object;

            var mockEnvironment = new Mock<IHostingEnvironment>();

            var expected = true;

            //Act
            var imageService = new ImageService(mockEnvironment.Object);
            var actual = imageService.IsImageFile(formFile);

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void IsImageFile_NotOk()
        {
            // Arrange.
            var fileMock = new Mock<IFormFile>();
            //Setup mock file using a memory stream
            var fileName = "doc.pdf";
            var ms = new MemoryStream();
            ms.Position = 0;
            fileMock.Setup(x => x.OpenReadStream()).Returns(ms);
            fileMock.Setup(x => x.FileName).Returns(fileName);
            fileMock.Setup(x => x.Length).Returns(ms.Length);

            var formFile = fileMock.Object;

            var mockEnvironment = new Mock<IHostingEnvironment>();

            var expected = false;

            //Act
            var imageService = new ImageService(mockEnvironment.Object);
            var actual = imageService.IsImageFile(formFile);

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async void SaveImage_Ok()
        {
            // Arrange.
            var fileMock = new Mock<IFormFile>();
            
            var mockEnvironment = new Mock<IHostingEnvironment>();

            //Setup mock file using a memory stream
            var fileName = "avatar.jpg";
            var ms = new MemoryStream();

            ms.Position = 0;
            fileMock.Setup(x => x.OpenReadStream()).Returns(ms);
            fileMock.Setup(x => x.FileName).Returns(fileName);
            fileMock.Setup(x => x.Length).Returns(ms.Length);
            fileMock.Setup(f =>  f.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(true));
            var formFile = fileMock.Object; 
            //...Setup the mock as needed
            mockEnvironment
                .Setup(m => m.WebRootPath)
                .Returns("");
            
            var mockService = new Mock<ImageService>(mockEnvironment.Object);
            mockService.Setup(x => x.FromFileStream(It.IsAny<string>())).Returns(FileStream.Null);
            var service = mockService.Object;

            //Act
            var actual = await service.SaveImage(formFile);
            var guid = actual.Substring(7, 36);

            //Assert
            Assert.Contains(formFile.FileName, actual);
            Assert.Contains("/Image/", actual);
            Assert.True(Guid.TryParse(guid, out Guid x));
        }

        [Fact]
        public async void SaveImage_NotOk()
        {
            // Arrange.
            var fileMock = new Mock<IFormFile>();
            //Setup mock file using a memory stream
            var fileName = "avatar.jpg";
            var ms = new MemoryStream();
            ms.Position = 0;
            fileMock.Setup(x => x.OpenReadStream()).Returns(ms);
            fileMock.Setup(x => x.FileName).Returns(fileName);
            fileMock.Setup(x => x.Length).Returns(ms.Length);

            var formFile = fileMock.Object;

            var mockEnvironment = new Mock<IHostingEnvironment>();
            //...Setup the mock as needed
            mockEnvironment
                .Setup(m => m.WebRootPath)
                .Returns("");

            //Act
            var imageService = new ImageService(mockEnvironment.Object);

            //Assert
            IOException exception = await Assert.ThrowsAsync<DirectoryNotFoundException>(() => imageService.SaveImage(formFile));
        }
    }
}
