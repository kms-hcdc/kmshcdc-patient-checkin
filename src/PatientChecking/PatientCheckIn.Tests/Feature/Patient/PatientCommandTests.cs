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
        public async void UpdatePatientInformationCommand_Ok_ReturnsNumberOfEffectedRow()
        {
            //Arrange
            var model = ModelDataTest();

            var data = GetPatientData();

            var mockPatientService = new Mock<IPatientService>();

            mockPatientService.Setup(x => x.GetPatientInDetail(model.PatientId)).ReturnsAsync(data);

            mockPatientService.Setup(x => x.UpdatePatientDetail(data)).ReturnsAsync(1);

            var command = new UpdatePatientInformationCommand() { PatientModel = model };
            var handler = new UpdatePatientInformationCommandHandler(mockPatientService.Object);

            //Act
            var actual = await handler.Handle(command, new CancellationToken());

            //Assert
            Assert.True(actual == 1);
        }

        [Fact]
        public async void UpdatePatientInformationCommand_NotFoundPatient_ReturnsNumberOfEffectedRow()
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

            //Act
            var actual = await handler.Handle(command, new CancellationToken());

            //Assert
            Assert.True(actual == -1);
        }

        [Fact]
        public async void UpdatePatientInformationCommand_FormatException_ReturnsNumberOfEffectedRow()
        {
            //Arrange
            var model = ModelDataTest();
            model.DoB = "DoB"; //Cause Exception

            var patient = GetPatientData();

            var mockPatientService = new Mock<IPatientService>();

            mockPatientService.Setup(x => x.GetPatientInDetail(model.PatientId)).ReturnsAsync(patient);

            var command = new UpdatePatientInformationCommand() { PatientModel = model };
            var handler = new UpdatePatientInformationCommandHandler(mockPatientService.Object);

            //Act
            var actual = await handler.Handle(command, new CancellationToken());

            //Assert
            Assert.True(actual == -1);
        }

        [Fact]
        public async void UploadPatientImageCommand_UploadSuccessfully_ReturnsImageUploadingStatus()
        {
            //Arrange
            var patientId = 1;
            var avatarLink = "/Image/ed87619a-dfbf-4794-9cbb-bcecab7b44f3_avatar.jpg";
            var image = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a mock IFormFile")), 0, 0, "Data", "avatar.jpg");

            var mockImageService = new Mock<IImageService>();
            mockImageService.Setup(x => x.IsImageFile(image)).Returns(true);
            mockImageService.Setup(x => x.SaveImage(image)).ReturnsAsync(avatarLink);

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
        public async void UploadPatientImageCommand_IsNotImageFailed_ReturnsImageUploadingStatus()
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
        public async void UploadPatientImageCommand_UploadFailed_ReturnsImageUploadingStatus()
        {
            //Arrange
            var patientId = -1;
            var avatarLink = "/Image/ed87619a-dfbf-4794-9cbb-bcecab7b44f3_avatar.jpg";
            var image = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a mock IFormFile")), 0, 0, "Data", "avatar.jpg");

            var mockImageService = new Mock<IImageService>();
            mockImageService.Setup(x => x.IsImageFile(image)).Returns(true);
            mockImageService.Setup(x => x.SaveImage(image)).ReturnsAsync(avatarLink);

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
        public async void UploadPatientImageCommand_ImageNull_ReturnsImageUploadingStatus()
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

        private PatientDetailViewModel ModelDataTest()
        {
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

            return model;
        }

        private DataAccess.Models.Patient GetPatientData()
        {
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

            return data;
        }
    }
}
