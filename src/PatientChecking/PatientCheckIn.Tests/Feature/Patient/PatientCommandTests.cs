using Microsoft.AspNetCore.Http;
using Moq;
using PatientChecking.Feature.Patient.Commands;
using PatientChecking.ServiceModels.Enum;
using PatientChecking.Services.Image;
using PatientChecking.Services.Patient;
using PatientChecking.Views.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace PatientCheckIn.Tests.Feature.Patient
{
    public class PatientCommandTests
    {
        [Fact]
        public async void UpdatePatientInformationCommandTest_Ok()
        {
            //Arrange
            var model = new PatientDetailViewModel
            {
                PatientId = 1,
                FirstName = "Harry",
                MiddleName = "James",
                LastName = "Potter",
                FullName = "Harry James Potter",
                DoB = "1990-07-31",
                Gender = 0,
                MaritalStatus = 1,
                Nationality = "American",
                EthnicRace = null,
                HomeTown = "Los Angeles",
                BirthplaceCity = "San Diego",
                IdcardNo = "201863654",
                IssuedDate = "2013-08-12",
                IssuedPlace = "Los Angeles",
                InsuranceNo = "200753645",
            };

            var data = new DataAccess.Models.Patient
            {
                PatientId = 1,
                FirstName = "Long",
                MiddleName = "Thanh",
                LastName = "Do",
                FullName = "Long Thanh Do",
                DoB = new DateTime(1999, 11, 09),
                Gender = 0,
                MaritalStatus = false,
                Nationality = "Vietnamese",
                EthnicRace = "Kinh",
                HomeTown = "Da Nang",
                BirthplaceCity = "Ho Chi Minh",
                IdcardNo = "201754622",
                IssuedDate = new DateTime(2014, 09, 14),
                IssuedPlace = "Da Nang",
                InsuranceNo = "201329231"
            };

            var mockPatientService = new Mock<IPatientService>();

            mockPatientService.Setup(x => x.GetPatientInDetail(model.PatientId)).ReturnsAsync(data);

            mockPatientService.Setup(x => x.UpdatePatientDetail(data)).ReturnsAsync(1);

            var command = new UpdatePatientInformationCommand() { PatientModel = model };
            var handler = new UpdatePatientInformationCommandHandler(mockPatientService.Object);

            var expected = 1;

            //Act
            var actual = await handler.Handle(command, new CancellationToken());

            //Assert
            Assert.True(actual != -1);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async void UpdatePatientInformationCommandTest_NotFoundPatient()
        {
            //Arrange
            var model = new PatientDetailViewModel
            {
                PatientId = -1
            };

            var mockPatientService = new Mock<IPatientService>();

            mockPatientService.Setup(x => x.GetPatientInDetail(model.PatientId)).ReturnsAsync((DataAccess.Models.Patient)null);

            var command = new UpdatePatientInformationCommand() { PatientModel = model };
            var handler = new UpdatePatientInformationCommandHandler(mockPatientService.Object);

            var expected = -1;

            //Act
            var actual = await handler.Handle(command, new CancellationToken());

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async void UpdatePatientInformationCommandTest_Exception()
        {
            //Arrange
            var model = new PatientDetailViewModel
            {
                PatientId = 1,
                FirstName = "Harry",
                MiddleName = "James",
                LastName = "Potter",
                FullName = "Harry James Potter",
                DoB = "DoB", //cause exception
                Gender = 0,
                MaritalStatus = 1,
                Nationality = "American",
                EthnicRace = null,
                HomeTown = "Los Angeles",
                BirthplaceCity = "San Diego",
                IdcardNo = "201863654",
                IssuedDate = "2013-08-12",
                IssuedPlace = "Los Angeles",
                InsuranceNo = "200753645",
            };

            var data = new DataAccess.Models.Patient
            {
                PatientId = 1,
                FirstName = "Long",
                MiddleName = "Thanh",
                LastName = "Do",
                FullName = "Long Thanh Do",
                DoB = new DateTime(1999, 11, 09),
                Gender = 0,
                MaritalStatus = false,
                Nationality = "Vietnamese",
                EthnicRace = "Kinh",
                HomeTown = "Da Nang",
                BirthplaceCity = "Ho Chi Minh",
                IdcardNo = "201754622",
                IssuedDate = new DateTime(2014, 09, 14),
                IssuedPlace = "Da Nang",
                InsuranceNo = "201329231"
            };

            var mockPatientService = new Mock<IPatientService>();

            mockPatientService.Setup(x => x.GetPatientInDetail(model.PatientId)).ReturnsAsync(data);

            var command = new UpdatePatientInformationCommand() { PatientModel = model };
            var handler = new UpdatePatientInformationCommandHandler(mockPatientService.Object);

            var expected = -1;

            //Act
            var actual = await handler.Handle(command, new CancellationToken());

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async void UploadPatientImageCommandTest_UploadSuccessfully()
        {
            //Arrange
            var patientId = 1;
            var avatarLink = "/Image/ed87619a-dfbf-4794-9cbb-bcecab7b44f3_avatar.jpg";
            var image = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a mock IFormFile")), 0, 0, "Data", "avatar.jpg");

            var mockImageService = new Mock<IImageService>();
            mockImageService.Setup(x => x.IsImageFile(image)).Returns(true);
            mockImageService.Setup(x => x.SaveImage(image)).Returns(avatarLink);

            var mockPatientService = new Mock<IPatientService>();
            mockPatientService.Setup(x => x.UploadPatientImage(patientId, avatarLink)).ReturnsAsync(1);

            var command = new UploadPatientImageCommand() { PatientId = patientId, FormFile = image };
            var handler = new UploadPatientImageCommandHandler(mockPatientService.Object, mockImageService.Object); 

            var expected = ImageUploadingStatus.UploadImageSuccessfully;

            //Act
            var actual = await handler.Handle(command, new CancellationToken());

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async void UploadPatientImageCommandTest_IsNotImageFailed()
        {
            //Arrange
            var patientId = 1;
            var image = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a mock IFormFile")), 0, 0, "Data", "avatar.pdf");

            var mockImageService = new Mock<IImageService>();
            mockImageService.Setup(x => x.IsImageFile(image)).Returns(false);

            var mockPatientService = new Mock<IPatientService>();

            var command = new UploadPatientImageCommand() { PatientId = patientId, FormFile = image };
            var handler = new UploadPatientImageCommandHandler(mockPatientService.Object, mockImageService.Object);

            var expected = ImageUploadingStatus.IsNotImageFailed;

            //Act
            var actual = await handler.Handle(command, new CancellationToken());

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async void UploadPatientImageCommandTest_UploadFailed()
        {
            //Arrange
            var patientId = -1;
            var avatarLink = "/Image/ed87619a-dfbf-4794-9cbb-bcecab7b44f3_avatar.jpg";
            var image = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a mock IFormFile")), 0, 0, "Data", "avatar.jpg");

            var mockImageService = new Mock<IImageService>();
            mockImageService.Setup(x => x.IsImageFile(image)).Returns(true);
            mockImageService.Setup(x => x.SaveImage(image)).Returns(avatarLink);

            var mockPatientService = new Mock<IPatientService>();
            mockPatientService.Setup(x => x.UploadPatientImage(patientId, avatarLink)).ReturnsAsync(-1);

            var command = new UploadPatientImageCommand() { PatientId = patientId, FormFile = image };
            var handler = new UploadPatientImageCommandHandler(mockPatientService.Object, mockImageService.Object);

            var expected = ImageUploadingStatus.UploadImageFailed;

            //Act
            var actual = await handler.Handle(command, new CancellationToken());

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async void UploadPatientImageCommandTest_ImageNull()
        {
            //Arrange
            var patientId = 1;

            var mockImageService = new Mock<IImageService>();

            var mockPatientService = new Mock<IPatientService>();

            var command = new UploadPatientImageCommand() { PatientId = patientId, FormFile = null };
            var handler = new UploadPatientImageCommandHandler(mockPatientService.Object, mockImageService.Object);

            var expected = ImageUploadingStatus.UploadImageFailed;

            //Act
            var actual = await handler.Handle(command, new CancellationToken());

            //Assert
            Assert.Equal(expected, actual);
        }
    }
}
