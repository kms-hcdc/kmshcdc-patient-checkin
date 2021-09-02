using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using PatientCheckIn.DataAccess.Models;
using PatientChecking.Controllers;
using PatientChecking.Feature.Appointment.Commands;
using PatientChecking.Feature.Appointment.Queries;
using PatientChecking.ServiceModels;
using PatientChecking.ServiceModels.Enum;
using PatientChecking.Views.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Xunit;

namespace PatientCheckIn.Tests.Controllers
{
    public class AppointmentControllerTests
    {
        private AppointmentListViewModel AppointmentDataTest()
        {
            var appointments = new List<AppointmentViewModel>
            {
                new AppointmentViewModel
                {
                    AppointmentId = 1, CheckInDate = new DateTime(2021, 08, 26).ToString("yyyy-MM-dd"), Status = "CheckIn",
                },
                new AppointmentViewModel
                {
                    AppointmentId = 2, CheckInDate = new DateTime(2021, 08, 27).ToString("yyyy-MM-dd"), Status = "Cancel", 
                },
                new AppointmentViewModel
                {
                    AppointmentId = 3,  CheckInDate = new DateTime(2021, 08, 25).ToString("yyyy-MM-dd"), Status = "Closed", 
                },
                new AppointmentViewModel
                {
                    AppointmentId = 4,  CheckInDate = new DateTime(2021, 08, 25).ToString("yyyy-MM-dd"), Status = "CheckIn", 
                }
            };
            var appointmentListViewModel = new AppointmentListViewModel
            {
                AppointmentViewModels = appointments,
                PageIndex = 1,
                PageSize = 4,
                SortSelection = 1,
                TotalCount = 4,
            };
            return appointmentListViewModel;
        }
        private AppointmentDetailViewModel AppointmentDetailViewModel()
        {
            var appointmentData = new AppointmentDetailViewModel
            {
                AppointmentId = 1,
                MedicalConcerns = "Head",
                CheckInDate = new DateTime(2021, 08, 26).ToString("yyyy-MM-dd"),
                Status = "CheckIn",
                PatientId = 1,
            };
            return appointmentData;
        }

        [Fact]
        public async void Detail_ReturnsDetailView()
        {
            //Arange
            var appointmentData = AppointmentDetailViewModel();
            var mediator = new Mock<IMediator>();
            mediator.Setup(x => x.Send(It.IsAny<GetAppointmentByIdQuery>(), new System.Threading.CancellationToken())).ReturnsAsync(appointmentData);
            var appointmentController = new AppointmentController(mediator.Object);

            var expected = appointmentData;

            //Act
            var actual = await appointmentController.Detail(1);

            //Assert
            var viewResult = Assert.IsType<Microsoft.AspNetCore.Mvc.ViewResult>(actual);
            var model = Assert.IsAssignableFrom<AppointmentDetailViewModel>(viewResult.ViewData.Model);
            Assert.Equal(expected.AppointmentId, model.AppointmentId);
            Assert.Equal(expected.MedicalConcerns, model.MedicalConcerns);
            Assert.Equal(expected.CheckInDate, model.CheckInDate);
            Assert.Equal(expected.Status, model.Status);
            Assert.Equal(expected.PatientId, model.PatientId);
        }
        [Fact]
        public async void Detail_NotFound_ReturnsRedirectToAction()
        {
            //Arange
            var mediator = new Mock<IMediator>();
            mediator.Setup(x => x.Send(It.IsAny<GetAppointmentByIdQuery>(), new System.Threading.CancellationToken())).ReturnsAsync((AppointmentDetailViewModel)null);
            var provider = new Mock<Microsoft.AspNetCore.Mvc.ViewFeatures.ITempDataProvider>();
            var tempData = new Microsoft.AspNetCore.Mvc.ViewFeatures.TempDataDictionary(new DefaultHttpContext(), provider.Object);
            
            var appointmentController = new AppointmentController(mediator.Object)
            {
                TempData = tempData,
            };

            //Act
            var actual = (RedirectToActionResult)await appointmentController.Detail(1000);
           
            //Assert
            Assert.Equal("Index", actual.ActionName);
        }

