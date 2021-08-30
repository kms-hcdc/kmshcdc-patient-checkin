using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using Newtonsoft.Json;
using PatientCheckIn.DataAccess.Models;
using PatientChecking.Controllers;
using PatientChecking.Feature.Patient.Commands;
using PatientChecking.Feature.Patient.Queries;
using PatientChecking.ServiceModels;
using PatientChecking.ServiceModels.Enum;
using PatientChecking.Views.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace PatientCheckIn.Tests.Controllers
{
    public class PatientControllerTests
    {
        [Fact]
        public async void Index_ThreeParams()
        {
            //Arrange 
            var data = DataTest();
            var mediator = new Mock<IMediator>();
            mediator.Setup(x => x.Send(It.IsAny<GetAllPatientsPagingQuery>(), new CancellationToken())).ReturnsAsync(data);
            var patientController = new PatientController(mediator.Object);

            var expected = data;

            //Act
            var result = await patientController.Index(0, 4, 1);

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<PatientListViewModel>(viewResult.Model);
            Assert.Equal(expected.TotalCount, model.TotalCount);
            Assert.Equal(expected.SortSelection, model.SortSelection);
            Assert.Equal(expected.PageIndex, model.PageIndex);
            Assert.Equal(expected.PageSize, model.PageSize);
            for(int i = 0; i < expected.Patients.Count; i++)
            {
                Assert.Equal(expected.Patients[i].PatientId, model.Patients[i].PatientId);
                Assert.Equal(expected.Patients[i].PatientIdentifier, model.Patients[i].PatientIdentifier);
                Assert.Equal(expected.Patients[i].FullName, model.Patients[i].FullName);
                Assert.Equal(expected.Patients[i].Gender, model.Patients[i].Gender);
                Assert.Equal(expected.Patients[i].DoB, model.Patients[i].DoB);
                Assert.Equal(expected.Patients[i].AvatarLink, model.Patients[i].AvatarLink);
                Assert.Equal(expected.Patients[i].Address, model.Patients[i].Address);
                Assert.Equal(expected.Patients[i].Email, model.Patients[i].Email);
                Assert.Equal(expected.Patients[i].PhoneNumber, model.Patients[i].PhoneNumber);
            }
        }

        [Fact]
        public async void Index_TwoParams()
        {
            //Arrange 
            var data = DataTest();
            var mediator = new Mock<IMediator>();
            mediator.Setup(x => x.Send(It.IsAny<GetAllPatientsPagingQuery>(), new CancellationToken())).ReturnsAsync(data);
            var patientController = new PatientController(mediator.Object);

            var expected = data;

            //Act
            var result = await patientController.Index(0, 4);

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<PatientListViewModel>(viewResult.Model);
            Assert.Equal(expected.TotalCount, model.TotalCount);
            Assert.Equal(expected.SortSelection, model.SortSelection);
            Assert.Equal(expected.PageIndex, model.PageIndex);
            Assert.Equal(expected.PageSize, model.PageSize);
            for (int i = 0; i < expected.Patients.Count; i++)
            {
                Assert.Equal(expected.Patients[i].PatientId, model.Patients[i].PatientId);
                Assert.Equal(expected.Patients[i].PatientIdentifier, model.Patients[i].PatientIdentifier);
                Assert.Equal(expected.Patients[i].FullName, model.Patients[i].FullName);
                Assert.Equal(expected.Patients[i].Gender, model.Patients[i].Gender);
                Assert.Equal(expected.Patients[i].DoB, model.Patients[i].DoB);
                Assert.Equal(expected.Patients[i].AvatarLink, model.Patients[i].AvatarLink);
                Assert.Equal(expected.Patients[i].Address, model.Patients[i].Address);
                Assert.Equal(expected.Patients[i].Email, model.Patients[i].Email);
                Assert.Equal(expected.Patients[i].PhoneNumber, model.Patients[i].PhoneNumber);
            }
        }

        [Fact]
        public async void Index_OneParam()
        {
            //Arrange 
            var data = DataTest();
            var mediator = new Mock<IMediator>();
            mediator.Setup(x => x.Send(It.IsAny<GetAllPatientsPagingQuery>(), new CancellationToken())).ReturnsAsync(data);
            var patientController = new PatientController(mediator.Object);

            var expected = data;

            //Act
            var result = await patientController.Index(0);

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<PatientListViewModel>(viewResult.Model);
            Assert.Equal(expected.TotalCount, model.TotalCount);
            Assert.Equal(expected.SortSelection, model.SortSelection);
            Assert.Equal(expected.PageIndex, model.PageIndex);
            Assert.Equal(expected.PageSize, model.PageSize);
            for (int i = 0; i < expected.Patients.Count; i++)
            {
                Assert.Equal(expected.Patients[i].PatientId, model.Patients[i].PatientId);
                Assert.Equal(expected.Patients[i].PatientIdentifier, model.Patients[i].PatientIdentifier);
                Assert.Equal(expected.Patients[i].FullName, model.Patients[i].FullName);
                Assert.Equal(expected.Patients[i].Gender, model.Patients[i].Gender);
                Assert.Equal(expected.Patients[i].DoB, model.Patients[i].DoB);
                Assert.Equal(expected.Patients[i].AvatarLink, model.Patients[i].AvatarLink);
                Assert.Equal(expected.Patients[i].Address, model.Patients[i].Address);
                Assert.Equal(expected.Patients[i].Email, model.Patients[i].Email);
                Assert.Equal(expected.Patients[i].PhoneNumber, model.Patients[i].PhoneNumber);
            }
        }

        [Fact]
        public async void Index_NoParam()
        {
            //Arrange 
            var data = DataTest();
            var mediator = new Mock<IMediator>();
            mediator.Setup(x => x.Send(It.IsAny<GetAllPatientsPagingQuery>(), new CancellationToken())).ReturnsAsync(data);
            var patientController = new PatientController(mediator.Object);

            var expected = data;

            //Act
            var result = await patientController.Index();

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<PatientListViewModel>(viewResult.Model);
            Assert.Equal(expected.TotalCount, model.TotalCount);
            Assert.Equal(expected.SortSelection, model.SortSelection);
            Assert.Equal(expected.PageIndex, model.PageIndex);
            Assert.Equal(expected.PageSize, model.PageSize);
            for (int i = 0; i < expected.Patients.Count; i++)
            {
                Assert.Equal(expected.Patients[i].PatientId, model.Patients[i].PatientId);
                Assert.Equal(expected.Patients[i].PatientIdentifier, model.Patients[i].PatientIdentifier);
                Assert.Equal(expected.Patients[i].FullName, model.Patients[i].FullName);
                Assert.Equal(expected.Patients[i].Gender, model.Patients[i].Gender);
                Assert.Equal(expected.Patients[i].DoB, model.Patients[i].DoB);
                Assert.Equal(expected.Patients[i].AvatarLink, model.Patients[i].AvatarLink);
                Assert.Equal(expected.Patients[i].Address, model.Patients[i].Address);
                Assert.Equal(expected.Patients[i].Email, model.Patients[i].Email);
                Assert.Equal(expected.Patients[i].PhoneNumber, model.Patients[i].PhoneNumber);
            }
        }

        [Fact]
        public async void Detail()
        {
            //Arrange
            var patient = PatientDetailDataTest();

            var mediator = new Mock<IMediator>();
            mediator.Setup(x => x.Send(It.IsAny<GetPatientInDetailByIdQuery>(), new CancellationToken())).ReturnsAsync(patient);
            var patientController = new PatientController(mediator.Object);

            var expected = patient;

            //Act
            var result = await patientController.Detail(patient.PatientId);

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<PatientDetailViewModel>(viewResult.Model);
            Assert.NotNull(model);
            Assert.Equal(expected.PatientId, model.PatientId);
            Assert.Equal(expected.PatientIdentifier, model.PatientIdentifier);
            Assert.Equal(expected.FirstName, model.FirstName);
            Assert.Equal(expected.MiddleName, model.MiddleName);
            Assert.Equal(expected.LastName, model.LastName);
            Assert.Equal(expected.FullName, model.FullName);
            Assert.Equal(expected.Nationality, model.Nationality);
            Assert.Equal(expected.DoB, model.DoB);
            Assert.Equal(expected.MaritalStatus, model.MaritalStatus);
            Assert.Equal(expected.Gender, model.Gender);
            Assert.Equal(expected.AvatarLink, model.AvatarLink);
            Assert.Equal(expected.Email, model.Email);
            Assert.Equal(expected.PhoneNumber, model.PhoneNumber);
            Assert.Equal(expected.EthnicRace, model.EthnicRace);
            Assert.Equal(expected.HomeTown, model.HomeTown);
            Assert.Equal(expected.BirthplaceCity, model.BirthplaceCity);
            Assert.Equal(expected.IdcardNo, model.IdcardNo);
            Assert.Equal(expected.IssuedDate, model.IssuedDate);
            Assert.Equal(expected.IssuedPlace, model.IssuedPlace);
            Assert.Equal(expected.InsuranceNo, model.InsuranceNo);
            for (int i = 0; i < expected.ProvinceCities.Count; i++)
            {
                Assert.Equal(expected.ProvinceCities[i], model.ProvinceCities[i]);
            }
        }

        [Fact]
        public async void Update_Ok()
        {
            //Arrange 
            var patient = PatientDetailDataTest();

            var mediator = new Mock<IMediator>();
            mediator.Setup(x => x.Send(It.IsAny<UpdatePatientInformationCommand>(), new CancellationToken())).ReturnsAsync(1);

            var provider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), provider.Object);

            var patientController = new PatientController(mediator.Object)
            {
                TempData = tempData
            };

            //Act
            var result = await patientController.Update(patient);

            //Assert
            var redirectToAction = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Detail", redirectToAction.ActionName);
            Assert.True(redirectToAction.RouteValues.Keys.Contains("patientId"));
            Assert.True(redirectToAction.RouteValues.Values.Contains(patient.PatientId));
        }

        [Fact]
        public async void Update_NotOk()
        {
            //Arrange 
            var patient = PatientDetailDataTest();

            var mediator = new Mock<IMediator>();
            mediator.Setup(x => x.Send(It.IsAny<UpdatePatientInformationCommand>(), new CancellationToken())).ReturnsAsync(-1);

            var provider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), provider.Object);

            var patientController = new PatientController(mediator.Object)
            {
                TempData = tempData
            };

            //Act
            var result = await patientController.Update(patient);

            //Assert
            var redirectToAction = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Detail", redirectToAction.ActionName);
            Assert.True(redirectToAction.RouteValues.Keys.Contains("patientId"));
            Assert.True(redirectToAction.RouteValues.Values.Contains(patient.PatientId));
        }

        [Fact]
        public async void UploadImage_IsNotImageFailed()
        {
            //Arrange 
            var patient = PatientDetailDataTest();
            var image = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a mock IFormFile")), 0, 0, "Data", "avatar.pdf");

            var mediator = new Mock<IMediator>();
            mediator.Setup(x => x.Send(It.IsAny<UploadPatientImageCommand>(), new CancellationToken())).ReturnsAsync(ImageUploadingStatus.IsNotImageFailed);

            var provider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), provider.Object);

            var patientController = new PatientController(mediator.Object)
            {
                TempData = tempData
            };

            //Act
            var result = await patientController.UploadImage(patient.PatientId, image);

            //Assert
            var redirectToAction = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Detail", redirectToAction.ActionName);
            Assert.True(redirectToAction.RouteValues.Keys.Contains("patientId"));
            Assert.True(redirectToAction.RouteValues.Values.Contains(patient.PatientId));
        }

        [Fact]
        public async void UploadImage_UploadImageFailed()
        {
            //Arrange 
            var patient = PatientDetailDataTest();
            var image = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a mock IFormFile")), 0, 0, "Data", "avatar.pdf");

            var mediator = new Mock<IMediator>();
            mediator.Setup(x => x.Send(It.IsAny<UploadPatientImageCommand>(), new CancellationToken())).ReturnsAsync(ImageUploadingStatus.UploadImageFailed);

            var provider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), provider.Object);

            var patientController = new PatientController(mediator.Object)
            {
                TempData = tempData
            };

            //Act
            var result = await patientController.UploadImage(patient.PatientId, image);

            //Assert
            var redirectToAction = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Detail", redirectToAction.ActionName);
            Assert.True(redirectToAction.RouteValues.Keys.Contains("patientId"));
            Assert.True(redirectToAction.RouteValues.Values.Contains(patient.PatientId));
        }

        [Fact]
        public async void UploadImage_UploadImageSuccessfully()
        {
            //Arrange 
            var patient = PatientDetailDataTest();
            var image = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a mock IFormFile")), 0, 0, "Data", "avatar.pdf");

            var mediator = new Mock<IMediator>();
            mediator.Setup(x => x.Send(It.IsAny<UploadPatientImageCommand>(), new CancellationToken())).ReturnsAsync(ImageUploadingStatus.UploadImageSuccessfully);

            var provider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), provider.Object);

            var patientController = new PatientController(mediator.Object)
            {
                TempData = tempData
            };

            //Act
            var result = await patientController.UploadImage(patient.PatientId, image);

            //Assert
            var redirectToAction = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Detail", redirectToAction.ActionName);
            Assert.True(redirectToAction.RouteValues.Keys.Contains("patientId"));
            Assert.True(redirectToAction.RouteValues.Values.Contains(patient.PatientId));
        }

        [Fact]
        public async Task UploadImage_IOException()
        {
            //Arrange 
            var patient = PatientDetailDataTest();
            var image = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a mock IFormFile")), 0, 0, "Data", "avatar.pdf");

            var mediator = new Mock<IMediator>();
            mediator.Setup(x => x.Send(It.IsAny<UploadPatientImageCommand>(), new CancellationToken())).Throws(new IOException());

            var provider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), provider.Object);

            var patientController = new PatientController(mediator.Object)
            {
                TempData = tempData
            };

            //Act
            var result = await patientController.UploadImage(patient.PatientId, image);

            //Assert
            var redirectToAction = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Detail", redirectToAction.ActionName);
            Assert.True(redirectToAction.RouteValues.Keys.Contains("patientId"));
            Assert.True(redirectToAction.RouteValues.Values.Contains(patient.PatientId));
        }

        private PatientListViewModel DataTest()
        {
            var patientsVm = new List<PatientViewModel>() {
                new PatientViewModel
                {
                    PatientId = 1,
                    PatientIdentifier = "KMS.0001",
                    FullName = "Long Thanh Do",
                    Gender = "Male",
                    DoB = "11-09-1999",
                    AvatarLink = "/Image/avatar.jpg",
                    Address = "Hoa Son, Hoa Vang, Da Nang",
                    Email = "longtdo@kms-technology.com",
                    PhoneNumber = "0905231436"
                },
                new PatientViewModel
                {
                    PatientId = 2,
                    PatientIdentifier = "KMS.0002",
                    FullName = "Duc Van Tran",
                    Gender = "Male",
                    DoB = "05-10-1999",
                    AvatarLink = "/Image/duck.jpg",
                    Address = "Hoa Son, Hoa Vang, Da Nang",
                    Email = "ducvant@kms-technology.com",
                    PhoneNumber = "0905231555"
                },
                new PatientViewModel
                {
                    PatientId = 3,
                    PatientIdentifier = "KMS.0003",
                    FullName = "Phien Minh Le",
                    Gender = "Male",
                    DoB = "06-12-1987",
                    AvatarLink = "/Image/phienle.jpg",
                    Address = "Quan 1, Ho Chi Minh",
                    Email = "phienle@kms-technology.com",
                    PhoneNumber = "0905444222"
                },
                new PatientViewModel
                {
                    PatientId = 4,
                    PatientIdentifier = "KMS.0004",
                    FullName = "Viet Hoang Vo",
                    Gender = "Male",
                    DoB = "10-08-1995",
                    AvatarLink = "/Image/vietvo.jpg",
                    Address = "Quan 10, Ho Chi Minh",
                    Email = "vietvo@kms-technology.com",
                    PhoneNumber = "0905333888"
                }
            };

            var data = new PatientListViewModel
            {
                Patients = patientsVm,
                SortSelection = 0,
                PageIndex = 1,
                PageSize = 4,
                TotalCount = 10
            };

            return data; 
        }
        private List<ProvinceCity> ProvinceCityDataTest()
        {
            var provinceCities = new List<ProvinceCity>
            {
                new ProvinceCity
                {
                   ProvinceCityId = 1, ProvinceCityName = "An Giang"
                },
                new ProvinceCity
                {
                   ProvinceCityId = 2, ProvinceCityName = "Bà rịa – Vung tàu"
                },
                new ProvinceCity
                {
                   ProvinceCityId = 3, ProvinceCityName = "Bắc Giang"
                },
                new ProvinceCity
                {
                   ProvinceCityId = 4, ProvinceCityName = "Bắc Kạn"
                },
                new ProvinceCity
                {
                   ProvinceCityId = 5, ProvinceCityName = "Bạc Liêu"
                },
            };
            return provinceCities;
        }

        private PatientDetailViewModel PatientDetailDataTest()
        {
            var cities = ProvinceCityDataTest();
            var cityList = new List<string>();

            foreach (ProvinceCity city in cities)
            {
                cityList.Add(city.ProvinceCityName);
            }

            var patient = new PatientDetailViewModel
            {
                PatientId = 1,
                PatientIdentifier = "KMS.0001",
                FirstName = "Long",
                LastName = "Do",
                MiddleName = "Thanh",
                FullName = "Long Thanh Do",
                Nationality = "Vietnamese",
                DoB = "1999-11-09",
                MaritalStatus = 0,
                Gender = 0,
                AvatarLink = "/Image/avatar.jpg",
                Email = "longtdo@kms-technogy.com",
                PhoneNumber = "0905546232",
                EthnicRace = "Kinh",
                HomeTown = "Da Nang",
                BirthplaceCity = "Ho Chi Minh",
                IdcardNo = "201723566",
                IssuedDate = "2013-08-06",
                IssuedPlace = "Da Nang",
                InsuranceNo = "2030455663",
                ProvinceCities = cityList
            };
            return patient;
        }
    }
}
