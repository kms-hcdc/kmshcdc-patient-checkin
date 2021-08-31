using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using PatientChecking.Controllers;
using PatientChecking.Feature.Dashboard.Queries;
using PatientChecking.Views.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PatientCheckIn.Tests.Controllers
{
    public class DashboardControllerTests
    {
        private DashboardViewModel DashboardViewModel()
        {
            var dashboardData = new DashboardViewModel
            {
                NumOfAppointments = 4,
                NumOfAppointmentsInMonth = 4,
                NumOfAppointmentsInToday = 1,
                NumOfPatients = 4,
                NumOfPatientsInMonth = 4
            };
            return dashboardData;
        }
        [Fact]
        public async void GetDashboardData_ReturnsJsonResult()
        {
            //Arrage
            var dashboardData = DashboardViewModel();

            var mediator = new Mock<IMediator>();
            mediator.Setup(x => x.Send(It.IsAny<GetDashBoardDataQuery>(), new System.Threading.CancellationToken())).ReturnsAsync(dashboardData);
            var dashboardController = new DashboardController(mediator.Object);

            var expected = dashboardData;

            //Act
            var actual = await dashboardController.GetDashBoardData();

            //Assert
            var jsonResultDashboard = Assert.IsType<JsonResult>(actual);
            string data = JsonConvert.SerializeObject(jsonResultDashboard.Value);
            var dashboardViewModelData = JsonConvert.DeserializeObject<DashboardViewModel>(data);
            Assert.Equal(expected.NumOfAppointments, dashboardViewModelData.NumOfAppointments);
            Assert.Equal(expected.NumOfAppointmentsInMonth, dashboardViewModelData.NumOfAppointmentsInMonth);
            Assert.Equal(expected.NumOfAppointmentsInToday, dashboardViewModelData.NumOfAppointmentsInToday);
            Assert.Equal(expected.NumOfPatients, dashboardViewModelData.NumOfPatients);
            Assert.Equal(expected.NumOfPatientsInMonth, dashboardViewModelData.NumOfPatientsInMonth);
        }

        [Fact]
        public void Home_ReturnsHomeView()
        {
            //Arrage
            var mediator = new Mock<IMediator>();
            var dashboardController = new DashboardController(mediator.Object);

            //Act
            var actual = dashboardController.Home();

            //Assert
            Assert.IsType<ViewResult>(actual);
        }
    }
}
