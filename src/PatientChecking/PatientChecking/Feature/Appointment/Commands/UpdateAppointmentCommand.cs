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

        public async Task<int> Handle(UpdateAppointmentCommand request, CancellationToken cancellationToken)
        {
            DateTime temp;
            var appointment = await _appointmentService.GetAppointmentById(request.appointmentDetailViewModel.AppointmentId);
            if(DateTime.TryParse(request.appointmentDetailViewModel.CheckInDate,out temp))
            {
                if (appointment != null)
                {
                    appointment.CheckInDate = DateTime.Parse(request.appointmentDetailViewModel.CheckInDate);
                    appointment.MedicalConcerns = request.appointmentDetailViewModel.MedicalConcerns;
                    appointment.Status = request.appointmentDetailViewModel.Status;
                    return await _appointmentService.UpdateAppointment(appointment);
                }
            }
            return -1;
        }
    }


}