        [Fact]
        public async void Index_ThreeParams_ReturnsIndexView()
        {
            //Arange
            var data = AppointmentDataTest();
            var mediator = new Mock<IMediator>();
            mediator.Setup(x => x.Send(It.IsAny<GetAppointmentPagingQuery>(), new System.Threading.CancellationToken())).ReturnsAsync(data);
            var appointmentController = new AppointmentController(mediator.Object);

            var expected = data;

            //Act
            var actual = await appointmentController.Index(1,4,1);

            //Assert
            var viewResult = Assert.IsType<Microsoft.AspNetCore.Mvc.ViewResult>(actual);
            var model = Assert.IsAssignableFrom<AppointmentListViewModel>(viewResult.ViewData.Model);
            Assert.NotNull(model);
            Assert.Equal(expected.TotalCount, model.TotalCount);
            Assert.Equal(expected.AppointmentViewModels.Count, model.AppointmentViewModels.Count);
            Assert.True(expected.AppointmentViewModels.All(x => model.AppointmentViewModels.Any(y => x.AppointmentId == y.AppointmentId && x.CheckInDate == y.CheckInDate
                                                                                                  && x.DoB == y.DoB && x.FullName == y.FullName
                                                                                                  && x.PatientIdentifier == y.PatientIdentifier
                                                                                                  && x.Status == y.Status && x.AvatarLink == y.AvatarLink)));
        }

        [Fact]
        public async void Index_TwoParams_ReturnsThreeParamsIndexAction()
        {
            //Arange
            var data = AppointmentDataTest();
            var mediator = new Mock<IMediator>();
            mediator.Setup(x => x.Send(It.IsAny<GetAppointmentPagingQuery>(), new System.Threading.CancellationToken())).ReturnsAsync(data);
            var appointmentController = new AppointmentController(mediator.Object);

            var expected = data;

            //Act
            var actual = await appointmentController.Index(1, 4);

            //Assert
            var viewResult = Assert.IsType<Microsoft.AspNetCore.Mvc.ViewResult>(actual);
            var model = Assert.IsAssignableFrom<AppointmentListViewModel>(viewResult.ViewData.Model);
            Assert.NotNull(model);
            Assert.Equal(expected.TotalCount, model.TotalCount);
            Assert.Equal(expected.AppointmentViewModels.Count, model.AppointmentViewModels.Count);
            Assert.True(expected.AppointmentViewModels.All(x => model.AppointmentViewModels.Any(y => x.AppointmentId == y.AppointmentId && x.CheckInDate == y.CheckInDate
                                                                                                  && x.DoB == y.DoB && x.FullName == y.FullName
                                                                                                  && x.PatientIdentifier == y.PatientIdentifier
                                                                                                  && x.Status == y.Status && x.AvatarLink == y.AvatarLink)));
        }

        [Fact]
        public async void Index_OneParam_ReturnsThreeParamsIndexAction()
        {
            //Arange
            var data = AppointmentDataTest();
            var mediator = new Mock<IMediator>();
            mediator.Setup(x => x.Send(It.IsAny<GetAppointmentPagingQuery>(), new System.Threading.CancellationToken())).ReturnsAsync(data);
            var appointmentController = new AppointmentController(mediator.Object);

            var expected = data;

            //Act
            var actual = await appointmentController.Index(1);

            //Assert
            var viewResult = Assert.IsType<Microsoft.AspNetCore.Mvc.ViewResult>(actual);
            var model = Assert.IsAssignableFrom<AppointmentListViewModel>(viewResult.ViewData.Model);
            Assert.NotNull(model);
            Assert.Equal(expected.TotalCount, model.TotalCount);
            Assert.Equal(expected.AppointmentViewModels.Count, model.AppointmentViewModels.Count);
            Assert.True(expected.AppointmentViewModels.All(x => model.AppointmentViewModels.Any(y => x.AppointmentId == y.AppointmentId && x.CheckInDate == y.CheckInDate
                                                                                                  && x.DoB == y.DoB && x.FullName == y.FullName
                                                                                                  && x.PatientIdentifier == y.PatientIdentifier
                                                                                                  && x.Status == y.Status && x.AvatarLink == y.AvatarLink)));
        }

