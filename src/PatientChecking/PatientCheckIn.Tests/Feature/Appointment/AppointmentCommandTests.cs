using Moq;
using PatientChecking.Feature.Appointment.Commands;
using PatientChecking.Services.Appointment;
using PatientChecking.Views.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace PatientCheckIn.Tests.Feature.Appointment
{
    public class AppointmentCommandTests
    {
        private AppointmentDetailViewModel AppointmentDataTest()
        {
            var appointment = new AppointmentDetailViewModel
            {
                AppointmentId = 1, 
                MedicalConcerns = "Head", 
                CheckInDate = "2021-08-26", 
                Status = "CheckIn", 
                PatientId = 1,
            };
            return appointment;
        }
        private DataAccess.Models.Appointment AppointmentDataAccess()
        {
            var appointment = new DataAccess.Models.Appointment
            {
                AppointmentId = 1,
                MedicalConcerns = "Lung",
                CheckInDate = new DateTime(2021, 08, 25),
                Status = "Closed",
                PatientId = 1,
            };
            return appointment;
        }

        [Fact]
        public async Task UpdateAppointmentCommand_ExistAppointment_ReturnNumberOfChangedLine()
        {
            //Arange
            var command = new UpdateAppointmentCommand
            {
                appointmentDetailViewModel = AppointmentDataTest()
            };
            var appointment = AppointmentDataAccess();
            var appointmentServices = new Mock<IAppointmentService>();
            appointmentServices.Setup(x => x.GetAppointmentByIdAsync(command.appointmentDetailViewModel.AppointmentId)).ReturnsAsync(appointment);
            appointmentServices.Setup(x => x.UpdateAppointmentAsync(appointment)).ReturnsAsync(1);
            var handler = new UpdateAppointmentCommandHandler(appointmentServices.Object);
            var cts = new CancellationToken();

            var expected = 1;

            //Act
            var actual = await handler.Handle(command, cts);

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task UpdateAppointmentCommand_NotFoundAppointment_ReturnNumberOfChangedLine()
        {
            //Arange
            var command = new UpdateAppointmentCommand
            {
                appointmentDetailViewModel = AppointmentDataTest()
            };
            var appointmentServices = new Mock<IAppointmentService>();
            appointmentServices.Setup(x => x.GetAppointmentByIdAsync(-1)).ReturnsAsync((DataAccess.Models.Appointment)null);
            var handler = new UpdateAppointmentCommandHandler(appointmentServices.Object);
            var cts = new CancellationToken();

            var expected = -1;

            //Act
            var actual = await handler.Handle(command, cts);

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task UpdateAppointmentCommand_InvalidCheckInDate_ReturnNumberOfChangedLine()
        {
            //Arange
            var command = new UpdateAppointmentCommand
            {
                appointmentDetailViewModel = AppointmentDataTest()
            };
            command.appointmentDetailViewModel.CheckInDate = "2021123-08-25"; //Invalid Date

            var appointment = AppointmentDataAccess();

            var appointmentServices = new Mock<IAppointmentService>();
            appointmentServices.Setup(x => x.GetAppointmentByIdAsync(command.appointmentDetailViewModel.AppointmentId)).ReturnsAsync(appointment);
            var handler = new UpdateAppointmentCommandHandler(appointmentServices.Object);
            var cts = new CancellationToken();

            var expected = -1;

            //Act
            var actual = await handler.Handle(command, cts);

            //Assert
            Assert.Equal(expected, actual);
        }
    }
}
