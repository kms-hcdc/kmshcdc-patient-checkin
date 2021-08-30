using MediatR;
using PatientChecking.Services.Appointment;
using PatientChecking.Views.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PatientChecking.Feature.Appointment.Commands
{
    public class UpdateAppointmentCommand : IRequest<int>
    {
        public AppointmentDetailViewModel appointmentDetailViewModel { get; set; }
    }
    public class UpdateAppointmentCommandHandler : IRequestHandler<UpdateAppointmentCommand, int>
    {
        private readonly IAppointmentService _appointmentService;

        public UpdateAppointmentCommandHandler(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        public Task<int> Handle(UpdateAppointmentCommand request, CancellationToken cancellationToken)
        {
            var appointment = new PatientCheckIn.DataAccess.Models.Appointment
            {
                AppointmentId = request.appointmentDetailViewModel.AppointmentId,
                CheckInDate = DateTime.Parse(request.appointmentDetailViewModel.CheckInDate),
                MedicalConcerns = request.appointmentDetailViewModel.MedicalConcerns,
                PatientId = request.appointmentDetailViewModel.PatientId,
                Status = request.appointmentDetailViewModel.Status
            };
            return _appointmentService.UpdateAppointment(appointment);
        }
    }


}
