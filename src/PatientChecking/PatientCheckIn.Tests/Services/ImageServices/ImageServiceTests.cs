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
        public static bool IsGuid(string value)
        {
            Guid x;
            return Guid.TryParse(value, out x);
        }

        [Fact]
        public void IsImageFile_Ok()
        {
            // Arrange.
            var file = new Mock<IFormFile>();
            var sourceImg = File.OpenRead(@"D:\Longtdo\avatar.jpg");
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(sourceImg);
            writer.Flush();
            ms.Position = 0;
            var fileName = "avatar.jpg";
            file.Setup(f => f.FileName).Returns(fileName).Verifiable();
            file.Setup(_ => _.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
                .Returns((Stream stream, CancellationToken token) => ms.CopyToAsync(stream))
                .Verifiable();
            var inputFile = file.Object;

            var mockEnvironment = new Mock<IHostingEnvironment>();
            //...Setup the mock as needed
            mockEnvironment
                .Setup(m => m.WebRootPath)
                .Returns("D:\\Longtdo\\PatientCheckIn\\kmshcdc-patient-checking\\src\\PatientChecking\\PatientChecking\\wwwroot");

            var expected = true;

            //Act
            var imageService = new ImageService(mockEnvironment.Object);
            var actual = imageService.IsImageFile(inputFile);

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void IsImageFile_NotOk()
        {
            // Arrange.
            var file = new Mock<IFormFile>();
            var sourceImg = File.OpenRead(@"D:\Longtdo\someFile.pdf");
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(sourceImg);
            writer.Flush();
            ms.Position = 0;
            var fileName = "someFile.pdf";
            file.Setup(f => f.FileName).Returns(fileName).Verifiable();
            file.Setup(_ => _.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
                .Returns((Stream stream, CancellationToken token) => ms.CopyToAsync(stream))
                .Verifiable();
            var inputFile = file.Object;

            var mockEnvironment = new Mock<IHostingEnvironment>();
            //...Setup the mock as needed
            mockEnvironment
                .Setup(m => m.WebRootPath)
                .Returns("D:\\Longtdo\\PatientCheckIn\\kmshcdc-patient-checking\\src\\PatientChecking\\PatientChecking\\wwwroot");

            var expected = false;

            //Act
            var imageService = new ImageService(mockEnvironment.Object);
            var actual = imageService.IsImageFile(inputFile);

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SaveImage_Ok()
        {
            // Arrange.
            var file = new Mock<IFormFile>();
            var sourceImg = File.OpenRead(@"D:\Longtdo\avatar.jpg");
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(sourceImg);
            writer.Flush();
            ms.Position = 0;
            var fileName = "avatar.jpg";
            file.Setup(f => f.FileName).Returns(fileName).Verifiable();
            file.Setup(_ => _.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
                .Returns((Stream stream, CancellationToken token) => ms.CopyToAsync(stream))
                .Verifiable();
            var inputFile = file.Object;

            var mockEnvironment = new Mock<IHostingEnvironment>();
            //...Setup the mock as needed
            mockEnvironment
                .Setup(m => m.WebRootPath)
                .Returns("D:\\Longtdo\\PatientCheckIn\\kmshcdc-patient-checking\\src\\PatientChecking\\PatientChecking\\wwwroot");

            //Act
            var imageService = new ImageService(mockEnvironment.Object);
            var actual = imageService.SaveImage(inputFile);
            var guid = actual.Substring(7, 36);

            //Assert
            Assert.Contains(inputFile.FileName, actual);
            Assert.Contains("/Image/", actual);
            Assert.True(IsGuid(guid));
        }

        [Fact]
        public void SaveImage_NotOk()
        {
            // Arrange.
            var file = new Mock<IFormFile>();
            var sourceImg = File.OpenRead(@"D:\Longtdo\avatar.jpg");
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(sourceImg);
            writer.Flush();
            ms.Position = 0;
            var fileName = "avatar.jpg";
            file.Setup(f => f.FileName).Returns(fileName).Verifiable();
            file.Setup(_ => _.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
                .Returns((Stream stream, CancellationToken token) => ms.CopyToAsync(stream))
                .Verifiable();
            var inputFile = file.Object;

            var mockEnvironment = new Mock<IHostingEnvironment>();
            //...Setup the mock as needed
            mockEnvironment
                .Setup(m => m.WebRootPath)
                .Returns("");

            //Act
            var imageService = new ImageService(mockEnvironment.Object);

            //Assert
            IOException exception = Assert.Throws<IOException>(() => imageService.SaveImage(inputFile));
            Assert.Equal("Cannot save image to server", exception.Message);
        }
    }
}