        [Fact]
        public async void Index_NoParam_ReturnsThreeParamsIndexAction()
        {
            //Arange
            var data = AppointmentDataTest();
            var mediator = new Mock<IMediator>();
            mediator.Setup(x => x.Send(It.IsAny<GetAppointmentPagingQuery>(), new System.Threading.CancellationToken())).ReturnsAsync(data);
            var appointmentController = new AppointmentController(mediator.Object);

            var expected = data;

            //Act
            var actual = await appointmentController.Index();

            //Assert
            var viewResult = Assert.IsType<Microsoft.AspNetCore.Mvc.ViewResult>(actual);
            var model = Assert.IsAssignableFrom<AppointmentListViewModel>(viewResult.ViewData.Model);
            Assert.NotNull(model);
            Assert.Equal(expected.TotalCount, model.TotalCount);
            Assert.Equal(expected.AppointmentViewModels.Count, model.AppointmentViewModels.Count);
            Assert.True(expected.AppointmentViewModels.All(x => model.AppointmentViewModels.Any(y => x.AppointmentId == y.AppointmentId && x.CheckInDate == y.CheckInDate
                                                                                                  && x.DoB == y.DoB && x.FullName == y.FullName
                                                                                                  && x.PatientIdentifier == y.PatientIdentifier
                                                                                                  && x.Status == y.Status && x.AvatarLink == y.AvatarLink)));
        }

        [Fact]
        public async void Update_ExistAppointment_ReturnsDetailAppointmentView()
        {
            //Arange
            var appointment = AppointmentDetailViewModel();
            var mediator = new Mock<IMediator>();
            mediator.Setup(x => x.Send(It.IsAny<UpdateAppointmentCommand>(), new System.Threading.CancellationToken())).ReturnsAsync(1);
            var provider = new Mock<Microsoft.AspNetCore.Mvc.ViewFeatures.ITempDataProvider>();
            var tempData = new Microsoft.AspNetCore.Mvc.ViewFeatures.TempDataDictionary(new DefaultHttpContext(), provider.Object);

            var appointmentController = new AppointmentController(mediator.Object)
            {
                TempData = tempData,
            };

            //Act
            var actual = (RedirectToActionResult)await appointmentController.Update(appointment);

            //Assert
            Assert.Equal("Detail", actual.ActionName);
            Assert.True(actual.RouteValues.Keys.Contains("appointmentId"));
            Assert.True(actual.RouteValues.Values.Contains(1));
        }

        [Fact]
        public async void Update_AppointmentNotFound_ReturnsDetailAppointmentView()
        {
            //Arange
            var appointment = AppointmentDetailViewModel();
            appointment.AppointmentId = 1000;
            var mediator = new Mock<IMediator>();
            mediator.Setup(x => x.Send(It.IsAny<UpdateAppointmentCommand>(), new System.Threading.CancellationToken())).ReturnsAsync(-1);
            var provider = new Mock<Microsoft.AspNetCore.Mvc.ViewFeatures.ITempDataProvider>();
            var tempData = new Microsoft.AspNetCore.Mvc.ViewFeatures.TempDataDictionary(new DefaultHttpContext(), provider.Object);

            var appointmentController = new AppointmentController(mediator.Object)
            {
                TempData = tempData,
            };

            //Act
            var actual = (RedirectToActionResult)await appointmentController.Update(appointment);

            //Assert
            Assert.Equal("Detail", actual.ActionName);
        }
    }
}
